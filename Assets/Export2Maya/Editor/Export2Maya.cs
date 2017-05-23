using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
 
// --------------------------------------------------
// Export2Maya ( Version 2.2.1 )
// By:	Michael Cook
// --------------------------------------------------
// IN PROGRESS:
//
// TO DO LIST:
//		Make the material export copy ALL textures it finds from the material
//		Add detail meshes + grass export for Terrains
//		Split the scripts up maybe to make it more organizable?
//		Animation Export
//		Look into being able to call Export2Maya from in game
//
// COMPLETED:
//		Fixed Material export to account gameobjects with no mesh renderer
//		Exporter Re-Written from the ground up
//			Much more organized than before
//		Standard Mesh Export
//			New Mesh Edge Generation - 3x faster!
//				Using StringBuilder to build strings of edges
//				Using List.BinarySearch for speed improvements
//			UV Export
//				Normal UV Export
//				Secondary UV Export
//		Skinned Mesh Export
//			Same features as Standard Mesh Export
//		Blendshapes
//			Single Frame Blendshape Support
//			Multi-Frame Blendshape Support
//		Material Export
//		Camera Export
//		Lights Export
//		Terrain Export
//			Standard Terrain Mesh Export
//			Standard Terrain UV Export
//			Standard Terrain Secondary UV Export
//			Standard Terrain Textures Export
//			Standard Terrain Splatmaps Export
//		Display Layer Export
//
// KNOWN ISSUES:
//		Camera export can be a bit unpredictable
//		BindPoses cannot be exported due to Unity limitations
//		Terrain Detail Mesh + Grass are not supported yet
// 
public class Export2Maya : EditorWindow {
	// --------------------------------------------------
	// Export2Maya Global Variables
	// --------------------------------------------------
	static string ExportPath;
	List<GameObject> SelectedObjects;				// The selected objects
	List<MayaNode> DAGNodes;						// List of GameObjects converted to MayaNodes
	List<MayaNode> DAGNodesBuffer;
	List<MayaNode> AUXNodes;						// Extra MayaNode list, used when creating auxiliary nodes
	List<MayaNode> MaterialNodes;					// List of materials used by the selected MayaNodes
	List<mDisplayLayer> DisplayLayerList;			// List of Display Layers
	StringBuilder Connections;						// Node connection list
	
	Dictionary<string, int> DAGNames;				// List of registered names of DAGNodes and the current name count
	Dictionary<string, int> GlobalNames;			// List of registered names of ALL nodes and the current name count
	
	int ColumnDataWidth = 5;						// Used to format the data in the Maya ASCII file into columns
	
	string ProgressTitle;							// ProgressBar Title
	float CurrentProgress;							// ProgressBar Current Progress
	float MaxProgress;								// ProgressBar Max Progress
	
	// --------------------------------------------------
	// Export2Maya UI variables
	// --------------------------------------------------
	string[] MayaVersions = new string[]{ "2016", "2015", "2014", "2013.5", "2013", "2012" };
	int MayaVersionIndex = 0;
	string[] MayaUnits = new string[]{ "millimeter", "centimeter", "meter", "inch", "foot", "yard" };
	int MayaUnitsIndex = 1;
	
	// --------------------------------------------------
	// Export2Maya GUI
	// --------------------------------------------------
	void OnGUI(){
		GUILayout.Label("Maya Version:", EditorStyles.boldLabel);
			MayaVersionIndex = EditorGUILayout.Popup(MayaVersionIndex, MayaVersions, GUILayout.MaxWidth(100));
		GUILayout.Label("Maya Units:", EditorStyles.boldLabel);
			MayaUnitsIndex = EditorGUILayout.Popup(MayaUnitsIndex, MayaUnits, GUILayout.MaxWidth(100));
		GUILayout.Label("Begin Export:", EditorStyles.boldLabel);
		if(GUILayout.Button("Export Selection", GUILayout.Height(22))) ExportMaya();
		GUI.enabled = false;
		GUILayout.Label("Export2Maya - ver 2.2.1", EditorStyles.miniLabel);
		GUI.enabled = true;
	}
	
	// --------------------------------------------------
	// Export2Maya Main
	// --------------------------------------------------
	#region The Main Entry point
	void ExportMaya(){
		// --------------------------------------------------
		// Initialize Variables
		// --------------------------------------------------
		ResetVariables();
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		// --------------------------------------------------
		// Variables
		// --------------------------------------------------
		string filePath = "";
		string fileName = "";

		// --------------------------------------------------
		// Grab the selected objects
		// --------------------------------------------------
		// Note - We do it this way because of joints, and 
		// having to dynamically add to the SelectedObjects
		// list during Joint Sweep
		if(Selection.gameObjects.Length < 1){
			EditorUtility.DisplayDialog("Nothing to Export!", "Please select the GameObjects you wish to export and try again.", "Ok");
			return;
		}
		BuildSelectedObjectsList();
				
		// --------------------------------------------------
		// File Save Prompt
		// --------------------------------------------------
		// Prompt the user where to save the Maya file
		fileName = EditorUtility.SaveFilePanel("Export Maya ASCII File", ExportPath, "", "ma");
		// If they cancel without saving, abort the export
		if(fileName == "") return;
		// Split out file name and file path
		string[] tokens = fileName.Split('/');
		filePath = "";
		for(int i=0; i<tokens.Length - 1; i++) filePath += tokens[i] + "/";
		fileName = tokens[tokens.Length - 1];
		ExportPath = filePath;

		// --------------------------------------------------
		// Start new file
		// --------------------------------------------------
		StartNewFile(filePath, fileName);

		// --------------------------------------------------
		// Convert GameObjects -> MayaNodes based on User selection
		// --------------------------------------------------
		EditorUtility.DisplayProgressBar(ProgressTitle, "Converting Selection to Maya Nodes", 0);
		SelectionToMayaNodes();
		
		// --------------------------------------------------
		// Joint Sweep
		// --------------------------------------------------
		// Since Unity doesn't have a "joint" class we have to
		// reverse query to find the bones by first finding all
		// the selected skinned mesh renderers, then see which
		// transforms are attached. We can then mark those as joints.
		//
		// We are double checking though, that if we select a
		// SkinnedMeshRenderer and not the joints that belong to it
		// we find the joints and rebuild the DAGList to include them
		FindTheJoints();
		FindTheJoints();
	
		// --------------------------------------------------
		// Connect MayaNodes -> MayaNodes based on Parent and Children
		// --------------------------------------------------
		// Go through selection again, this time populating the
		// MayaNodes connection data ( parents and children )
		EditorUtility.DisplayProgressBar(ProgressTitle, "Connecting Maya Nodes", 0);
		CreateMayaNodeConnections();
		
		// --------------------------------------------------
		// Initial DAGNode rename
		// --------------------------------------------------
		// This is a bit hacky, but right after we build the MayaNode
		// transforms, we need to insure that their names are unique 
		// before we expand the nodes. Since we are creating the materials
		// next and we need connection data with correctly renamed names, if we don't
		// rename now the connections could point to old DAGNode names that
		// don't exist any more. We reset the list afterwards to reset the DAGNode
		// name counts so the object counts don't keep going up and up
		RenameDAGNodes();
		DAGNames = new Dictionary<string, int>();
		
		// --------------------------------------------------
		// Create Materials and Textures
		// --------------------------------------------------
		// We need to create the materials first before we expand the DAGNodes
		EditorUtility.DisplayProgressBar(ProgressTitle, "Creating Materials", 0);
		CreateMaterials();

		// --------------------------------------------------
		// Expand MayaNodes (Create Auxiliary Nodes)
		// --------------------------------------------------
		// Expand each MayaNode into a full node network and record
		// connections between the nodes
		EditorUtility.DisplayProgressBar(ProgressTitle, "Expanding Maya Nodes", 0);
		ExpandNodes();

		// --------------------------------------------------
		// Reorder the DAGNodes so they are in order in our List
		// based on hierarchy. This saves us from having to use
		// recursive functions everywhere
		// --------------------------------------------------
		EditorUtility.DisplayProgressBar(ProgressTitle, "Reordering Maya Nodes", 0);
		ReorderDAGNodes();

		// --------------------------------------------------
		// Rename DAGNodes using DAG Path Again ...
		// --------------------------------------------------
		// Again, go through DAGNodes and assign a unique Maya 
		// name to avoid name clashes
		EditorUtility.DisplayProgressBar(ProgressTitle, "Renaming Maya Objects", 0);
		RenameDAGNodes();
		
		// --------------------------------------------------
		// Create Display Layers
		// --------------------------------------------------
		EditorUtility.DisplayProgressBar(ProgressTitle, "Creating Display Layers", 0);
		CreateDisplayLayers();
						
		// --------------------------------------------------
		// Initialize Progress Bar
		// --------------------------------------------------		
		CurrentProgress = 0.0f;
		MaxProgress = DAGNodes.Count + AUXNodes.Count + MaterialNodes.Count;		
	
		
		// --------------------------------------------------
		// Write DAGNodes to file
		// --------------------------------------------------
		WriteDAGNodes(filePath, fileName);
		
		// --------------------------------------------------
		// Write AUXNodes to file
		// --------------------------------------------------
		WriteAUXNodes(filePath, fileName);
		
		// --------------------------------------------------
		// Write Display Layer Manager and Display Layers
		// --------------------------------------------------
		WriteDisplayLayers(filePath, fileName);
		
		// --------------------------------------------------
		// Write LightLinker
		// --------------------------------------------------
		WriteLightLinker(filePath, fileName);
		
		// --------------------------------------------------
		// Write RenderPartition
		// --------------------------------------------------		
		WriteRenderPartition(filePath, fileName);
		
		// --------------------------------------------------
		// Write DefaultShaderList
		// --------------------------------------------------
		WriteDefaultShaderList(filePath, fileName);

		// --------------------------------------------------
		// Write DefaultTextureList
		// --------------------------------------------------
		WriteDefaultRenderUtilityList(filePath, fileName);
		
		// --------------------------------------------------
		// Write InitialShadingGroup Setup
		// --------------------------------------------------
		WriteInitialShadingGroupSetup(filePath, fileName);
	
		// --------------------------------------------------
		// Write Materials to file
		// --------------------------------------------------
		WriteMaterials(filePath, fileName);

		// --------------------------------------------------
		// Write Connections to file
		// --------------------------------------------------
		EditorUtility.DisplayProgressBar(ProgressTitle, "Writing Connections", 0);
		WriteConnections(filePath, fileName);
		
		// --------------------------------------------------
		// Copy Textures
		// --------------------------------------------------
		CopyTextures(filePath, fileName);
	
		// --------------------------------------------------
		// Clear Progress Bar
		// --------------------------------------------------
		EditorUtility.ClearProgressBar();
		
		// --------------------------------------------------
		// Display output info
		// --------------------------------------------------
		stopwatch.Stop();
		string NiceFileName = filePath + fileName;
		NiceFileName = NiceFileName.Replace("/","\\");
		UnityEngine.Debug.Log("[Export2Maya]: Exported (" + NiceFileName + ") in " + stopwatch.ElapsedMilliseconds + " ms\n\n");
	}
	#endregion
	
	#region Get User Selection
	// --------------------------------------------------
	// Build Selected Objects List
	// --------------------------------------------------
	// We reference the selected objects into our own list
	// because we might have to dynamically add to the list
	// depending on what the user has selected. 
	void BuildSelectedObjectsList(){
		SelectedObjects = new List<GameObject>();
		for(int i=0; i<Selection.gameObjects.Length; i++) SelectedObjects.Add(Selection.gameObjects[i]);
	}
	#endregion
	
	// --------------------------------------------------
	// Get Unity Object Type
	// --------------------------------------------------
	// Given an MayaNode, this will query known components
	// and best guess what "type" of object the UnityObject is
	string GetUnityObjectType(MayaNode Node){
		// --------------------------------------------------
		// Joint Check (Needs to happen first)
		// --------------------------------------------------
		// Since Unity doesn't have joints, and we already
		// preprocessed the joints first, check that Type isn't
		// already a joint. If it is, bail out
		if(Node is mJoint) return "Joint";
		
		// --------------------------------------------------
		// Mesh Check
		// --------------------------------------------------
		MeshFilter MeshCheck = Node.UnityObject.gameObject.GetComponent<MeshFilter>();
		if(MeshCheck != null){
			// Just because there is a mesh filter attached to it doesn't mean there is a mesh
			// linked to it. Check that there is actually a shared mesh
			Mesh SharedMeshCheck = MeshCheck.sharedMesh;
			if(SharedMeshCheck != null){
				// Finally, there might be a mesh, but no data (procedural meshes), so check that there is
				// actually data
				if(SharedMeshCheck.vertices.Length > 2) return "Mesh";
			}
		}
		
		// --------------------------------------------------
		// Skinned Mesh Check
		// --------------------------------------------------
		SkinnedMeshRenderer SkinnedMeshCheck = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>();
		if(SkinnedMeshCheck != null){
			// Just because there is a mesh filter it doesn't mean there is a mesh
			// linked to it. Check that there is actually a shared mesh
			Mesh SharedMeshCheck = SkinnedMeshCheck.sharedMesh;
			if(SharedMeshCheck != null) return "SkinnedMesh";
		}
		
		// --------------------------------------------------
		// Light Check
		// --------------------------------------------------
		Light LightCheck = Node.UnityObject.gameObject.GetComponent<Light>();
		if(LightCheck != null){
			// Determine Light Type (spot, point, directional, area)
			if(LightCheck.type == LightType.Spot) return "SpotLight";
			if(LightCheck.type == LightType.Directional) return "DirectionalLight";
			if(LightCheck.type == LightType.Point) return "PointLight";
			if(LightCheck.type == LightType.Area) return "AreaLight";
		}
		
		// --------------------------------------------------
		// Terrain Check
		// --------------------------------------------------
		Terrain TerrainCheck = Node.UnityObject.gameObject.GetComponent<Terrain>();
		if(TerrainCheck != null){
			return "Terrain";
		}
		
		// --------------------------------------------------
		// Camera Check
		// --------------------------------------------------
		Camera CameraCheck = Node.UnityObject.gameObject.GetComponent<Camera>();
		if(CameraCheck != null){
			return "Camera";
		}

		// --------------------------------------------------
		// Final Null Check
		// --------------------------------------------------
		return "Null";
	}

	#region Find Joint Transforms
	// --------------------------------------------------
	// Find Which Transforms are being used as joints
	// --------------------------------------------------
	// Unity doesn't have a "joint" object, so we need to
	// determine which transforms are actually joints by
	// going through all the selected SkinnedMeshRenderers
	// and see which transforms are connected.
	void FindTheJoints(){
		// Used to check if SelectedObjects count changes in the
		// event we need to include more bones than what the user
		// originally selected
		int SelectedObjectsCount = SelectedObjects.Count;

		// Go through every MayaNode
		for(int i=0; i<DAGNodes.Count; i++){
			// If Shape == false, skip it
			if(DAGNodes[i].Shape == false) continue;

			// If the UnityObject is a skinned mesh
			if(GetUnityObjectType(DAGNodes[i]) == "SkinnedMesh"){
				Transform[] joints = DAGNodes[i].UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().bones;
				for(int j=0; j<joints.Length; j++){
					// Try to find the associated MayaNode
					int JointIndex = GetMayaNodeFromTransform(joints[j]);

					// If the joint existed in our DAGNodes already, that means
					// the user had manually selected it or the joints parent, so
					// just switch the Type to MayaNodeType.joint
					if(JointIndex != -1){
						mJoint joint = new mJoint();
						joint.Shape = true;
						joint.UnityObject = DAGNodes[JointIndex].UnityObject;
						joint.MayaName = CleanName(DAGNodes[JointIndex].UnityObject.name);
						DAGNodes[JointIndex] = joint;
					}
					// If it wasn't found, then the user didn't select the joint and
					// we have to manually add it and rebuild our DAGNodes
					else{
						SelectedObjects.Add(joints[j].gameObject);
					}					
				}
			}
		}
		
		// Rebuild DAGNodes with the included joint transforms if they
		// weren't originally included in the selection
		if(SelectedObjects.Count > SelectedObjectsCount){
			DAGNodes = new List<MayaNode>();
			SelectionToMayaNodes();
		}
	}
	#endregion
	
	#region MayaNode Creation
	// --------------------------------------------------
	// This will go through the SelectedObjects list and create
	// MayaNode versions of them
	// --------------------------------------------------
	void SelectionToMayaNodes(){
		// Go through selected objects
		for(int i=0; i<SelectedObjects.Count; i++){
			// Recursively find all children of selected objects
			// Note - We will automatically set ExportShape to
			// TRUE for children
			ProcessChildren(SelectedObjects[i].transform);
			
			// Recursively find all parents of selected objects
			// Note - We leave ExportShape to FALSE because
			// we only need the parent transforms for placement
			ProcessParents(SelectedObjects[i].transform);
		}
	}
	
	// --------------------------------------------------
	// Recursively process all child transforms of given transform
	// --------------------------------------------------
	void ProcessChildren(Transform t){
		// Add this GameObject to our DAGNodes
		CreateMayaNode(t, true);
		
		// Find any children of this transform and process them
		foreach(Transform child in t){
			ProcessChildren(child);
		}
	}
	
	// --------------------------------------------------
	// Recursively process all parent transforms of given transform
	// --------------------------------------------------
	void ProcessParents(Transform t){
		// Add this GameObject to our DAGNodes
		CreateMayaNode(t, false);
		
		// If transform has a parent, process it
		if(t.parent != null){
			ProcessParents(t.parent);
		}
	}
	
	// --------------------------------------------------
	// Create MayaNode
	// --------------------------------------------------
	// Given a transform, this will create a MayaNode and link
	// the transform to it (so we know which GameObject this 
	// MayaNode references)
	//
	// Note - By default MayaNodes have Shape disabled.
	// We export "transforms only" for parents of selected
	// GameObjects, since we need them to accurately place 
	// the MayaNodes in the scene, but don't want their shape data
	void CreateMayaNode(Transform t, bool ExportShape){
		// First check if the object already exists in our DAGNodes
		int Exists = GetMayaNodeFromTransform(t);
		
		// If we found it (Exists != -1)
		if(Exists != -1){
			if(ExportShape){
				if(DAGNodes[Exists].Shape == false) DAGNodes[Exists].Shape = true;
			}
		}
		else{
			// Create a new transform MayaNode
			mTransform n = new mTransform();
			n.UnityObject = t;
			n.MayaName = CleanName(t.name);
			n.Shape = ExportShape;

			// Add this object to our flat object list
			DAGNodes.Add(n);
		}
	}
	#endregion
	
	#region MayaNode Connections
	// --------------------------------------------------
	// Connects MayaNodes based on Parents and Children
	// --------------------------------------------------
	void CreateMayaNodeConnections(){
		// Go through every object in the DAGNodes
		for(int i=0; i<DAGNodes.Count; i++){
			// First assign the parent
			Transform NodeParent = DAGNodes[i].UnityObject.parent;
			if(NodeParent != null){
				// Find the corresponding MayaNode 
				int NodeIndex = GetMayaNodeFromTransform(NodeParent);
				DAGNodes[i].Parent = DAGNodes[NodeIndex];
			}
			
			// Next assign the children
			for(int c=0; c<DAGNodes[i].UnityObject.childCount; c++){
				Transform NodeChild = DAGNodes[i].UnityObject.GetChild(c);
				// Find the corresponding MayaNode 
				int NodeIndex = GetMayaNodeFromTransform(NodeChild);
				
				// Some children wont be added to the DAGNodes depending on user
				// selection, so if we couldn't find the MayaNode, that means it shouldn't
				// be exported. Just ignore it.
				if(NodeIndex != -1) DAGNodes[i].Children.Add(DAGNodes[NodeIndex]);
			}
		}
	}
	#endregion
	
	#region MayaNode Searching
	// --------------------------------------------------
	// Finds Index of MayaNode in DAGNodes from UnityObject
	// --------------------------------------------------
	// Note - Will return -1 if it couldn't find it
	int GetMayaNodeFromTransform(Transform t){
		int Index = -1;
		for(int i=0; i<DAGNodes.Count; i++){
			// If we found the MayaNode
			if(DAGNodes[i].UnityObject == t){
				Index = i;
				break;
			}
		}
		return Index;
	}
	
	// --------------------------------------------------
	// Finds Index of MayaNode in MaterialNodes from Material
	// --------------------------------------------------
	// Note - Will return -1 if it couldn't find it
	int GetMayaNodeFromMaterial(Material m){
		int Index = -1;
		for(int i=0; i<MaterialNodes.Count; i++){
			// If we found the MayaNode
			if(MaterialNodes[i].UnityMaterial == m){
				Index = i;
				break;
			}
		}
		return Index;
	}
	// --------------------------------------------------
	// Finds Index of MayaNode in MaterialNodes from TerrainData
	// --------------------------------------------------
	// Note - Will return -1 if it couldn't find it
	int GetMayaNodeFromTerrainData(TerrainData tData){
		int Index = -1;
		for(int i=0; i<MaterialNodes.Count; i++){
			// If we found the MayaNode
			if(MaterialNodes[i].UnityTerrainData == tData){
				Index = i;
				break;
			}
		}
		return Index;
	}
	#endregion
	
	#region DAGNodes ReOrder
	// --------------------------------------------------
	// Reorders the DAGNodes so they are listed based on hierarchy
	// --------------------------------------------------
	void ReorderDAGNodes(){
		DAGNodesBuffer = new List<MayaNode>();
		for(int i=0; i<DAGNodes.Count; i++){
			if(DAGNodes[i].Parent == null){
				ReorderDAGNodesRecursive(DAGNodes[i]);
			}
		}
		DAGNodes = DAGNodesBuffer;
	}
	void ReorderDAGNodesRecursive(MayaNode n){
		DAGNodesBuffer.Add(n);
		for(int i=0; i<n.Children.Count; i++){
			ReorderDAGNodesRecursive(n.Children[i]);
		}
	}
	#endregion
	
	#region MayaNode Expansion
	// --------------------------------------------------
	// Creates additional auxiliary nodes based on DAGNodes type
	// --------------------------------------------------
	void ExpandNodes(){
		for(int i=0; i<DAGNodes.Count; i++){
			// Determine object type
			string UnityObjectType = GetUnityObjectType(DAGNodes[i]);

			// --------------------------------------------------
			// Empty Group
			// --------------------------------------------------
			if(UnityObjectType == "Null"){
				// No need to do anything since MayaNodes default to type mTransform
				// ...
			}
			// --------------------------------------------------
			// Mesh
			// --------------------------------------------------
			if(UnityObjectType == "Mesh"){
				// Make MeshShape node
				mMesh meshShape = new mMesh();
				meshShape.UnityObject = DAGNodes[i].UnityObject;
				meshShape.MayaName = DAGNodes[i].MayaName + "Shape";
				
				// Add to DAGNodesBuffer
				DAGNodesBuffer.Add(meshShape);
				
				// Set Parent and Children
				DAGNodes[i].Children.Add(meshShape);
				meshShape.Parent = DAGNodes[i];
				
				// --------------------------------------------------
				// Material(s) Assignment
				// --------------------------------------------------
				Mesh m = DAGNodes[i].UnityObject.gameObject.GetComponent<MeshFilter>().sharedMesh;
				
				// First check that the mesh has a Renderer Component
				Renderer RendererComponentCheck = DAGNodes[i].UnityObject.gameObject.GetComponent<Renderer>();
				if(RendererComponentCheck == null){
					// If no renderer component, assign default Lambert
					Connections.Append("connectAttr \"").Append(GetDAGPath(meshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
					continue;
				}
				
				// Get the materials on the mesh
				Material[] Materials = RendererComponentCheck.sharedMaterials;
				
				// Get the sub-mesh count
				int SubMeshCount = m.subMeshCount;				
				
				// Multiple Materials
				if(SubMeshCount > 1){
					// Go through sub meshes
					for(int s=0; s<SubMeshCount; s++){
						// Note - User Error Check !!
						// If you import the mesh and manually alter the array count in inspector, this could result
						// in the submesh count being greater than the materials count. Which is bad. In this case we
						// create a new index that is based on Submesh count (i) and check if its greater than materials count.
						// if it is, we clamp it to the materials count.
						int ii = s;
						if(ii >= Materials.Length) ii = Materials.Length - 1;
						
						// Get Material Index in MaterialNodes List
						int MaterialIndex = GetMayaNodeFromMaterial(Materials[ii]);
						
						// If we couldn't find it, assign default Lambert (this shouldn't happen)
						if(MaterialIndex == -1){
							Connections.Append("connectAttr \"").Append(GetDAGPath(meshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
						}						
						// Otherwise create material connections
						else{							
							mGroupId GroupID = new mGroupId();
							GroupID.MayaName = GetUniqueGlobalNodeName("groupId");
							AUXNodes.Add(GroupID);
							
							// Connect groupID.id > mesh.instObjGroups
							Connections.Append("connectAttr \"").Append(GroupID.MayaName).Append(".id\" \"").Append(GetDAGPath(meshShape)).Append(".iog.og[").Append(s).AppendLine("].gid\";");

							// Connect groupID.message > SG.groupNodes
							Connections.Append("connectAttr \"").Append(GroupID.MayaName).Append(".msg\" \"").Append(MaterialNodes[MaterialIndex].MayaName).AppendLine(".gn\" -na;");
							
							// Connect SG.memberWireframeColor > mesh.instObjGroups
							Connections.Append("connectAttr \"").Append(MaterialNodes[MaterialIndex].MayaName).Append(".mwc\" \"").Append(GetDAGPath(meshShape)).Append(".iog.og[").Append(s).AppendLine("].gco\";");
							
							// Connect mesh.instObjGroups > SG.dagSetMembers
							Connections.Append("connectAttr \"").Append(GetDAGPath(meshShape)).Append(".iog.og[").Append(s).Append("]\" \"").Append(MaterialNodes[MaterialIndex].MayaName).AppendLine(".dsm\" -na;");
						}
					}
				}
				// Single Material
				else{
					// If the Materials array is 0, assign default Lambert
					if(Materials.Length < 1){
						Connections.Append("connectAttr \"").Append(GetDAGPath(meshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
						continue;
					}
					// If the single material is null, assign default Lambert
					if(Materials[0] == null){
						Connections.Append("connectAttr \"").Append(GetDAGPath(meshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
						continue;
					}
					
					// Get Material Index in MaterialNodes List
					int MaterialIndex = GetMayaNodeFromMaterial(Materials[0]);
								
					// If we found the material, assign it
					if(MaterialIndex != -1){
						Connections.Append("connectAttr \"").Append(GetDAGPath(meshShape)).Append(".iog\" \"").Append(MaterialNodes[MaterialIndex].MayaName).AppendLine(".dsm\" -na;");
					}
					// If we couldn't find it, just assign default Lambert (this shouldn't happen)
					else{
						Connections.Append("connectAttr \"").Append(GetDAGPath(meshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
					}
				}
			}
			
			// --------------------------------------------------
			// SkinnedMesh
			// --------------------------------------------------
			if(UnityObjectType == "SkinnedMesh"){
				// Skinned Mesh Export process goes like this:
				//		Create Nodes that are common between all skinned mesh setups
				//		Create Material assignment Nodes - we have to do this part next so in case there is per-face material assignments
				//			the instObjGroups indices are set correctly
				//		Create common node connects with updated instObjGroup indices
				//		Assign Joints
				//		Create Blendshape nodes
				//		If no Blendshapes, tweak node gets connected to SkinClusterGroupParts, otherwise it gets connected to BlendShapeGroupParts
				
				// Make SkinClusterSet node
				mObjectSet SkinClusterSet = new mObjectSet();
				SkinClusterSet.MayaName = GetUniqueGlobalNodeName("skinClusterSet");
				
				// Make TweakSet node
				mObjectSet TweakSet = new mObjectSet();
				TweakSet.MayaName = GetUniqueGlobalNodeName("tweakSet");
				
				// Make SkinnedMesh node
				mMeshShell SkinMeshShape = new mMeshShell();
				SkinMeshShape.UnityObject = DAGNodes[i].UnityObject;
				SkinMeshShape.MayaName = DAGNodes[i].MayaName + "Shape";
				((mTransform)DAGNodes[i]).LockTransform = true;
				
				// Make SkinCluster node
				mSkinCluster SkinCluster = new mSkinCluster();
				SkinCluster.UnityObject = DAGNodes[i].UnityObject;
				SkinCluster.MayaName = GetUniqueGlobalNodeName("skinCluster");
				
				// Make SkinClusterGroupID node
				mGroupId SkinClusterGroupID = new mGroupId();
				SkinClusterGroupID.MayaName = GetUniqueGlobalNodeName("skinClusterGroupId"); 
				
				// Make SkinClusterGroupParts node
				mGroupParts SkinClusterGroupParts = new mGroupParts();
				SkinClusterGroupParts.UnityObject = DAGNodes[i].UnityObject;
				SkinClusterGroupParts.MayaName = GetUniqueGlobalNodeName("skinClusterGroupParts");
				
				// Make Tweak node
				mTweak TweakNode = new mTweak();
				TweakNode.MayaName = GetUniqueGlobalNodeName("tweak");
				
				// Make GroupParts node
				mGroupParts GroupParts = new mGroupParts();
				GroupParts.UnityObject = DAGNodes[i].UnityObject;
				GroupParts.MayaName = GetUniqueGlobalNodeName("groupParts");
				
				// Make GroupID node
				mGroupId GroupID = new mGroupId();
				GroupID.MayaName = GetUniqueGlobalNodeName("groupId");
				
				// Make MeshOrig node
				mMesh MeshOrigShape = new mMesh();
				MeshOrigShape.UnityObject = DAGNodes[i].UnityObject;
				MeshOrigShape.MayaName = DAGNodes[i].MayaName + "ShapeOrig";
				MeshOrigShape.IntermediateObject = true;
				
				// Add to DAGNodesBuffer
				DAGNodesBuffer.Add(SkinMeshShape);
				DAGNodesBuffer.Add(MeshOrigShape);
				
				// Add to AUXNodesBuffer
				AUXNodes.Add(SkinClusterSet);
				AUXNodes.Add(TweakSet);
				AUXNodes.Add(SkinCluster);
				AUXNodes.Add(SkinClusterGroupParts);
				AUXNodes.Add(SkinClusterGroupID);
				AUXNodes.Add(TweakNode);
				AUXNodes.Add(GroupParts);
				AUXNodes.Add(GroupID);
				
				// Set Parent and Children
				// Note - this must happen before setting connections
				DAGNodes[i].Children.Add(SkinMeshShape);
				SkinMeshShape.Parent = DAGNodes[i];
				DAGNodes[i].Children.Add(MeshOrigShape);
				MeshOrigShape.Parent = DAGNodes[i];
				
				// --------------------------------------------------
				// Material(s) Assignment
				// --------------------------------------------------
				// Note - We must assign material connections first, so the instanceObjectGroups gets the correct values
				Mesh m = DAGNodes[i].UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
				
				// Get the materials on the mesh
				Material[] Materials = DAGNodes[i].UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
				
				// Get the sub-mesh count
				int SubMeshCount = m.subMeshCount;
				
				// Multiple Materials
				if(SubMeshCount > 1){
					// Used to hook up the MeshOrig to the correct GroupParts node
					// We update this with each GroupParts node we make, then use the result
					// later on for the connection
					MayaNode LastGroupParts = GroupParts;
					
					// Go through sub meshes
					// Note - for some reason we need to build the materials list backwards
					// for this to work
					for(int s=SubMeshCount - 1; s>-1; s--){
						// Note - User Error Check !!
						// If you import the mesh and manually alter the array count in inspector, this could result
						// in the submesh count being greater than the materials count. Which is bad. In this case we
						// create a new index that is based on Submesh count (i) and check if its greater than materials count.
						// if it is, we clamp it to the materials count.
						int ii = s;
						if(ii >= Materials.Length) ii = Materials.Length - 1;
						
						// Get Material Index in MaterialNodes List
						int MaterialIndex = GetMayaNodeFromMaterial(Materials[ii]);
						
						// If we couldn't find it, assign default Lambert (this shouldn't happen)
						if(MaterialIndex == -1){
							Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
						}						
						// Otherwise create material connections
						else{
							// Make NewGroupParts node
							mGroupParts NewGroupParts = new mGroupParts();
							NewGroupParts.UnityObject = DAGNodes[i].UnityObject;
							NewGroupParts.MayaName = GetUniqueGlobalNodeName("groupParts");
							NewGroupParts.SubMeshIndex = s;
							AUXNodes.Add(NewGroupParts);
				
							// Make NewGroupID node
							mGroupId NewGroupID = new mGroupId();
							NewGroupID.MayaName = GetUniqueGlobalNodeName("groupId");
							AUXNodes.Add(NewGroupID);
							
							int GroupIDInstObj = SkinMeshShape.GetInstObjCount();
							
							// Null material check
							string MaterialName = MaterialNodes[MaterialIndex].MayaName;
							if(Materials[ii] == null) MaterialName = "initialShadingGroup";

							// Create connections
							Connections.Append("connectAttr \"").Append(NewGroupID.MayaName).Append(".msg\" \"").Append(MaterialName).AppendLine(".gn\" -na;");
							Connections.Append("connectAttr \"").Append(NewGroupID.MayaName).Append(".id\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(GroupIDInstObj).AppendLine("].gid\";");
							Connections.Append("connectAttr \"").Append(NewGroupID.MayaName).Append(".id\" \"").Append(NewGroupParts.MayaName).AppendLine(".gi\";");
							Connections.Append("connectAttr \"").Append(MaterialName).Append(".mwc\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(GroupIDInstObj).AppendLine("].gco\";");
							Connections.Append("connectAttr \"").Append(NewGroupParts.MayaName).Append(".og\" \"").Append(LastGroupParts.MayaName).AppendLine(".ig\";");
							Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(GroupIDInstObj).Append("]\" \"").Append(MaterialName).AppendLine(".dsm\" -na;");
							
							// Update LastGroupParts
							LastGroupParts = NewGroupParts;
						}
					}
					
					// Connect MeshOrig -> Single Material GroupParts node
					Connections.Append("connectAttr \"").Append(GetDAGPath(MeshOrigShape)).Append(".w\" \"").Append(LastGroupParts.MayaName).AppendLine(".ig\";");
					
				}
				// Single Material
				else{
					// If Materials Length is 0
					if(Materials.Length < 1){
						Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
					}
					// If Materials Length is greater than 0
					else{
						// If material is null
						if(Materials[0] == null){
							Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");	
						}
						// Connect skinned mesh to material
						else{
							int MaterialIndex = GetMayaNodeFromMaterial(Materials[0]);
							
							// If we couldn't find it, assign default Lambert (this shouldn't happen)
							if(MaterialIndex == -1){
								Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");						
							}
							else{
								Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog\" \"").Append(MaterialNodes[MaterialIndex].MayaName).AppendLine(".dsm\" -na;");
							}
						}
					}
					
					// Connect MeshOrig -> Single Material GroupParts node
					Connections.Append("connectAttr \"").Append(GetDAGPath(MeshOrigShape)).Append(".w\" \"").Append(GroupParts.MayaName).AppendLine(".ig\";");
				}
				
				// Create Connections
				// Note - Read the latest InstObj from the SkinMeshShape node
				int SkinInstObj1 = SkinMeshShape.GetInstObjCount();
				int SkinInstObj2 = SkinMeshShape.GetInstObjCount();
				Connections.Append("connectAttr \"").Append(SkinClusterGroupID.MayaName).Append(".id\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(SkinInstObj1).AppendLine("].gid\";");
				Connections.Append("connectAttr \"").Append(SkinClusterSet.MayaName).Append(".mwc\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(SkinInstObj1).AppendLine("].gco\";");				
				Connections.Append("connectAttr \"").Append(GroupID.MayaName).Append(".id\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(SkinInstObj2).AppendLine("].gid\";");				
				Connections.Append("connectAttr \"").Append(TweakSet.MayaName).Append(".mwc\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(SkinInstObj2).AppendLine("].gco\";");				
				Connections.Append("connectAttr \"").Append(SkinCluster.MayaName).Append(".og[0]\" \"").Append(GetDAGPath(SkinMeshShape)).AppendLine(".i\";");				
				Connections.Append("connectAttr \"").Append(TweakNode.MayaName).Append(".vl[0].vt[0]\" \"").Append(GetDAGPath(SkinMeshShape)).AppendLine(".twl\";");				
				Connections.Append("connectAttr \"").Append(SkinClusterGroupParts.MayaName).Append(".og\" \"").Append(SkinCluster.MayaName).AppendLine(".ip[0].ig\";");				
				Connections.Append("connectAttr \"").Append(SkinClusterGroupID.MayaName).Append(".id\" \"").Append(SkinCluster.MayaName).AppendLine(".ip[0].gi\";");				
				Connections.Append("connectAttr \"").Append(GroupParts.MayaName).Append(".og\" \"").Append(TweakNode.MayaName).AppendLine(".ip[0].ig\";");				
				Connections.Append("connectAttr \"").Append(GroupID.MayaName).Append(".id\" \"").Append(TweakNode.MayaName).AppendLine(".ip[0].gi\";");				
				Connections.Append("connectAttr \"").Append(SkinClusterGroupID.MayaName).Append(".msg\" \"").Append(SkinClusterSet.MayaName).AppendLine(".gn\" -na;");				
				Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[0]\" \"").Append(SkinClusterSet.MayaName).AppendLine(".dsm\" -na;");
				Connections.Append("connectAttr \"").Append(SkinCluster.MayaName).Append(".msg\" \"").Append(SkinClusterSet.MayaName).AppendLine(".ub[0]\";");
				Connections.Append("connectAttr \"").Append(SkinClusterGroupID.MayaName).Append(".id\" \"").Append(SkinClusterGroupParts.MayaName).AppendLine(".gi\";");
				Connections.Append("connectAttr \"").Append(GroupID.MayaName).Append(".msg\" \"").Append(TweakSet.MayaName).AppendLine(".gn\" -na;");
				Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[1]\" \"").Append(TweakSet.MayaName).AppendLine(".dsm\" -na;");
				Connections.Append("connectAttr \"").Append(TweakNode.MayaName).Append(".msg\" \"").Append(TweakSet.MayaName).AppendLine(".ub[0]\";");
				Connections.Append("connectAttr \"").Append(GroupID.MayaName).Append(".id\" \"").Append(GroupParts.MayaName).AppendLine(".gi\";");
				
				// Create Joint Connections
				Transform[] joints = DAGNodes[i].UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().bones;
				for(int j=0; j<joints.Length; j++){
					// Sometimes the joints can be null, like when you skin a mesh but delete
					// joints afterwards that aren't effecting the mesh. So check if the joint
					// index is null before doing anything
					if(joints[j] == null) continue;
					
					int JointIndex = GetMayaNodeFromTransform(joints[j]);
					// If we couldn't find the joint in our list skip it (this should never happen)
					if(JointIndex == -1) continue;
					Connections.Append("connectAttr \"").Append(GetDAGPath(DAGNodes[JointIndex])).Append(".wm\" \"").Append(SkinCluster.MayaName).Append(".ma[").Append(j).AppendLine("]\";");
					Connections.Append("connectAttr \"").Append(GetDAGPath(DAGNodes[JointIndex])).Append(".liw\" \"").Append(SkinCluster.MayaName).Append(".lw[").Append(j).AppendLine("]\";");
				}
				
				
				// Check for blendshapes
				int NumBlendShapes = m.blendShapeCount;
				
				// If there are blendshapes
				if(NumBlendShapes > 0){
					// Create Blendshape Nodes
					mObjectSet BlendShapeSet = new mObjectSet();
					BlendShapeSet.MayaName = GetUniqueGlobalNodeName("blendShapeSet");
					
					mGroupId BlendShapeGroupID = new mGroupId();
					BlendShapeGroupID.MayaName = GetUniqueGlobalNodeName("blendShapeGroupId");
					
					mGroupParts BlendShapeGroupParts = new mGroupParts();
					BlendShapeGroupParts.MayaName = GetUniqueGlobalNodeName("blendShapeGroupParts");
					BlendShapeGroupParts.ForceAllVerts = true;
					
					mBlendShape BlendShape = new mBlendShape();
					BlendShape.MayaName = GetUniqueGlobalNodeName("blendShape");
					BlendShape.UnityObject = DAGNodes[i].UnityObject;
					
					// Add to AUXNodesBuffer
					AUXNodes.Add(BlendShapeSet);
					AUXNodes.Add(BlendShapeGroupID);
					AUXNodes.Add(BlendShapeGroupParts);
					AUXNodes.Add(BlendShape);
					
					// Get new InstObjGroup connection index
					int BlendInstObj = SkinMeshShape.GetInstObjCount();
					
					// Create connections
					Connections.Append("connectAttr \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(BlendInstObj).Append("]\" \"").Append(BlendShapeSet.MayaName).AppendLine(".dsm\" -na;");
					Connections.Append("connectAttr \"").Append(BlendShapeSet.MayaName).Append(".mwc\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(BlendInstObj).AppendLine("].gco\";");
					Connections.Append("connectAttr \"").Append(BlendShape.MayaName).Append(".msg\" \"").Append(BlendShapeSet.MayaName).AppendLine(".ub[0]\";");
					Connections.Append("connectAttr \"").Append(BlendShapeGroupID.MayaName).Append(".msg\" \"").Append(BlendShapeSet.MayaName).AppendLine(".gn\" -na;");
					Connections.Append("connectAttr \"").Append(BlendShapeGroupID.MayaName).Append(".id\" \"").Append(GetDAGPath(SkinMeshShape)).Append(".iog.og[").Append(BlendInstObj).AppendLine("].gid\";");
					Connections.Append("connectAttr \"").Append(BlendShapeGroupID.MayaName).Append(".id\" \"").Append(BlendShape.MayaName).AppendLine(".ip[0].gi\";");
					Connections.Append("connectAttr \"").Append(BlendShapeGroupID.MayaName).Append(".id\" \"").Append(BlendShapeGroupParts.MayaName).AppendLine(".gi\";");
					Connections.Append("connectAttr \"").Append(BlendShapeGroupParts.MayaName).Append(".og\" \"").Append(BlendShape.MayaName).AppendLine(".ip[0].ig\";");
					Connections.Append("connectAttr \"").Append(BlendShape.MayaName).Append(".og[0]\" \"").Append(SkinClusterGroupParts.MayaName).AppendLine(".ig\";");
					
					// Connect TweakNode to BlendShapeGroupParts
					Connections.Append("connectAttr \"").Append(TweakNode.MayaName).Append(".og[0]\" \"").Append(BlendShapeGroupParts.MayaName).AppendLine(".ig\";");
				}
				// If no blendshapes
				else{
					// Connect TweakNode to SkinClusterGroupParts
					Connections.Append("connectAttr \"").Append(TweakNode.MayaName).Append(".og[0]\" \"").Append(SkinClusterGroupParts.MayaName).AppendLine(".ig\";");
				}
			}
			// --------------------------------------------------
			// Terrain
			// --------------------------------------------------
			if(UnityObjectType == "Terrain"){
				// Get the terrain data
				TerrainData tData = DAGNodes[i].UnityObject.gameObject.GetComponent<Terrain>().terrainData;
				
				// Create "terrain" shape node
				mTerrain Terrain = new mTerrain();
				Terrain.UnityObject = DAGNodes[i].UnityObject;
				Terrain.MayaName = CleanName(tData.name + "Shape");
				
				// Override Terrain Transform Rotate
				((mTransform)DAGNodes[i]).UseRotateOverride = true;
				((mTransform)DAGNodes[i]).RotateOverride = Vector3.zero;
				
				// Add new nodes to Buffer
				DAGNodesBuffer.Add(Terrain);
				
				// Connect New Nodes
				DAGNodes[i].Children.Add(Terrain);
				Terrain.Parent = DAGNodes[i];
			
				// --------------------------------------------------
				// Terrain Material assignment
				// --------------------------------------------------
				// Get Material Index in MaterialNodes List
				int MaterialIndex = GetMayaNodeFromTerrainData(tData);
								
				// If we found the material, assign it
				if(MaterialIndex != -1){
					Connections.Append("connectAttr \"").Append(GetDAGPath(Terrain)).Append(".iog\" \"").Append(MaterialNodes[MaterialIndex].MayaName).AppendLine(".dsm\" -na;");
				}
				// If we couldn't find it, just assign default Lambert (this shouldn't happen)
				else{
					Connections.Append("connectAttr \"").Append(GetDAGPath(Terrain)).AppendLine(".iog\" \":initialShadingGroup.dsm\" -na;");
				}
			}
			// --------------------------------------------------
			// Camera
			// --------------------------------------------------
			if(UnityObjectType == "Camera"){
				mCamera Camera = new mCamera();
				Camera.UnityObject = DAGNodes[i].UnityObject;
				Camera.MayaName = DAGNodes[i].MayaName + "Shape";
			
				// Add new nodes to Buffer
				DAGNodesBuffer.Add(Camera);
			
				// Connect New Nodes
				DAGNodes[i].Children.Add(Camera);
				Camera.Parent = DAGNodes[i];
				
				// Flip Camera RotateY 180 Degrees
				Vector3 RotateOverride = DAGNodes[i].UnityObject.localRotation.eulerAngles;
				RotateOverride.y += 180;
				((mTransform)DAGNodes[i]).UseRotateOverride = true;
				((mTransform)DAGNodes[i]).RotateOverride = RotateOverride;
			}
			
			// --------------------------------------------------
			// SpotLight
			// --------------------------------------------------
			if(UnityObjectType == "SpotLight"){
				mSpotLight SpotLight = new mSpotLight();
				SpotLight.UnityObject = DAGNodes[i].UnityObject;
				SpotLight.MayaName = DAGNodes[i].MayaName + "Shape";
				
				// Add new nodes to Buffer
				DAGNodesBuffer.Add(SpotLight);
			
				// Connect New Nodes
				DAGNodes[i].Children.Add(SpotLight);
				SpotLight.Parent = DAGNodes[i];
				
				// Create Connections
				Connections.Append("connectAttr \"").Append(SpotLight.MayaName).AppendLine(".ltd\" \":lightList1.l\" -na;");
				Connections.Append("connectAttr \"").Append(SpotLight.MayaName).AppendLine(".iog\" \":defaultLightSet.dsm\" -na;");
				
				// Flip Light RotateX 180 Degrees
				Vector3 RotateOverride = DAGNodes[i].UnityObject.localRotation.eulerAngles;
				RotateOverride.x += 180;
				((mTransform)DAGNodes[i]).UseRotateOverride = true;
				((mTransform)DAGNodes[i]).RotateOverride = RotateOverride;
			}
			
			// --------------------------------------------------
			// DirectionalLight
			// --------------------------------------------------
			if(UnityObjectType == "DirectionalLight"){
				mDirectionalLight DirectionalLight = new mDirectionalLight();
				DirectionalLight.UnityObject = DAGNodes[i].UnityObject;
				DirectionalLight.MayaName = DAGNodes[i].MayaName + "Shape";
				
				// Add new nodes to Buffer
				DAGNodesBuffer.Add(DirectionalLight);
			
				// Connect New Nodes
				DAGNodes[i].Children.Add(DirectionalLight);
				DirectionalLight.Parent = DAGNodes[i];
				
				// Create Connections
				Connections.Append("connectAttr \"").Append(DirectionalLight.MayaName).AppendLine(".ltd\" \":lightList1.l\" -na;");
				Connections.Append("connectAttr \"").Append(DirectionalLight.MayaName).AppendLine(".iog\" \":defaultLightSet.dsm\" -na;");
				
				// Flip Light RotateX 180 Degrees
				Vector3 RotateOverride = DAGNodes[i].UnityObject.localRotation.eulerAngles;
				RotateOverride.x += 180;
				((mTransform)DAGNodes[i]).UseRotateOverride = true;
				((mTransform)DAGNodes[i]).RotateOverride = RotateOverride;
			}
			
			// --------------------------------------------------
			// PointLight
			// --------------------------------------------------
			if(UnityObjectType == "PointLight"){
				mPointLight PointLight = new mPointLight();
				PointLight.UnityObject = DAGNodes[i].UnityObject;
				PointLight.MayaName = DAGNodes[i].MayaName + "Shape";
				
				// Add new nodes to Buffer
				DAGNodesBuffer.Add(PointLight);
			
				// Connect New Nodes
				DAGNodes[i].Children.Add(PointLight);
				PointLight.Parent = DAGNodes[i];
				
				// Create Connections
				Connections.Append("connectAttr \"").Append(PointLight.MayaName).AppendLine(".ltd\" \":lightList1.l\" -na;");
				Connections.Append("connectAttr \"").Append(PointLight.MayaName).AppendLine(".iog\" \":defaultLightSet.dsm\" -na;");
				
				// Flip Light RotateX 180 Degrees
				Vector3 RotateOverride = DAGNodes[i].UnityObject.localRotation.eulerAngles;
				RotateOverride.x += 180;
				((mTransform)DAGNodes[i]).UseRotateOverride = true;
				((mTransform)DAGNodes[i]).RotateOverride = RotateOverride;
			}
			
			// --------------------------------------------------
			// AreaLight
			// --------------------------------------------------
			if(UnityObjectType == "AreaLight"){
				mAreaLight AreaLight = new mAreaLight();
				AreaLight.UnityObject = DAGNodes[i].UnityObject;
				AreaLight.MayaName = DAGNodes[i].MayaName + "Shape";
				
				// Add new nodes to Buffer
				DAGNodesBuffer.Add(AreaLight);
			
				// Connect New Nodes
				DAGNodes[i].Children.Add(AreaLight);
				AreaLight.Parent = DAGNodes[i];
				
				// Create Connections
				Connections.Append("connectAttr \"").Append(AreaLight.MayaName).AppendLine(".ltd\" \":lightList1.l\" -na;");
				Connections.Append("connectAttr \"").Append(AreaLight.MayaName).AppendLine(".iog\" \":defaultLightSet.dsm\" -na;");
				
				// Flip Light RotateX 180 Degrees
				Vector3 RotateOverride = DAGNodes[i].UnityObject.localRotation.eulerAngles;
				RotateOverride.x += 180;
				((mTransform)DAGNodes[i]).UseRotateOverride = true;
				((mTransform)DAGNodes[i]).RotateOverride = RotateOverride;
			}
		}
		
		// --------------------------------------------------
		// Copy contents of DAGNodesBuffer to DAGNodes
		// --------------------------------------------------
		DAGNodes.AddRange(DAGNodesBuffer);
		DAGNodesBuffer = new List<MayaNode>();
	}
	#endregion

	// --------------------------------------------------
	// Creates Material and Texture MayaNodes
	// --------------------------------------------------
	// After the MayaNodes have been expanded, we need to build
	// a separate list of Material and Texture MayaNodes
	void CreateMaterials(){
		// Go through every MayaNode
		for(int i=0; i<DAGNodes.Count; i++){
			// Material array of all the materials on the object
			Material[] Materials = null;
			
			// Get the type of UnityObject it is so we know how to query the materials
			string UnityObjectType = GetUnityObjectType(DAGNodes[i]);

			// --------------------------------------------------
			// Terrain Materials
			// --------------------------------------------------
			// If the MayaNode is a Terrain, handle material querying as special case
			// here, then skip the rest of this method
			if(UnityObjectType == "Terrain"){
				// Get the terrain data
				TerrainData tData = DAGNodes[i].UnityObject.gameObject.GetComponent<Terrain>().terrainData;
				
				// Get the size of the terrain (so we can figure out the repeating textures)
				Vector3 TerrainSize = tData.size;
				
				// Check if there is a material for this terrain already
				int TerrainMatIndex = GetMayaNodeFromTerrainData(tData);

				// If we didn't find it
				if(TerrainMatIndex == -1){
					// --------------------------------------------------
					// Create MaterialInfo MayaNode
					// --------------------------------------------------
					mMaterialInfo MatInfo = new mMaterialInfo();
					MatInfo.MayaName = GetUniqueGlobalNodeName("materialInfo");
					MaterialNodes.Add(MatInfo);
					
					// --------------------------------------------------
					// Create ShadingGroup MayaNode
					// --------------------------------------------------
					mShadingEngine TerrainSG = new mShadingEngine();
					TerrainSG.UnityTerrainData = tData;
					TerrainSG.MayaName = GetUniqueGlobalNodeName(tData.name + "SG");
					MaterialNodes.Add(TerrainSG);
					
					// Connect to MaterialInfo
					Connections.Append("connectAttr \"").Append(TerrainSG.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".sg\";");
					// Connect to LightLinker (Lights)
					Connections.Append("relationship \"link\" \":lightLinker1\" \"").Append(TerrainSG.MayaName).AppendLine(".message\" \":defaultLightSet.message\";");
					// Connect to LightLinker (Shadows)
					Connections.Append("relationship \"shadowLink\" \":lightLinker1\" \"").Append(TerrainSG.MayaName).AppendLine(".message\" \":defaultLightSet.message\";");
					Connections.Append("connectAttr \"").Append(TerrainSG.MayaName).AppendLine(".pa\" \":renderPartition.st\" -na;");
					
					// --------------------------------------------------
					// Create Material MayaNode
					// --------------------------------------------------
					mBlinn TerrainMat = new mBlinn();
					TerrainMat.MaterialType = MayaMaterialType.TerrainMaterial;
					TerrainMat.UnityTerrainData = tData;
					TerrainMat.MayaName = GetUniqueGlobalNodeName(tData.name);
					MaterialNodes.Add(TerrainMat);

					// Connect to MaterialInfo
					Connections.Append("connectAttr \"").Append(TerrainMat.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".m\";");
					// Connect to ShadingGroup
					Connections.Append("connectAttr \"").Append(TerrainMat.MayaName).Append(".oc\" \"").Append(TerrainSG.MayaName).AppendLine(".ss\";");
					// Makes the Blinn show up in Hypershade
					Connections.Append("connectAttr \"").Append(TerrainMat.MayaName).AppendLine(".msg\" \":defaultShaderList1.s\" -na;");
					
					// --------------------------------------------------
					// Get the Terrain Texture(s)
					// --------------------------------------------------
					SplatPrototype[] terrainTextures = tData.splatPrototypes;
					
					// If there are terrain textures
					if(terrainTextures.Length > 0){
						// Layered Texture Nodes
						mLayeredTexture LayeredDiffuse = new mLayeredTexture();
						mLayeredTexture LayeredNormal = new mLayeredTexture();
						
						// Create a layered texture node
						LayeredDiffuse = new mLayeredTexture();
						LayeredDiffuse.UnityTerrainData = tData;
						LayeredDiffuse.MayaName = GetUniqueGlobalNodeName("layered_diffuse");
						LayeredDiffuse.NumberOfInputs = terrainTextures.Length;
						MaterialNodes.Add(LayeredDiffuse);
						
						// Create Connections
						Connections.Append("connectAttr \"").Append(LayeredDiffuse.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
						Connections.Append("connectAttr \"").Append(LayeredDiffuse.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".t\" -na;");
						Connections.Append("connectAttr \"").Append(LayeredDiffuse.MayaName).Append(".oc\" \"").Append(TerrainMat.MayaName).AppendLine(".c\";");
	
						// Before we create a layered texture for normals, check to see if any normal maps
						// exist. If not, then we don't need to make it
						bool NormalTexturesExist = false;
						for(int s=0; s<terrainTextures.Length; s++){
							if(terrainTextures[s].normalMap != null){
								NormalTexturesExist = true;
								break;
							}
						}
						if(NormalTexturesExist){
							// Create a layered normal node
							LayeredNormal = new mLayeredTexture();
							LayeredNormal.UnityTerrainData = tData;
							LayeredNormal.MayaName = GetUniqueGlobalNodeName("layered_normal");
							LayeredNormal.NumberOfInputs = terrainTextures.Length;
							MaterialNodes.Add(LayeredNormal);
							
							// Create a bump2d node
							mBump2d TerrainBump = new mBump2d();
							TerrainBump.UnityTerrainData = tData;
							TerrainBump.MayaName = GetUniqueGlobalNodeName("layered_bump");
							MaterialNodes.Add(TerrainBump);
							
							// Connection Layered Normal -> Default Texture List
							Connections.Append("connectAttr \"").Append(LayeredNormal.MayaName).Append(".msg\" \":defaultTextureList1.tx\" -na;");
							// Connect Layered Normal -> Bump2D
							Connections.Append("connectAttr \"").Append(LayeredNormal.MayaName).Append(".oa\" \"").Append(TerrainBump.MayaName).AppendLine(".bv\";");
							// Connect Bump2D -> Terrain Material
							Connections.Append("connectAttr \"").Append(TerrainBump.MayaName).Append(".o\" \"").Append(TerrainMat.MayaName).AppendLine(".n\";");
							// Connect Bump2D -> Default Render Utility List
							Connections.Append("connectAttr \"").Append(TerrainBump.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
						}
						
						// Create all terrain texture nodes
						// Note - We go backwards through the list so they are in the
						// correct order in the layered texture node
						for(int t=terrainTextures.Length - 1; t>-1; t--){
							// Get tiling and offset
							Vector2 TexTiling = terrainTextures[t].tileSize;
							Vector2 TexOffset = terrainTextures[t].tileOffset;
							
							// Create a terrain texture node
							mFile TerrainTex = new mFile();
							TerrainTex.UnityTexture = terrainTextures[t].texture;
							TerrainTex.MayaName = GetUniqueGlobalNodeName("terrain_texture");
							MaterialNodes.Add(TerrainTex);
							
							// Create texture connections
							Connections.Append("connectAttr \"").Append(TerrainTex.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
							Connections.Append("connectAttr \"").Append(TerrainTex.MayaName).Append(".oc\" \"").Append(LayeredDiffuse.MayaName).Append(".cs[").Append(t.ToString()).AppendLine("].c\";");
							
							// Create Place2DTexture Node
							mPlace2dTexture Place2DTex = new mPlace2dTexture();
							Place2DTex.TexTiling = new Vector2(TerrainSize.x / TexTiling.x, TerrainSize.z / TexTiling.y);
							Place2DTex.TexOffset = TexOffset;
							//Place2DTex.TexTiling = terrainTextures[t].tileSize;
							//Place2DTex.TexOffset = terrainTextures[t].tileOffset;
							Place2DTex.MayaName = GetUniqueGlobalNodeName("place2dTexture");
							MaterialNodes.Add(Place2DTex);
							
							// Makes the Place2DTexture node show up in the HyperShade
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
							
							// Connections from Place2DTexture to File Texture
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".c\" \"").Append(TerrainTex.MayaName).AppendLine(".c\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".tf\" \"").Append(TerrainTex.MayaName).AppendLine(".tf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".rf\" \"").Append(TerrainTex.MayaName).AppendLine(".rf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mu\" \"").Append(TerrainTex.MayaName).AppendLine(".mu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mv\" \"").Append(TerrainTex.MayaName).AppendLine(".mv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".s\" \"").Append(TerrainTex.MayaName).AppendLine(".s\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wu\" \"").Append(TerrainTex.MayaName).AppendLine(".wu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wv\" \"").Append(TerrainTex.MayaName).AppendLine(".wv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".re\" \"").Append(TerrainTex.MayaName).AppendLine(".re\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".of\" \"").Append(TerrainTex.MayaName).AppendLine(".of\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".r\" \"").Append(TerrainTex.MayaName).AppendLine(".ro\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".n\" \"").Append(TerrainTex.MayaName).AppendLine(".n\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt1\" \"").Append(TerrainTex.MayaName).AppendLine(".vt1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt2\" \"").Append(TerrainTex.MayaName).AppendLine(".vt2\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt3\" \"").Append(TerrainTex.MayaName).AppendLine(".vt3\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vc1\" \"").Append(TerrainTex.MayaName).AppendLine(".vc1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".o\" \"").Append(TerrainTex.MayaName).AppendLine(".uv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".ofs\" \"").Append(TerrainTex.MayaName).AppendLine(".fs\";");
							
							// If normal textures exist at all
							if(NormalTexturesExist){
								// Create a terrain normal texture (if not null)
								if(terrainTextures[t].normalMap != null){
									// Create a terrain texture node
									mFile TerrainTexNormal = new mFile();
									TerrainTexNormal.UnityTexture = terrainTextures[t].normalMap;
									TerrainTexNormal.MayaName = GetUniqueGlobalNodeName("terrain_normal");
									MaterialNodes.Add(TerrainTexNormal);
									
									// Create texture connections
									Connections.Append("connectAttr \"").Append(TerrainTexNormal.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
									Connections.Append("connectAttr \"").Append(TerrainTexNormal.MayaName).Append(".oc\" \"").Append(LayeredNormal.MayaName).Append(".cs[").Append(t.ToString()).AppendLine("].c\";");
									
									// Create Place2DTexture Node
									mPlace2dTexture Place2DTexNormal = new mPlace2dTexture();
									Place2DTexNormal.TexTiling = new Vector2(TerrainSize.x / TexTiling.x, TerrainSize.z / TexTiling.y);
									Place2DTexNormal.TexOffset = TexOffset;
									Place2DTexNormal.MayaName = GetUniqueGlobalNodeName("place2dTexture");
									MaterialNodes.Add(Place2DTexNormal);
									
									// Makes the Place2DTexture node show up in the HyperShade
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
									
									// Connections from Place2DTexture to File Texture
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".c\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".c\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".tf\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".tf\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".rf\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".rf\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".mu\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".mu\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".mv\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".mv\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".s\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".s\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".wu\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".wu\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".wv\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".wv\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".re\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".re\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".of\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".of\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".r\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".ro\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".n\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".n\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".vt1\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".vt1\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".vt2\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".vt2\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".vt3\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".vt3\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".vc1\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".vc1\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".o\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".uv\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".ofs\" \"").Append(TerrainTexNormal.MayaName).AppendLine(".fs\";");
								}
								// Otherwise create a blank ramp node so the normals blend correctly
								else{
									// Create a ramp node
									mRamp BlankTexNormal = new mRamp();
									BlankTexNormal.UnityTexture = terrainTextures[t].normalMap;
									BlankTexNormal.MayaName = GetUniqueGlobalNodeName("blank_normal");
									BlankTexNormal.Colors.Add(new Color(0.5019607843137255f, 0.5019607843137255f, 1.0f));
									MaterialNodes.Add(BlankTexNormal);
									
									// Create texture connections
									Connections.Append("connectAttr \"").Append(BlankTexNormal.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
									Connections.Append("connectAttr \"").Append(BlankTexNormal.MayaName).Append(".oc\" \"").Append(LayeredNormal.MayaName).Append(".cs[").Append(t.ToString()).AppendLine("].c\";");
									
									// Create Place2DTexture Node
									mPlace2dTexture Place2DTexNormal = new mPlace2dTexture();
									Place2DTexNormal.TexTiling = new Vector2(1, 1);
									Place2DTexNormal.TexOffset = TexOffset;
									Place2DTexNormal.MayaName = GetUniqueGlobalNodeName("place2dTexture");
									MaterialNodes.Add(Place2DTexNormal);
									
									// Makes the Place2DTexture node show up in the HyperShade
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
									
									// Connections from Place2DTexture to Ramp Texture
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".o\" \"").Append(BlankTexNormal.MayaName).AppendLine(".uv\";");
									Connections.Append("connectAttr \"").Append(Place2DTexNormal.MayaName).Append(".ofs\" \"").Append(BlankTexNormal.MayaName).AppendLine(".fs\";");
								}
							}
							
							// Create splatmap texture nodes for this layer
							mTerrainFileAlpha SplatMap = new mTerrainFileAlpha();
							SplatMap.UnityTexture = terrainTextures[t].texture;
							SplatMap.MayaName = GetUniqueGlobalNodeName(tData.name + "_splatmap");
							SplatMap.ImgName = (tData.name + "_splatmap_" + t);
							MaterialNodes.Add(SplatMap);
							
							Connections.Append("connectAttr \"").Append(SplatMap.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
							
							// Connect to alpha of diffuse
							Connections.Append("connectAttr \"").Append(SplatMap.MayaName).Append(".ocr\" \"").Append(LayeredDiffuse.MayaName).Append(".cs[").Append(t.ToString()).AppendLine("].a\";");
							
							// Connect to alpha of normal
							if(NormalTexturesExist){
								Connections.Append("connectAttr \"").Append(SplatMap.MayaName).Append(".ocr\" \"").Append(LayeredNormal.MayaName).Append(".cs[").Append(t.ToString()).AppendLine("].a\";");
							}
						}
					}					
				}

				// --------------------------------------------------
				// Continue to next object in NodesList
				// (Skip everything below this)
				// --------------------------------------------------
				continue;
			}			
			
			// --------------------------------------------------
			// Standard Materials
			// --------------------------------------------------
			// Get the materials on the mesh depending on type
			if(UnityObjectType == "Mesh"){
				Renderer RendererCheck = DAGNodes[i].UnityObject.gameObject.GetComponent<Renderer>();
				if(RendererCheck != null) Materials = RendererCheck.sharedMaterials;
			}
			if(UnityObjectType == "SkinnedMesh"){
				// No need to check for SkinnedMeshRenderer, since without it, it wouldn't be identified as a SkinnedMesh
				Materials = DAGNodes[i].UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
			}

			// --------------------------------------------------
			// Materials Checks
			// --------------------------------------------------
			// If there are no materials, skip this DAGNode
			if(Materials == null) continue;
		
			// Go through every material and see if it exists in our MaterialNodes
			// If not, make it and add it
			for(int m=0; m<Materials.Length; m++){
				// Skip if material is null
				if(Materials[m] == null) continue;
				
				int MaterialIndex = GetMayaNodeFromMaterial(Materials[m]);
				
				// If we didn't find it
				if(MaterialIndex == -1){
					// --------------------------------------------------
					// Create MaterialInfo MayaNode
					// --------------------------------------------------
					mMaterialInfo MatInfo = new mMaterialInfo();
					MatInfo.MayaName = GetUniqueGlobalNodeName("materialInfo");
					MaterialNodes.Add(MatInfo);
					
					// --------------------------------------------------
					// Create ShadingGroup MayaNode
					// --------------------------------------------------
					mShadingEngine NewSG = new mShadingEngine();
					NewSG.UnityMaterial = Materials[m];
					NewSG.MayaName = GetUniqueGlobalNodeName(Materials[m].name + "SG");
					MaterialNodes.Add(NewSG);

					// Connect to MaterialInfo
					Connections.Append("connectAttr \"").Append(NewSG.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".sg\";");
					// Connect to LightLinker (Lights)
					Connections.Append("relationship \"link\" \":lightLinker1\" \"").Append(NewSG.MayaName).AppendLine(".message\" \":defaultLightSet.message\";");
					// Connect to LightLinker (Shadows)
					Connections.Append("relationship \"shadowLink\" \":lightLinker1\" \"").Append(NewSG.MayaName).AppendLine(".message\" \":defaultLightSet.message\";");
					Connections.Append("connectAttr \"").Append(NewSG.MayaName).AppendLine(".pa\" \":renderPartition.st\" -na;");
					
					// --------------------------------------------------
					// Create Material MayaNode
					// --------------------------------------------------
					mBlinn NewMat = new mBlinn();
					NewMat.UnityMaterial = Materials[m];
					NewMat.MaterialType = MayaMaterialType.RegularMaterial;
					NewMat.MayaName = GetUniqueGlobalNodeName(Materials[m].name);
					MaterialNodes.Add(NewMat);

					// Connect to MaterialInfo
					Connections.Append("connectAttr \"").Append(NewMat.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".m\";");
					// Connect to ShadingGroup
					Connections.Append("connectAttr \"").Append(NewMat.MayaName).Append(".oc\" \"").Append(NewSG.MayaName).AppendLine(".ss\";");
					// Makes the Blinn show up in Hypershade
					Connections.Append("connectAttr \"").Append(NewMat.MayaName).AppendLine(".msg\" \":defaultShaderList1.s\" -na;");
					
					// --------------------------------------------------
					// Create Texture MayaNodes (if available)
					// --------------------------------------------------
					// NOTE! Only search for textures if Material is not NULL
					if(Materials[m] == null) continue;
					
					// Get tiling and offset
					Vector2 TexTiling = new Vector2(1,1);
					Vector2 TexOffset = new Vector2(0,0);
					if(Materials[m].HasProperty("_MainTex")){
						TexTiling = Materials[m].GetTextureScale("_MainTex");
						TexOffset = Materials[m].GetTextureOffset("_MainTex");
					}
					
					// _MainTex
					if(Materials[m].HasProperty("_MainTex")){
						Texture MainTex = Materials[m].GetTexture("_MainTex");
						if(MainTex != null){
							mFile MainTexNode = new mFile();
							MainTexNode.UnityTexture = MainTex;
							MainTexNode.MayaName = GetUniqueGlobalNodeName("file");
							MaterialNodes.Add(MainTexNode);
							
							// Connect to MaterialInfo
							Connections.Append("connectAttr \"").Append(MainTexNode.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".t\" -na;");
							// Makes the file node show up in Hypershade
							Connections.Append("connectAttr \"").Append(MainTexNode.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
							// Connect to InColor of Material
							Connections.Append("connectAttr \"").Append(MainTexNode.MayaName).Append(".oc\" \"").Append(NewMat.MayaName).AppendLine(".c\";");
							
							mPlace2dTexture Place2DTex = new mPlace2dTexture();
							Place2DTex.TexTiling = TexTiling;
							Place2DTex.TexOffset = TexOffset;
							Place2DTex.MayaName = GetUniqueGlobalNodeName("place2dTexture");
							MaterialNodes.Add(Place2DTex);
							
							// Makes the Place2DTexture node show up in the HyperShade
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
							
							// Connections from Place2DTexture to File Texture
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".c\" \"").Append(MainTexNode.MayaName).AppendLine(".c\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".tf\" \"").Append(MainTexNode.MayaName).AppendLine(".tf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".rf\" \"").Append(MainTexNode.MayaName).AppendLine(".rf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mu\" \"").Append(MainTexNode.MayaName).AppendLine(".mu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mv\" \"").Append(MainTexNode.MayaName).AppendLine(".mv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".s\" \"").Append(MainTexNode.MayaName).AppendLine(".s\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wu\" \"").Append(MainTexNode.MayaName).AppendLine(".wu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wv\" \"").Append(MainTexNode.MayaName).AppendLine(".wv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".re\" \"").Append(MainTexNode.MayaName).AppendLine(".re\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".of\" \"").Append(MainTexNode.MayaName).AppendLine(".of\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".r\" \"").Append(MainTexNode.MayaName).AppendLine(".ro\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".n\" \"").Append(MainTexNode.MayaName).AppendLine(".n\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt1\" \"").Append(MainTexNode.MayaName).AppendLine(".vt1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt2\" \"").Append(MainTexNode.MayaName).AppendLine(".vt2\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt3\" \"").Append(MainTexNode.MayaName).AppendLine(".vt3\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vc1\" \"").Append(MainTexNode.MayaName).AppendLine(".vc1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".o\" \"").Append(MainTexNode.MayaName).AppendLine(".uv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".ofs\" \"").Append(MainTexNode.MayaName).AppendLine(".fs\";");
						}
					}
					// _SpecGloassMap
					if(Materials[m].HasProperty("_SpecGlossMap")){
						Texture SpecularTex = Materials[m].GetTexture("_SpecGlossMap");
						if(SpecularTex != null){
							mFile SpecularTexNode = new mFile();
							SpecularTexNode.UnityTexture = SpecularTex;
							SpecularTexNode.MayaName = GetUniqueGlobalNodeName("file");
							MaterialNodes.Add(SpecularTexNode);
							
							// Connect to MaterialInfo
							Connections.Append("connectAttr \"").Append(SpecularTexNode.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".t\" -na;");
							// Makes the file node show up in Hypershade
							Connections.Append("connectAttr \"").Append(SpecularTexNode.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
							// Connect to SpecularColor of Material
							Connections.Append("connectAttr \"").Append(SpecularTexNode.MayaName).Append(".oc\" \"").Append(NewMat.MayaName).AppendLine(".sc\";");
							
							mPlace2dTexture Place2DTex = new mPlace2dTexture();
							Place2DTex.TexTiling = TexTiling;
							Place2DTex.TexOffset = TexOffset;
							Place2DTex.MayaName = GetUniqueGlobalNodeName("place2dTexture");
							MaterialNodes.Add(Place2DTex);
							
							// Makes the Place2DTexture node show up in the HyperShade
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
							
							// Connections from Place2DTexture to File Texture
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".c\" \"").Append(SpecularTexNode.MayaName).AppendLine(".c\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".tf\" \"").Append(SpecularTexNode.MayaName).AppendLine(".tf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".rf\" \"").Append(SpecularTexNode.MayaName).AppendLine(".rf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mu\" \"").Append(SpecularTexNode.MayaName).AppendLine(".mu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mv\" \"").Append(SpecularTexNode.MayaName).AppendLine(".mv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".s\" \"").Append(SpecularTexNode.MayaName).AppendLine(".s\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wu\" \"").Append(SpecularTexNode.MayaName).AppendLine(".wu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wv\" \"").Append(SpecularTexNode.MayaName).AppendLine(".wv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".re\" \"").Append(SpecularTexNode.MayaName).AppendLine(".re\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".of\" \"").Append(SpecularTexNode.MayaName).AppendLine(".of\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".r\" \"").Append(SpecularTexNode.MayaName).AppendLine(".ro\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".n\" \"").Append(SpecularTexNode.MayaName).AppendLine(".n\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt1\" \"").Append(SpecularTexNode.MayaName).AppendLine(".vt1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt2\" \"").Append(SpecularTexNode.MayaName).AppendLine(".vt2\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt3\" \"").Append(SpecularTexNode.MayaName).AppendLine(".vt3\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vc1\" \"").Append(SpecularTexNode.MayaName).AppendLine(".vc1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".o\" \"").Append(SpecularTexNode.MayaName).AppendLine(".uv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".ofs\" \"").Append(SpecularTexNode.MayaName).AppendLine(".fs\";");
						}
					}
					// _BumpMap
					if(Materials[m].HasProperty("_BumpMap")){
						Texture BumpTex = Materials[m].GetTexture("_BumpMap");
						if(BumpTex != null){
							// Create bump file node
							mFile BumpTexNode = new mFile();
							BumpTexNode.UnityTexture = BumpTex;
							BumpTexNode.MayaName = GetUniqueGlobalNodeName("file");
							MaterialNodes.Add(BumpTexNode);
							
							// Create a bump2d node
							mBump2d BumpNode = new mBump2d();
							BumpNode.UnityTexture = BumpTex;
							BumpNode.MayaName = GetUniqueGlobalNodeName("bump2d");
							if(Materials[m].HasProperty("_BumpScale")){
								BumpNode.BumpAmount = Materials[m].GetFloat("_BumpScale");
							}
							MaterialNodes.Add(BumpNode);
							
							// Connect to MaterialInfo
							Connections.Append("connectAttr \"").Append(BumpTexNode.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".t\" -na;");
							// Makes the file node show up in Hypershade
							Connections.Append("connectAttr \"").Append(BumpTexNode.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
							// Connect file -> bump2d
							Connections.Append("connectAttr \"").Append(BumpTexNode.MayaName).Append(".oa\" \"").Append(BumpNode.MayaName).AppendLine(".bv\";");
							// Connect Bump -> Material
							Connections.Append("connectAttr \"").Append(BumpNode.MayaName).Append(".o\" \"").Append(NewMat.MayaName).AppendLine(".n\";");
							
							mPlace2dTexture Place2DTex = new mPlace2dTexture();
							Place2DTex.TexTiling = TexTiling;
							Place2DTex.TexOffset = TexOffset;
							Place2DTex.MayaName = GetUniqueGlobalNodeName("place2dTexture");
							MaterialNodes.Add(Place2DTex);
							
							// Makes the Place2DTexture node show up in the HyperShade
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
							
							// Connections from Place2DTexture to File Texture
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".c\" \"").Append(BumpTexNode.MayaName).AppendLine(".c\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".tf\" \"").Append(BumpTexNode.MayaName).AppendLine(".tf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".rf\" \"").Append(BumpTexNode.MayaName).AppendLine(".rf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mu\" \"").Append(BumpTexNode.MayaName).AppendLine(".mu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mv\" \"").Append(BumpTexNode.MayaName).AppendLine(".mv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".s\" \"").Append(BumpTexNode.MayaName).AppendLine(".s\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wu\" \"").Append(BumpTexNode.MayaName).AppendLine(".wu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wv\" \"").Append(BumpTexNode.MayaName).AppendLine(".wv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".re\" \"").Append(BumpTexNode.MayaName).AppendLine(".re\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".of\" \"").Append(BumpTexNode.MayaName).AppendLine(".of\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".r\" \"").Append(BumpTexNode.MayaName).AppendLine(".ro\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".n\" \"").Append(BumpTexNode.MayaName).AppendLine(".n\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt1\" \"").Append(BumpTexNode.MayaName).AppendLine(".vt1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt2\" \"").Append(BumpTexNode.MayaName).AppendLine(".vt2\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt3\" \"").Append(BumpTexNode.MayaName).AppendLine(".vt3\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vc1\" \"").Append(BumpTexNode.MayaName).AppendLine(".vc1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".o\" \"").Append(BumpTexNode.MayaName).AppendLine(".uv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".ofs\" \"").Append(BumpTexNode.MayaName).AppendLine(".fs\";");
						}
					}
					// _EmissionMap
					if(Materials[m].HasProperty("_EmissionMap")){
						Texture EmissionTex = Materials[m].GetTexture("_EmissionMap");
						if(EmissionTex != null){
							mFile EmissionTexNode = new mFile();
							EmissionTexNode.UnityTexture = EmissionTex;
							EmissionTexNode.MayaName = GetUniqueGlobalNodeName("file");
							MaterialNodes.Add(EmissionTexNode);
							
							// Connect to MaterialInfo
							Connections.Append("connectAttr \"").Append(EmissionTexNode.MayaName).Append(".msg\" \"").Append(MatInfo.MayaName).AppendLine(".t\" -na;");
							// Makes the file node show up in Hypershade
							Connections.Append("connectAttr \"").Append(EmissionTexNode.MayaName).AppendLine(".msg\" \":defaultTextureList1.tx\" -na;");
							// Connect to InColor of Material
							Connections.Append("connectAttr \"").Append(EmissionTexNode.MayaName).Append(".oc\" \"").Append(NewMat.MayaName).AppendLine(".ic\";");
							
							mPlace2dTexture Place2DTex = new mPlace2dTexture();
							Place2DTex.TexTiling = TexTiling;
							Place2DTex.TexOffset = TexOffset;
							Place2DTex.MayaName = GetUniqueGlobalNodeName("place2dTexture");
							MaterialNodes.Add(Place2DTex);
							
							// Makes the Place2DTexture node show up in the HyperShade
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).AppendLine(".msg\" \":defaultRenderUtilityList1.u\" -na;");
							
							// Connections from Place2DTexture to File Texture
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".c\" \"").Append(EmissionTexNode.MayaName).AppendLine(".c\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".tf\" \"").Append(EmissionTexNode.MayaName).AppendLine(".tf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".rf\" \"").Append(EmissionTexNode.MayaName).AppendLine(".rf\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mu\" \"").Append(EmissionTexNode.MayaName).AppendLine(".mu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".mv\" \"").Append(EmissionTexNode.MayaName).AppendLine(".mv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".s\" \"").Append(EmissionTexNode.MayaName).AppendLine(".s\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wu\" \"").Append(EmissionTexNode.MayaName).AppendLine(".wu\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".wv\" \"").Append(EmissionTexNode.MayaName).AppendLine(".wv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".re\" \"").Append(EmissionTexNode.MayaName).AppendLine(".re\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".of\" \"").Append(EmissionTexNode.MayaName).AppendLine(".of\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".r\" \"").Append(EmissionTexNode.MayaName).AppendLine(".ro\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".n\" \"").Append(EmissionTexNode.MayaName).AppendLine(".n\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt1\" \"").Append(EmissionTexNode.MayaName).AppendLine(".vt1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt2\" \"").Append(EmissionTexNode.MayaName).AppendLine(".vt2\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vt3\" \"").Append(EmissionTexNode.MayaName).AppendLine(".vt3\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".vc1\" \"").Append(EmissionTexNode.MayaName).AppendLine(".vc1\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".o\" \"").Append(EmissionTexNode.MayaName).AppendLine(".uv\";");
							Connections.Append("connectAttr \"").Append(Place2DTex.MayaName).Append(".ofs\" \"").Append(EmissionTexNode.MayaName).AppendLine(".fs\";");
						}
					}
				}
			}
		}
	}
	
	// --------------------------------------------------
	// Get Full Texture Path
	// --------------------------------------------------
	// Given a Texture, this will return the full file path
	string GetFullTexturePath(Texture t){
		// Get the application data path
		// Note - This will be a path to the assets folder
		string dataPath = Application.dataPath;
		string[] tokens = dataPath.Split('/');
		dataPath = "";
		// Go through and rebuild path
		// Note - We go through Length - 1 since we want
		// to strip off the extra Assets token
		for(int i=0; i<tokens.Length - 1; i++){
			dataPath += tokens[i] + "/";
		}
		
		// Get the local path
		// Note - this will be a path FROM the assets folder
		// to the texture
		string localPath = AssetDatabase.GetAssetPath(t);
		
		// Return the full texture path
		return (dataPath + localPath);
	}

	// --------------------------------------------------
	// Get Texture Name
	// --------------------------------------------------
	// Given a Texture, this will return the name of the texture asset + file extension
	string GetTextureNameExt(Texture t){
		// Get the local path
		// Note - this will be a path FROM the assets folder
		// to the texture
		string localPath = AssetDatabase.GetAssetPath(t);
		
		// Split the string
		string[] tokens = localPath.Split('/');
		
		return tokens[tokens.Length - 1];
	}

	// Given a Texture, this will return just the name of the texture asset
	string GetTextureName(Texture t){
		// Get the local path
		// Note - this will be a path FROM the assets folder
		// to the texture
		string localPath = AssetDatabase.GetAssetPath(t);
		
		// Split the string
		string[] tokens = localPath.Split('/');
		tokens = tokens[tokens.Length - 1].Split('.');
		
		return tokens[0];
	}

	// --------------------------------------------------
	// Create DisplayLayer List
	// --------------------------------------------------
	void CreateDisplayLayers(){
		// Go through the NodesList, and check if the object
		// is set to lightmap static. If it is, create the DisplayLayer 
		// for the lightmap index and add the MayaNode to it
		for(int i=0; i<DAGNodes.Count; i++){
			if(!(DAGNodes[i] is mTransform)) continue;
			
			// Lightmap Index
			int LightmapIndex = -1;
			
			// Get the lightmap index based on type of object
			string UnityObjectType = GetUnityObjectType(DAGNodes[i]);
			
			if(UnityObjectType == "Mesh"){
				Renderer RendererCheck = DAGNodes[i].UnityObject.gameObject.GetComponent<Renderer>();
				if(RendererCheck != null){
					LightmapIndex = RendererCheck.lightmapIndex;
				}
			}
			if(UnityObjectType == "SkinnedMesh"){
				LightmapIndex = DAGNodes[i].UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().lightmapIndex;
			}
			if(UnityObjectType == "Terrain"){
				LightmapIndex = DAGNodes[i].UnityObject.gameObject.GetComponent<Terrain>().lightmapIndex;
			}
			
			// If lightmap index isn't valid, skip this object
			if(LightmapIndex > 253) continue;
			if(LightmapIndex < 0) continue;
			
			// Find the DisplayLayer who's lightmap index matches
			int DisplayLayerIndex = -1;
			for(int d=0; d<DisplayLayerList.Count; d++){
				if(DisplayLayerList[d].LightmapIndex == LightmapIndex){
					DisplayLayerIndex = d;
					break;
				}
			}
					
			// If the DisplayLayer was not found, create it
			if(DisplayLayerIndex == -1){
				mDisplayLayer DisplayLayer = new mDisplayLayer();
				DisplayLayer.LightmapIndex = LightmapIndex;
				DisplayLayerList.Add(DisplayLayer);
						
				// Update index to latest index
				DisplayLayerIndex = DisplayLayerList.Count - 1;
			}
					
			// Add MayaNode Parent (transform) to display layer Objects list
			DisplayLayerList[DisplayLayerIndex].Objects.Add(DAGNodes[i]);
		}
		// Sort the DisplayLayers based on LightmapIndex property
		DisplayLayerList = DisplayLayerList.OrderBy(o=>o.LightmapIndex).ToList();
	}
	
	#region File Writing
	// --------------------------------------------------
	// Begin writing to a file
	// --------------------------------------------------
	// Note - First we pass the file name to the StreamWriter and tell it to NOT
	// append data. This will then erase any contents that were previously inside
	// the file
	void StartNewFile(string filePath, string fileName){
		// Erase the file contents
		using (StreamWriter writer = new StreamWriter(filePath + fileName, false)){
			writer.Write("");
		}
		
		StringBuilder data = new StringBuilder();
		
		// --------------------------------------------------
		// Adds Maya Scene File Header Info
		// --------------------------------------------------
		// This is required for any Maya Scene File.
		// This is also where you specify which version of Maya
		// the scene file should open on
		data.Append("//Maya ASCII ").Append(MayaVersions[MayaVersionIndex]).AppendLine(" scene");
		data.Append("requires maya \"").Append(MayaVersions[MayaVersionIndex]).AppendLine("\";");
		data.Append("currentUnit -l ").Append(MayaUnits[MayaUnitsIndex]).AppendLine(" -a degree -t film;");
		data.AppendLine("fileInfo \"application\" \"maya\";");
		data.Append("fileInfo \"product\" \"Maya ").Append(MayaVersions[MayaVersionIndex]).AppendLine("\";");
		data.Append("fileInfo \"version\" \"").Append(MayaVersions[MayaVersionIndex]).AppendLine("\";");
		
		// --------------------------------------------------
		// Adds Maya Default Cameras (persp, front, top, side)
		// --------------------------------------------------
		// You don't NEED this, but if you don't the organization in the Outliner
		// becomes very confusing, with all the objects listed before the cameras.
		// This will force the cameras to be created first and makes the objects
		// show up in the correct order
		data.AppendLine("createNode transform -s -n \"persp\";");
			data.AppendLine("\tsetAttr \".v\" no;");
			data.AppendLine("\tsetAttr \".t\" -type \"double3\" 57 43 57 ;");
			data.AppendLine("\tsetAttr \".r\" -type \"double3\" -28.076862662266123 44.999999999999986 8.9959671327898901e-015 ;");
		data.AppendLine("createNode camera -s -n \"perspShape\" -p \"persp\";");
			data.AppendLine("\tsetAttr -k off \".v\" no;");
			data.AppendLine("\tsetAttr \".fl\" 34.999999999999993;");
			data.AppendLine("\tsetAttr \".fcp\" 1000;");
			data.AppendLine("\tsetAttr \".coi\" 91.361917668140052;");
			data.AppendLine("\tsetAttr \".imn\" -type \"string\" \"persp\";");
			data.AppendLine("\tsetAttr \".den\" -type \"string\" \"persp_depth\";");
			data.AppendLine("\tsetAttr \".man\" -type \"string\" \"persp_mask\";");
			data.AppendLine("\tsetAttr \".hc\" -type \"string\" \"viewSet -p %camera\";");
		data.AppendLine("createNode transform -s -n \"top\";");
			data.AppendLine("\tsetAttr \".v\" no;");
			data.AppendLine("\tsetAttr \".t\" -type \"double3\" 0 100.1 0 ;");
			data.AppendLine("\tsetAttr \".r\" -type \"double3\" -89.999999999999986 0 0 ;");
		data.AppendLine("createNode camera -s -n \"topShape\" -p \"top\";");
			data.AppendLine("\tsetAttr -k off \".v\" no;");
			data.AppendLine("\tsetAttr \".rnd\" no;");
			data.AppendLine("\tsetAttr \".fcp\" 1000;");
			data.AppendLine("\tsetAttr \".coi\" 100.1;");
			data.AppendLine("\tsetAttr \".ow\" 30;");
			data.AppendLine("\tsetAttr \".imn\" -type \"string\" \"top\";");
			data.AppendLine("\tsetAttr \".den\" -type \"string\" \"top_depth\";");
			data.AppendLine("\tsetAttr \".man\" -type \"string\" \"top_mask\";");
			data.AppendLine("\tsetAttr \".hc\" -type \"string\" \"viewSet -t %camera\";");
			data.AppendLine("\tsetAttr \".o\" yes;");
		data.AppendLine("createNode transform -s -n \"front\";");
			data.AppendLine("\tsetAttr \".v\" no;");
			data.AppendLine("\tsetAttr \".t\" -type \"double3\" 0 0 100.1 ;");
		data.AppendLine("createNode camera -s -n \"frontShape\" -p \"front\";");
			data.AppendLine("\tsetAttr -k off \".v\" no;");
			data.AppendLine("\tsetAttr \".rnd\" no;");
			data.AppendLine("\tsetAttr \".fcp\" 1000;");
			data.AppendLine("\tsetAttr \".coi\" 100.1;");
			data.AppendLine("\tsetAttr \".ow\" 30;");
			data.AppendLine("\tsetAttr \".imn\" -type \"string\" \"front\";");
			data.AppendLine("\tsetAttr \".den\" -type \"string\" \"front_depth\";");
			data.AppendLine("\tsetAttr \".man\" -type \"string\" \"front_mask\";");
			data.AppendLine("\tsetAttr \".hc\" -type \"string\" \"viewSet -f %camera\";");
			data.AppendLine("\tsetAttr \".o\" yes;");
		data.AppendLine("createNode transform -s -n \"side\";");
			data.AppendLine("\tsetAttr \".v\" no;");
			data.AppendLine("\tsetAttr \".t\" -type \"double3\" 100.1 0 0 ;");
			data.AppendLine("\tsetAttr \".r\" -type \"double3\" 0 89.999999999999986 0 ;");
		data.AppendLine("createNode camera -s -n \"sideShape\" -p \"side\";");
			data.AppendLine("\tsetAttr -k off \".v\" no;");
			data.AppendLine("\tsetAttr \".rnd\" no;");
			data.AppendLine("\tsetAttr \".fcp\" 1000;");
			data.AppendLine("\tsetAttr \".coi\" 100.1;");
			data.AppendLine("\tsetAttr \".ow\" 30;");
			data.AppendLine("\tsetAttr \".imn\" -type \"string\" \"side\";");
			data.AppendLine("\tsetAttr \".den\" -type \"string\" \"side_depth\";");
			data.AppendLine("\tsetAttr \".man\" -type \"string\" \"side_mask\";");
			data.AppendLine("\tsetAttr \".hc\" -type \"string\" \"viewSet -s %camera\";");
			data.AppendLine("\tsetAttr \".o\" yes;");
			
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write DAGNodes
	// --------------------------------------------------
	void WriteDAGNodes(string filePath, string fileName){
		for(int i=0; i<DAGNodes.Count; i++){
			EditorUtility.DisplayProgressBar(ProgressTitle, "Writing " + DAGNodes[i].MayaName, CurrentProgress / MaxProgress);
			WriteNode(DAGNodes[i], filePath, fileName);
			CurrentProgress++;
		}
	}
	
	// --------------------------------------------------
	// Write AUXNodes
	// --------------------------------------------------
	void WriteAUXNodes(string filePath, string fileName){
		for(int i=0; i<AUXNodes.Count; i++){
			EditorUtility.DisplayProgressBar(ProgressTitle, "Writing " + AUXNodes[i].MayaName, CurrentProgress / MaxProgress);
			WriteNode(AUXNodes[i], filePath, fileName);
			CurrentProgress++;
		}
	}
	
	// --------------------------------------------------
	// Write LightLinker
	// --------------------------------------------------
	// Note - We set these values to the number of materials + lights + 1
	// because the initial particleCloud1 shader gets included into this list
	void WriteLightLinker(string filePath, string fileName){
		StringBuilder data = new StringBuilder();
		
		// Get the total number of lights
		int LightCount = 0;
		for(int i=0; i<DAGNodes.Count; i++){
			if(DAGNodes[i] is mDirectionalLight) LightCount++;
			if(DAGNodes[i] is mPointLight) LightCount++;
			if(DAGNodes[i] is mSpotLight) LightCount++;
			if(DAGNodes[i] is mAreaLight) LightCount++;
		}
		
		// Get the total number of materials
		int MaterialCount = 0;
		for(int i=0; i<MaterialNodes.Count; i++){
			if(MaterialNodes[i] is mBlinn) MaterialCount++;
		}
		
		// Total Amount is Number of Lights + Number of Materials
		data.AppendLine("createNode lightLinker -s -n \"lightLinker1\";");
			data.Append("\tsetAttr -s ").Append(LightCount + MaterialCount + 2).AppendLine(" \".lnk\";");
			data.Append("\tsetAttr -s ").Append(LightCount + MaterialCount + 2).AppendLine(" \".slnk\";");
			
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write renderPartition
	// --------------------------------------------------
	// Note - We set these values to the number of materials + lights + 1
	// because the initial particleCloud1 shader gets included into this list
	void WriteRenderPartition(string filePath, string fileName){
		StringBuilder data = new StringBuilder();
		
		// Get the total number of materials
		int MaterialCount = 0;
		for(int i=0; i<MaterialNodes.Count; i++){
			if(MaterialNodes[i] is mBlinn) MaterialCount++;
		}
		
		data.AppendLine("select -ne :renderPartition;");
			data.Append("\tsetAttr -s ").Append(MaterialCount + 2).AppendLine(" \".st\";");
			
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
		
	// --------------------------------------------------
	// Write defaultShaderList
	// --------------------------------------------------
	// Note - We set these values to the number of materials + lights + 1
	// because the initial particleCloud1 shader gets included into this list
	void WriteDefaultShaderList(string filePath, string fileName){
		StringBuilder data = new StringBuilder();
		
		// Get the total number of materials
		int MaterialCount = 0;
		for(int i=0; i<MaterialNodes.Count; i++){
			if(MaterialNodes[i] is mBlinn) MaterialCount++;
		}
		
		data.AppendLine("select -ne :defaultShaderList1;");
			data.Append("\tsetAttr -s ").Append(MaterialCount + 2).AppendLine(" \".s\";");
				
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write DefaultTextureList
	// --------------------------------------------------
	// This will write out the defaultRenderUtilityList and set
	// the size of it to our number of File Textures + Bump Map Nodes
	void WriteDefaultRenderUtilityList(string filePath, string fileName){
		StringBuilder data = new StringBuilder();
		
		// Get the total number of file and bump2d nodes
		int FileAndBumpCount = 0;
		for(int i=0; i<MaterialNodes.Count; i++){
			if(MaterialNodes[i] is mFile) FileAndBumpCount++;
			if(MaterialNodes[i] is mBump2d) FileAndBumpCount++;
		}
		
		data.AppendLine("select -ne :defaultRenderUtilityList1;");
			data.Append("\tsetAttr -s ").Append(FileAndBumpCount).AppendLine(" \".u\";");
	
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write Initial Shading Group Setup
	// --------------------------------------------------
	// We need this in order to allow default Lambert to be
	// assigned to objects
	void WriteInitialShadingGroupSetup(string filePath, string fileName){
		StringBuilder data = new StringBuilder();
		
		data.AppendLine("select -ne :initialShadingGroup;");
			data.AppendLine("\tsetAttr -s 2 \".dsm\";");
			data.AppendLine("\tsetAttr \".ro\" yes;");
			
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write MaterialNodes
	// --------------------------------------------------
	void WriteMaterials(string filePath, string fileName){
		for(int i=0; i<MaterialNodes.Count; i++){
			string MaterialDisplayName = "NULL";
			if(MaterialNodes[i].UnityMaterial != null) MaterialDisplayName = MaterialNodes[i].MayaName;
			EditorUtility.DisplayProgressBar(ProgressTitle, "Writing Material: " + MaterialDisplayName, CurrentProgress / MaxProgress);
			WriteNode(MaterialNodes[i], filePath, fileName);
			CurrentProgress++;
		}
	}
	
	// --------------------------------------------------
	// Write Display Layers to file
	// --------------------------------------------------
	void WriteDisplayLayers(string filePath, string fileName){
		StringBuilder data = new StringBuilder();
		
		data.AppendLine("createNode displayLayerManager -n \"layerManager\";");
			string LayerOrder = "";
			for(int i=0; i<DisplayLayerList.Count; i++){
				LayerOrder += (i+1) + " ";
			}
			data.Append("\tsetAttr -s ").Append(DisplayLayerList.Count.ToString()).Append(" \".dli[").Append(DisplayLayerList.Count > 1 ? "1:" : "").Append(DisplayLayerList.Count).Append("]\" ").Append(LayerOrder).AppendLine(";");
			data.Append("\tsetAttr -s ").Append(DisplayLayerList.Count.ToString()).AppendLine(" \".dli\";");
		Connections.AppendLine("connectAttr \"layerManager.dli[0]\" \"defaultLayer.id\";");
			
		// Write out each display layer and connections
		for(int i=0; i<DisplayLayerList.Count; i++){
			data.Append("createNode displayLayer -n \"Lightmap_Layer_").Append(i.ToString()).AppendLine("\";");
				data.Append("\tsetAttr \".do\" ").Append(i+1).AppendLine(";");
			for(int j=0; j<DisplayLayerList[i].Objects.Count; j++){
				Connections.Append("connectAttr \"Lightmap_Layer_").Append(i.ToString()).Append(".di\" \"").Append(GetDAGPath(DisplayLayerList[i].Objects[j])).AppendLine(".do\";");
			}
			Connections.Append("connectAttr \"layerManager.dli[").Append(i+1).Append("]\" \"Lightmap_Layer_").Append(i.ToString()).AppendLine(".id\";");
		}
		
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write Node
	// --------------------------------------------------
	void WriteNode(MayaNode Node, string filePath, string fileName){
		StringBuilder data = new StringBuilder();
	
		// --------------------------------------------------
		// Transform
		// --------------------------------------------------
		if(Node is mTransform){
			// Get transform properties
			Vector3 t;
			Vector3 r;
			Vector3 s;
			
			// Check for override values, if they are null, use UnityObject values
			
			// Translate Override
			if(((mTransform)Node).UseTranslateOverride){
				t = ((mTransform)Node).TranslateOverride; t = MayaConvert.MayaTranslation(t);
			}
			else{
				t = Node.UnityObject.localPosition; t = MayaConvert.MayaTranslation(t);
			}
			// Rotate Override
			if(((mTransform)Node).UseRotateOverride){
				r = ((mTransform)Node).RotateOverride; r = MayaConvert.MayaRotation(r);
			}
			else{
				r = Node.UnityObject.localRotation.eulerAngles; r = MayaConvert.MayaRotation(r);
			}
			// Scale Override
			if(((mTransform)Node).UseScaleOverride){
				s = ((mTransform)Node).ScaleOverride;
			}
			else{
				s = Node.UnityObject.localScale;
			}
			
			// If transform has no parent
			if(Node.Parent == null) data.Append("createNode transform -n \"").Append(Node.MayaName).AppendLine("\";");
			// If transform has a parent
			else data.Append("createNode transform -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
			
			// Set visible
			if(!Node.UnityObject.gameObject.activeSelf){
				data.AppendLine("\tsetAttr \".v\" no;");
			}
			
			// Add transformation data
			data.Append("\tsetAttr \".t\" -type \"double3\" ").Append(t.x).Append(" ").Append(t.y).Append(" ").Append(t.z).AppendLine(";");
			data.Append("\tsetAttr \".r\" -type \"double3\" ").Append(r.x).Append(" ").Append(r.y).Append(" ").Append(r.z).AppendLine(";");
			data.Append("\tsetAttr \".s\" -type \"double3\" ").Append(s.x).Append(" ").Append(s.y).Append(" ").Append(s.z).AppendLine(";");
			
			if(((mTransform)Node).LockTransform){               
				data.AppendLine("\tsetAttr -l on \".tx\";");
				data.AppendLine("\tsetAttr -l on \".ty\";");
				data.AppendLine("\tsetAttr -l on \".tz\";");
				data.AppendLine("\tsetAttr -l on \".rx\";");
				data.AppendLine("\tsetAttr -l on \".ry\";");
				data.AppendLine("\tsetAttr -l on \".rz\";");
				data.AppendLine("\tsetAttr -l on \".sx\";");
				data.AppendLine("\tsetAttr -l on \".sy\";");
				data.AppendLine("\tsetAttr -l on \".sz\";");
			}
			
			// Set rotation order to ZXY instead of XYZ
			data.AppendLine("\tsetAttr \".ro\" 2;");
		}
		
		// --------------------------------------------------
		// Mesh
		// --------------------------------------------------
		// 		Used to write Meshes
		//		Used to write SkinnedMeshes
		//		Used to write Terrains
		// --------------------------------------------------
		if(Node is mMesh){
			// Get UnityObject Type so we can get the mesh data correctly (Mesh, SkinnedMesh, Terrain)
			string UnityObjectType = GetUnityObjectType(Node);

			// --------------------------------------------------
			// Gather mesh data
			// --------------------------------------------------
			Mesh m;
			Vector3[] verts;
			Vector2[] uvs;
			Vector2[] uvs2;
			Color[] colors;
			Vector3[] normals;
			int[] tris;
			
			bool ExportLightmapUVs = true;
			Vector4 tilingOffset = new Vector4();
			
			StringBuilder InstObjGroups;
			string InstObjGroupsStr = "";

			// --------------------------------------------------
			// Mesh
			// --------------------------------------------------
			if(UnityObjectType == "Mesh"){
				m = Node.UnityObject.gameObject.GetComponent<MeshFilter>().sharedMesh;
				verts = m.vertices;
				uvs = m.uv;
				uvs2 = m.uv2;
				colors = m.colors;
				normals = m.normals;
				tris = m.triangles;
			
				// Renderer Component Check			
				Renderer RendererComponentCheck = Node.UnityObject.gameObject.GetComponent<Renderer>();
				if(RendererComponentCheck == null) ExportLightmapUVs = false;
				
				// --------------------------------------------------
				// Only continue with the next sections if there
				// is a Renderer component attached
				// --------------------------------------------------
				if(RendererComponentCheck != null){
					// Get the Lightmap tiling and offset			
					if(uvs2.Length > 0 && ExportLightmapUVs) tilingOffset = RendererComponentCheck.lightmapScaleOffset;
					
					// Get the sub-mesh count
					int SubMeshCount = m.subMeshCount;
					if(SubMeshCount > 1){
						// --------------------------------------------------
						// Sub Mesh - Per face material assignment
						// --------------------------------------------------
						// If the sub mesh count is greater than 1, we have
						// per face material assignment. If this is the case, write
						// out the per face assignments
						InstObjGroups = new StringBuilder();
							
						// Skinned meshes wont write the InstanceObjectGroups, so check if we should write it
						if(!((mMesh)Node).IntermediateObject){
							// Set the instObjGroup size to the number of sub meshes
							InstObjGroups.Append("\tsetAttr -s ").Append(SubMeshCount).AppendLine(" \".iog[0].og\";");
									
							// Go through each sub mesh and add its face assignments
							int TotalSubMeshTris = 0;
							for(int i=0; i<SubMeshCount; i++){
								// Get the sub mesh triangle count
								int[] SubTriangles = m.GetTriangles(i);
								InstObjGroups.Append("\tsetAttr \".iog[0].og[").Append(i).Append("].gcl\" -type \"componentList\" 1 \"f[").Append((SubTriangles.Length / 3) > 1 ? (TotalSubMeshTris.ToString() + ":") : "").Append(((SubTriangles.Length / 3) - 1) + TotalSubMeshTris).AppendLine("]\";");
									
								// Increment TotalSubMeshTris
								TotalSubMeshTris += SubTriangles.Length / 3;
							}
						}
						InstObjGroupsStr = InstObjGroups.ToString();
					}
				}
				
				// --------------------------------------------------
				// Write Mesh
				// --------------------------------------------------
				WriteMesh(filePath, fileName, Node.MayaName, Node.Parent, ((mMesh)Node).IntermediateObject, verts, uvs, uvs2, colors, normals, tris, InstObjGroupsStr, tilingOffset, ExportLightmapUVs);
			}
			// --------------------------------------------------
			// SkinnedMesh
			// --------------------------------------------------
			if(UnityObjectType == "SkinnedMesh"){
				m = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
				verts = m.vertices;
				uvs = m.uv;
				uvs2 = m.uv2;
				colors = m.colors;
				normals = m.normals;
				tris = m.triangles;
				
				// Get the Lightmap tiling and offset			
				if(uvs2.Length > 0 && ExportLightmapUVs) tilingOffset = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().lightmapScaleOffset;
					
				// Get the sub-mesh count
				int SubMeshCount = m.subMeshCount;
				if(SubMeshCount > 1){
					// --------------------------------------------------
					// Sub Mesh - Per face material assignment
					// --------------------------------------------------
					// If the sub mesh count is greater than 1, we have
					// per face material assignment. If this is the case, write
					// out the per face assignments
					InstObjGroups = new StringBuilder();
						
					// Skinned meshes wont write the InstanceObjectGroups, so check if we should write it
					if(!((mMesh)Node).IntermediateObject){
						// Set the instObjGroup size to the number of sub meshes
						InstObjGroups.Append("\tsetAttr -s ").Append(SubMeshCount).AppendLine(" \".iog[0].og\";");
								
						// Go through each sub mesh and add its face assignments
						int TotalSubMeshTris = 0;
						for(int i=0; i<SubMeshCount; i++){
							// Get the sub mesh triangle count
							int[] SubTriangles = m.GetTriangles(i);
							InstObjGroups.Append("\tsetAttr \".iog[0].og[").Append(i).Append("].gcl\" -type \"componentList\" 1 \"f[").Append((SubTriangles.Length / 3) > 1 ? (TotalSubMeshTris.ToString() + ":") : "").Append(((SubTriangles.Length / 3) - 1) + TotalSubMeshTris).AppendLine("]\";");
								
							// Increment TotalSubMeshTris
							TotalSubMeshTris += SubTriangles.Length / 3;
						}
					}
					InstObjGroupsStr = InstObjGroups.ToString();
				}
				
				// --------------------------------------------------
				// Write SkinnedMesh
				// --------------------------------------------------
				WriteMesh(filePath, fileName, Node.MayaName, Node.Parent, ((mMesh)Node).IntermediateObject, verts, uvs, uvs2, colors, normals, tris, InstObjGroupsStr, tilingOffset, ExportLightmapUVs);
			}
		}

		// --------------------------------------------------
		// Terrain
		// --------------------------------------------------		
		if(Node is mTerrain){
			// --------------------------------------------------
			// Get Terrain data
			// --------------------------------------------------
			EditorUtility.DisplayProgressBar(ProgressTitle, "Gathering " + Node.MayaName + " Data", CurrentProgress / MaxProgress);
			TerrainData tData = Node.UnityObject.gameObject.GetComponent<Terrain>().terrainData;
			int Width = tData.heightmapWidth;
			int Height = tData.heightmapHeight;
			Vector3 TerrainSize = tData.size;
			Vector2 uvScale = new Vector2(1.0f / (Width - 1), 1.0f / (Height - 1));
			float[,] tHeights = tData.GetHeights(0, 0, Width, Height);
			
			// Check if this terrain is light mapped
			Vector4 tilingOffset = new Vector4();
			bool DoLightmap = false;
			int LightmapIndex = Node.UnityObject.gameObject.GetComponent<Terrain>().lightmapIndex;
			if(LightmapIndex < 254 && LightmapIndex > -1){
				DoLightmap = true;
				tilingOffset = Node.UnityObject.gameObject.GetComponent<Terrain>().lightmapScaleOffset;
			}
			
		
			// --------------------------------------------------
			// Generate Mesh Variables
			// --------------------------------------------------
			MayaTerrainFace[,] FaceList = new MayaTerrainFace[Width - 1, Height - 1];
			List<MayaEdge> EdgeList = new List<MayaEdge>();
			Vector3[] verts = new Vector3[Width * Height];
			Vector2[] uvs = new Vector2[Width * Height];
			Vector2[] uv2 = new Vector2[Width * Height];

			// --------------------------------------------------
			// Build Vertex, UV and UV2 Position Arrays
			// --------------------------------------------------
			EditorUtility.DisplayProgressBar(ProgressTitle, "Building " + Node.MayaName + " Point Data Lists", CurrentProgress / MaxProgress);
			int IndexCounter = 0;
			for(int y=0; y<Height; y++){
				for(int x=0; x<Width; x++){
					verts[IndexCounter] = new Vector3(x * (TerrainSize.x / (Width - 1)), tHeights[y,x] * TerrainSize.y, y * (TerrainSize.z / (Height - 1)));
					
					// Generate Terrain UVs
					// Note - X and Y are swapped here so the UVs are generated in correct UV orientation
					uvs[IndexCounter] = Vector2.Scale( new Vector2(x,y), uvScale);
					
					// Optional - Generate Terrain Lightmap UVs
					if(DoLightmap){
						Vector2 UV2 = Vector2.Scale( new Vector2(x,y), uvScale);
						uv2[IndexCounter] = new Vector2((UV2.x * tilingOffset.x) + tilingOffset.z, (UV2.y * tilingOffset.y) + tilingOffset.w);
					}
					
					IndexCounter++;
				}
			}
				
			// --------------------------------------------------
			// Build Terrain Faces
			// --------------------------------------------------
			// Note - Formula for indexing into the flat verts array:
			// (Y * Width) + X
			EditorUtility.DisplayProgressBar(ProgressTitle, "Building " + Node.MayaName + " Terrain Faces", CurrentProgress / MaxProgress);
			for(int y=0; y<(Height - 1); y++){
				for(int x=0; x<(Width - 1); x++){
					//	Image Orientation
					//     _________________
					//     |                |
					//     |                |
					//     |    Image       | |
					//     |                | |
					//     |                |
					//     |                | +
					//     |________________| X
					//                --- +Y
                    //    |
                    //    Y+                                            
                    //  ______                ______                __4___
                    //  |    /|               |    /  /|            |    /     /|
                    //  |   / |               | B /  / |            | B /     / |
                    //  |  /  |               |  /  /  |           5|  /3   2/  |1
                    //  | /   |               | /  / A |            | /     / A |
                    //  |/____| X+ --         |/  /____|            |/     /____|
                    //                                                        0
                    //  Terrain Layout       Face Triangles         Edge Numbers
					//
					// When building each new face in order:
					//		Edge 1 : Never shared
					//		Edge 2 : Never shared
					//		Edge 4 : Never shared
					//		Edge 3 : Always shared
					//		Edge 0 : Sometimes shared
					//		Edge 5 : Sometimes shared
	
					// Create a new MayaTerrainFace
					MayaTerrainFace NewFace = new MayaTerrainFace();
					MayaEdge e;
					
					// --------------------------------------------------
					// Triangle 1
					// --------------------------------------------------
					// Edge 0
					if(y == 0){
						e = new MayaEdge((y * Width) + x, (y * Width) + (x+1));
						EdgeList.Add(e);
						NewFace.EdgeIndex0 = EdgeList.Count - 1;
					}
					else{
						NewFace.EdgeIndex0 = FaceList[y-1, x].EdgeIndex4;
					}
					
					// Edge 1
					e = new MayaEdge((y * Width) + (x+1), ((y+1) * Width) + (x+1));
					EdgeList.Add(e);
					NewFace.EdgeIndex1 = EdgeList.Count - 1;
						
					// Edge 2
					e = new MayaEdge(((y+1) * Width) + (x+1), (y * Width) + x);
					EdgeList.Add(e);
					NewFace.EdgeIndex2 = EdgeList.Count - 1;
					
					// UV Indices
					NewFace.UVIndex0 = (y * Width) + x;
					NewFace.UVIndex1 = (y * Width) + (x+1);
					NewFace.UVIndex2 = ((y+1) * Width) + (x+1);
					
					// --------------------------------------------------
					// Triangle 2
					// --------------------------------------------------
					// Edge 3
					NewFace.EdgeIndex3 = NewFace.EdgeIndex2;
					
					// Edge 4
					e = new MayaEdge(((y+1) * Width) + (x+1), ((y+1) * Width) + x);
					EdgeList.Add(e);
					NewFace.EdgeIndex4 = EdgeList.Count - 1;
					
					// Edge 5
					if(x== 0){
						e = new MayaEdge(((y+1) * Width) + x, (y * Width) + x);
						EdgeList.Add(e);
						NewFace.EdgeIndex5 = EdgeList.Count - 1;
					}
					else{
						NewFace.EdgeIndex5 = FaceList[y, x-1].EdgeIndex1;
					}
					
					// UV Indices
					NewFace.UVIndex3 = (y * Width) + x;
					NewFace.UVIndex4 = ((y+1) * Width) + (x+1);
					NewFace.UVIndex5 = ((y+1) * Width) + x;
					
					// --------------------------------------------------
					// Add this face to the FaceList
					// --------------------------------------------------
					FaceList[y,x] = NewFace;
				}
			}

			// --------------------------------------------------
			// Write shape attributes
			// --------------------------------------------------
			data.Append("createNode mesh -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
			data.AppendLine("\tsetAttr -k off \".v\";");
			data.AppendLine("\tsetAttr \".vir\" yes;");
			data.AppendLine("\tsetAttr \".vif\" yes;");
			
			// --------------------------------------------------
			// Write UV Data
			// --------------------------------------------------
			EditorUtility.DisplayProgressBar(ProgressTitle, "Writing " + Node.MayaName + " UV Data", CurrentProgress / MaxProgress);
			int ColumnCounter = 0;
			data.AppendLine("\tsetAttr \".uvst[0].uvsn\" -type \"string\" \"map1\";");
			data.Append("\tsetAttr -s ").Append(verts.Length).Append(" \".uvst[0].uvsp[").Append(verts.Length > 1 ? "0:" : "").Append(verts.Length - 1).AppendLine("]\" -type \"float2\" ");
			for(int i=0; i<uvs.Length; i++){
				if(ColumnCounter == 0) data.Append("\t\t");
				data.Append(uvs[i].x).Append(" ").Append(uvs[i].y).Append(" ");
				if(ColumnCounter == ColumnDataWidth) data.AppendLine();
				
				// Increment column counter
				ColumnCounter++;
				if(ColumnCounter > ColumnDataWidth) ColumnCounter = 0;
			}
			data.AppendLine(";");
			data.AppendLine("\tsetAttr  \".cuvs\" -type \"string\" \"map1\";");	// Set the current UV set to the main UV set
				
			// --------------------------------------------------
			// Write UV2 Data
			// --------------------------------------------------
			if(DoLightmap){
				EditorUtility.DisplayProgressBar(ProgressTitle, "Writing " + Node.MayaName + " Lightmap Data", CurrentProgress / MaxProgress);
				ColumnCounter = 0;
				data.AppendLine("\tsetAttr \".uvst[1].uvsn\" -type \"string\" \"lightmapUV\";");
				data.Append("\tsetAttr -s ").Append(verts.Length).Append(" \".uvst[1].uvsp[").Append(verts.Length > 1 ? "0:" : "").Append(verts.Length - 1).AppendLine("]\" -type \"float2\" ");
				for(int i=0; i<uv2.Length; i++){
					if(ColumnCounter == 0) data.Append("\t\t");
					data.Append(uv2[i].x).Append(" ").Append(uv2[i].y).Append(" ");
					if(ColumnCounter == ColumnDataWidth) data.AppendLine();
					
					// Increment column counter
					ColumnCounter++;
					if(ColumnCounter > ColumnDataWidth) ColumnCounter = 0;
				}
				data.AppendLine(";");
			}
			
			// --------------------------------------------------
			// Write Vertex data
			// --------------------------------------------------
			// Column counter used for nicely formatting the data in the Maya file
			// NOTE - We also use it below for the edge list as well
			EditorUtility.DisplayProgressBar(ProgressTitle, "Writing " + Node.MayaName + " Vertex Data", CurrentProgress / MaxProgress);
			ColumnCounter = 0;
			data.Append("\tsetAttr -s ").Append(verts.Length).Append(" \".vt[").Append(verts.Length > 1 ? "0:" : "").Append(verts.Length - 1).AppendLine("]\" ");
			for(int i=0; i<verts.Length; i++){
				// Format vertex data
				if(ColumnCounter == 0) data.Append("\t\t");
				Vector3 MayaVert = MayaConvert.MayaTranslation(verts[i]);
				data.Append(MayaVert.x).Append(" ").Append(MayaVert.y).Append(" ").Append(MayaVert.z).Append(" ");
				if(ColumnCounter == ColumnDataWidth) data.AppendLine();
				
				// Increment column counter
				ColumnCounter++;
				if(ColumnCounter > ColumnDataWidth) ColumnCounter = 0;
			}
			data.AppendLine(";");
			
			// --------------------------------------------------
			// Write Edge data
			// --------------------------------------------------
			EditorUtility.DisplayProgressBar(ProgressTitle, "Writing " + Node.MayaName + " Edge Data", CurrentProgress / MaxProgress);
			ColumnCounter = 0;
			data.Append("\tsetAttr -s ").Append(EdgeList.Count).Append(" \".ed[").Append((EdgeList.Count > 1 ? "0:" : "")).Append(EdgeList.Count - 1).AppendLine("]\" ");
			for(int i=0; i<EdgeList.Count; i++){
				if(ColumnCounter == 0) data.Append("\t\t");
				data.Append(EdgeList[i].StartEdge).Append(" ").Append(EdgeList[i].EndEdge).Append(" 1 "); // 1 means we want soft edges
				if(ColumnCounter == ColumnDataWidth) data.AppendLine();
							
				// Increment column counter
				ColumnCounter++;
				if(ColumnCounter > ColumnDataWidth) ColumnCounter = 0;
			}
			data.AppendLine(";");
			
			// --------------------------------------------------
			// Write Face data
			// --------------------------------------------------
			EditorUtility.DisplayProgressBar(ProgressTitle, "Writing " + Node.MayaName + " Face Data", CurrentProgress / MaxProgress);
			data.Append("\tsetAttr -s ").Append(((Width - 1) * (Height - 1)) * 2).Append(" \".fc[0:").Append((((Width - 1) * (Height - 1)) * 2) - 1).AppendLine("]\" -type \"polyFaces\"");
			for(int y=0; y<(Height - 1); y++){
				for(int x=0; x<(Width - 1); x++){
					// Record first triangle data of this face
					data.Append("\t\tf 3 ").Append(FaceList[y,x].EdgeIndex0.ToString()).Append(" ").Append(FaceList[y,x].EdgeIndex1.ToString()).Append(" ").AppendLine(FaceList[y,x].EdgeIndex2.ToString());
					data.Append("\t\tmu 0 3 ").Append(FaceList[y,x].UVIndex0.ToString()).Append(" ").Append(FaceList[y,x].UVIndex1.ToString()).Append(" ").AppendLine(FaceList[y,x].UVIndex2.ToString());
					if(DoLightmap){
						data.Append("\t\tmu 1 3 ").Append(FaceList[y,x].UVIndex0.ToString()).Append(" ").Append(FaceList[y,x].UVIndex1.ToString()).Append(" ").AppendLine(FaceList[y,x].UVIndex2.ToString());
					}
					
					// Record 2nd triangle data of this face
					data.Append("\t\tf 3 ").Append(FaceList[y,x].EdgeIndex3.ToString()).Append(" ").Append(FaceList[y,x].EdgeIndex4.ToString()).Append(" ").AppendLine(FaceList[y,x].EdgeIndex5.ToString());
					data.Append("\t\tmu 0 3 ").Append(FaceList[y,x].UVIndex3.ToString()).Append(" ").Append(FaceList[y,x].UVIndex4.ToString()).Append(" ").AppendLine(FaceList[y,x].UVIndex5.ToString());
					if(DoLightmap){
						data.Append("\t\tmu 0 3 ").Append(FaceList[y,x].UVIndex3.ToString()).Append(" ").Append(FaceList[y,x].UVIndex4.ToString()).Append(" ").AppendLine(FaceList[y,x].UVIndex5.ToString());
					}
				}
			}
			// Add trailing semicolon after face setup
			data.AppendLine(";");
		}
		
		// --------------------------------------------------
		// Mesh Shell
		// --------------------------------------------------
		// This is a made up class to make it easier to export
		// skinned meshes
		if(Node is mMeshShell){
			// Get Mesh
			Mesh m = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
				
			data.Append("createNode mesh -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
				data.AppendLine("\tsetAttr -k off \".v\";");
				data.Append("\tsetAttr -s ").Append(m.subMeshCount).AppendLine(" \".iog[0].og\";");
				data.AppendLine("\tsetAttr \".vir\" yes;");
				data.AppendLine("\tsetAttr \".vif\" yes;");
				data.AppendLine("\tsetAttr \".uvst[0].uvsn\" -type \"string\" \"map1\";");
				data.AppendLine("\tsetAttr \".cuvs\" -type \"string\" \"map1\";");
				data.AppendLine("\tsetAttr \".dcc\" -type \"string\" \"Ambient+Diffuse\";");
				data.AppendLine("\tsetAttr \".vcs\" 2;");
		}		
		
		// --------------------------------------------------
		// Joint
		// --------------------------------------------------
		if(Node is mJoint){
			// Get transform properties
			Vector3 t = Node.UnityObject.localPosition; t = MayaConvert.MayaTranslation(t);
			Vector3 r = Node.UnityObject.localRotation.eulerAngles; r = MayaConvert.MayaRotation(r);
			Vector3 s = Node.UnityObject.localScale;
			
			// If transform has no parent
			if(Node.Parent == null) data.Append("createNode joint -n \"").Append(GetDAGPath(Node)).AppendLine("\";");
			else data.Append("createNode joint -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
			
			data.AppendLine("\taddAttr -ci true -sn \"liw\" -ln \"lockInfluenceWeights\" -min 0 -max 1 -at \"bool\";");
			
			// Add transformation data
			data.Append("\tsetAttr \".t\" -type \"double3\" ").Append(t.x).Append(" ").Append(t.y).Append(" ").Append(t.z).AppendLine(";");
			data.Append("\tsetAttr \".r\" -type \"double3\" ").Append(r.x).Append(" ").Append(r.y).Append(" ").Append(r.z).AppendLine(";");
			data.Append("\tsetAttr \".s\" -type \"double3\" ").Append(s.x).Append(" ").Append(s.y).Append(" ").Append(s.z).AppendLine(";");
				
			// Set rotation order to ZXY instead of XYZ
			data.AppendLine("\tsetAttr \".ro\" 2;");
		}
		
		// --------------------------------------------------		
		// SkinCluster node
		// --------------------------------------------------
		if(Node is mSkinCluster){
			data.Append("createNode skinCluster -n \"").Append(Node.MayaName).AppendLine("\";");
			
			// Weight List
			// Each entry corresponds to a vertex on the mesh
			// .wl[VertIndex].w[NumberOfJoints]
			
			// Set number of vertices on mesh:
			int VertCount = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh.vertexCount;
			data.Append("\tsetAttr -s ").Append(VertCount).AppendLine(" \".wl\";");
	
			// Get the joint transforms for this mesh
			Transform[] Joints = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().bones;
			
			// Get the bone weights for this mesh
			BoneWeight[] BoneWeights = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh.boneWeights;
			
			// Go through the bone weights
			for(int w=0; w<BoneWeights.Length; w++){
				// Determine how many bones are effecting this vertex
				float Bone0Weight = BoneWeights[w].weight0;
				float Bone1Weight = BoneWeights[w].weight1;
				float Bone2Weight = BoneWeights[w].weight2;
				float Bone3Weight = BoneWeights[w].weight3;
				
				int TotalBones = 0;
				if(Bone0Weight > 0) TotalBones++;
				if(Bone1Weight > 0) TotalBones++;
				if(Bone2Weight > 0) TotalBones++;
				if(Bone3Weight > 0) TotalBones++;
				
				// Set the weights per vertex and list each joints influence on the vertex:
				data.Append("\tsetAttr -s ").Append(TotalBones.ToString()).Append(" \".wl[").Append(w).AppendLine("].w\";");
				if(Bone0Weight > 0) data.Append("\tsetAttr \".wl[").Append(w).Append("].w[").Append(BoneWeights[w].boneIndex0.ToString()).Append("]\" ").Append(Bone0Weight.ToString()).AppendLine(";");
				if(Bone1Weight > 0) data.Append("\tsetAttr \".wl[").Append(w).Append("].w[").Append(BoneWeights[w].boneIndex1.ToString()).Append("]\" ").Append(Bone1Weight.ToString()).AppendLine(";");
				if(Bone2Weight > 0) data.Append("\tsetAttr \".wl[").Append(w).Append("].w[").Append(BoneWeights[w].boneIndex2.ToString()).Append("]\" ").Append(Bone2Weight.ToString()).AppendLine(";");
				if(Bone3Weight > 0) data.Append("\tsetAttr \".wl[").Append(w).Append("].w[").Append(BoneWeights[w].boneIndex3.ToString()).Append("]\" ").Append(Bone3Weight.ToString()).AppendLine(";");
			}
		
			// Number of driving joints:
			data.Append("\tsetAttr -s ").Append(Joints.Length).AppendLine(" \".pm\";");
			
			// World Inverse Matrix of all the joints:
			Matrix4x4[] bindposes = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh.bindposes;
			for(int i=0; i<bindposes.Length; i++){
				// Get the prebind matrix for this joint
				float[] JointMatrix = MayaConvert.Matrix4x4ToFloatArray(bindposes[i] * Node.UnityObject.worldToLocalMatrix);
				float[] ConvertMatrix = MayaConvert.UnityMatrixToMayaMatrix(JointMatrix);
				
				data.Append("\tsetAttr \".pm[").Append(i).Append("]\" -type \"matrix\" ");
				for(int j=0; j<ConvertMatrix.Length; j++){
					data.Append(ConvertMatrix[j]).Append(" ");
				}
				data.AppendLine(";");
				
			}
			
			// Geometry prebind matrix
			// Note - we just grab the mesh's localToWorldMatrix
			float[] jm = MayaConvert.Matrix4x4ToFloatArray(Node.UnityObject.localToWorldMatrix);
			float[] cm = MayaConvert.UnityMatrixToMayaMatrix(jm);
			data.Append("\tsetAttr \".gm\" -type \"matrix\" ");
			for(int i=0; i<cm.Length; i++){
					data.Append(cm[i]).Append(" ");
			}
			data.AppendLine(";");
			
			// Driving transforms array, just set to number of joints?:
			data.Append("\tsetAttr -s ").Append(Joints.Length).AppendLine(" \".ma\";");
			
			// Lock influence weights
			// Set to number of joints:
			data.Append("\tsetAttr -s ").Append(Joints.Length).AppendLine(" \".lw\";");
			
			// Maintain Max Influences
			data.AppendLine("\tsetAttr \".mmi\" yes;");
			
			// Maximum number of influences
			// I'm assuming set to 4 to match Unity?:
			data.AppendLine("\tsetAttr \".mi\" 4;");
		}
		
		// --------------------------------------------------
		// BlendShape
		// --------------------------------------------------
		if(Node is mBlendShape){
			Mesh m = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
			int NumBlendShapes = m.blendShapeCount;
			
			data.Append("createNode blendShape -n \"").Append(Node.MayaName).AppendLine("\";");
				data.AppendLine("\taddAttr -ci true -h true -sn \"aal\" -ln \"attributeAliasList\" -dt \"attributeAlias\";");
				
				// Set Topology Check - NO
				data.AppendLine("\tsetAttr \".tc\" no;");

				// Set the weights for each target
				data.Append("\tsetAttr -s ").Append(NumBlendShapes.ToString()).Append(" \".w[0");
				if(NumBlendShapes > 1){ 
					data.Append(":").Append(NumBlendShapes - 1);
				}
				data.Append("]\"  ");
				SkinnedMeshRenderer smr = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>();
				for(int i=0; i<NumBlendShapes; i++){
					// Get the weight of the target blendshape
					data.Append(smr.GetBlendShapeWeight(i) / 100).Append(" ");
				}
				data.AppendLine(";");
				
				// Go through each blendshape
				for(int i=0; i<NumBlendShapes; i++){
					// Get number of frames for this blendshape target
					int FrameCount = m.GetBlendShapeFrameCount(i);
					
					// Calculate per frame weight
					float PerFrameWeight = 1.0f / FrameCount;
				
					// Set the number of in-between frames
					data.Append("\tsetAttr -s ").Append(FrameCount).Append(" \".it[0].itg[").Append(i).AppendLine("].iti\";");
						// Go through every frame
						for(int f=0; f<FrameCount; f++){
							// Calculate frame weight
							int FrameWeight = (int)(((PerFrameWeight * f) + PerFrameWeight) * 1000) + 5000;
							
							// Set the weight for this target and the number of verts
							data.Append("\tsetAttr \".it[0].itg[").Append(i).Append("].iti[").Append(FrameWeight.ToString()).Append("].ipt\" -type \"pointArray\" ").AppendLine(m.vertexCount.ToString());
							
							// Get Blendshape deltas
							Vector3[] deltaVertices = new Vector3[m.vertexCount];
							Vector3[] deltaNormals = new Vector3[m.vertexCount];
							Vector3[] deltaTangents = new Vector3[m.vertexCount];
							m.GetBlendShapeFrameVertices(i, f, deltaVertices, deltaNormals, deltaTangents);
							
							// Set the weights, 3 values plus 1 at the end
							for(int d=0; d<deltaVertices.Length; d++){
								Vector3 ConvertedDelta = MayaConvert.MayaTranslation(deltaVertices[d]);
								data.Append("\t\t").Append(ConvertedDelta.x.ToString()).Append(" ").Append(ConvertedDelta.y.ToString()).Append(" ").Append(ConvertedDelta.z.ToString()).Append(" ").Append("1");
								if(d < deltaVertices.Length - 1) data.AppendLine();
							}
							data.Append(";").AppendLine();
							
							// Set which components this weight group effects (all)
							data.Append("\tsetAttr \".it[0].itg[").Append(i).Append("].iti[").Append(FrameWeight).Append("].ict\" -type \"componentList\" 1 \"vtx[0:").Append(m.vertexCount).AppendLine("]\";");
						}
				}

				// Set the names for the targets
				List<string> BlendShapeNames = new List<string>();
				for(int i=0; i<NumBlendShapes; i++){
					string BlendShapeName = m.GetBlendShapeName(i);
					string[] split = BlendShapeName.Split('.');
					BlendShapeNames.Add(split[split.Length - 1]);
				}
				data.Append("\tsetAttr \".aal\" -type \"attributeAlias\" {");
				for(int i=0; i<NumBlendShapes; i++){
					data.Append("\"").Append(BlendShapeNames[i]).Append("\"").Append(",").Append("\"").Append("weight[").Append(i).Append("]").Append("\"");
					if(i<BlendShapeNames.Count-1) data.Append(",");
				}
				data.AppendLine("};");
		}
		
		// --------------------------------------------------
		// ObjectSet
		// --------------------------------------------------
		if(Node is mObjectSet){
			data.Append("createNode objectSet -n \"").Append(Node.MayaName).AppendLine("\";");
				data.AppendLine("\tsetAttr \".ihi\" 0;");
				data.AppendLine("\tsetAttr \".vo\" yes;");
		}
		
		// --------------------------------------------------
		// Tweak
		// --------------------------------------------------
		if(Node is mTweak){
			data.Append("createNode tweak -n \"").Append(Node.MayaName).AppendLine("\";");
		}
		
		// --------------------------------------------------
		// GroupParts
		// --------------------------------------------------
		// Tells Maya which faces are part of the group
		if(Node is mGroupParts){
			data.Append("createNode groupParts -n \"").Append(Node.MayaName).AppendLine("\";");
			data.AppendLine("\tsetAttr \".ihi\" 0;");
			
			// If this is a blendshape GroupParts node
			if(((mGroupParts)Node).ForceAllVerts){
				data.AppendLine("\tsetAttr \".ic\" -type \"componentList\" 1 \"vtx[*]\";");
			}
			// Else it is a SkinCluster GroupParts node
			else{
				// Get the submesh indices
				if(((mGroupParts)Node).SubMeshIndex != -1){			
					// Get the materials on the mesh
					int[] SubMeshTris = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh.GetTriangles(((mGroupParts)Node).SubMeshIndex);
					
					// We need to figure out which faces correspond to the SubMeshTris vertex indices
					// So go through the triangle list and see if the SubMeshTris values match
					// if they do, record this triangle index
					List<int> FaceIndices = new List<int>();
					int[] tris = Node.UnityObject.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh.triangles;
					for(int i=0; i<SubMeshTris.Length; i+=3){
						for(int j=0; j<tris.Length; j+=3){
							if(SubMeshTris[i] == tris[j] && SubMeshTris[i+1] == tris[j+1] && SubMeshTris[i+2] == tris[j+2]) FaceIndices.Add(j/3);
						}
					}
				
					data.Append("\tsetAttr \".ic\" -type \"componentList\" ").Append(FaceIndices.Count);
					for(int i=0; i<FaceIndices.Count; i++) data.Append(" ").Append("\"f[").Append(FaceIndices[i]).Append("]\"");
					data.Append(";").AppendLine();
				}
				else{
					data.AppendLine("\tsetAttr \".ic\" -type \"componentList\" 1 \"vtx[*]\";");
				}
			}
		}
		
		// --------------------------------------------------
		// GroupID node
		// --------------------------------------------------
		if(Node is mGroupId){
			data.Append("createNode groupId -n \"").Append(Node.MayaName).AppendLine("\";");
				data.AppendLine("\tsetAttr \".ihi\" 0;");
		}
		
		// --------------------------------------------------
		// Shading Group
		// --------------------------------------------------
		if(Node is mShadingEngine){
			data.Append("createNode shadingEngine -n \"").Append(Node.MayaName).AppendLine("\";");
				data.AppendLine("\tsetAttr \".ihi\" 0;");
				data.AppendLine("\tsetAttr \".ro\" yes;");
		}

		// --------------------------------------------------
		// Material
		// --------------------------------------------------
		if(Node is mBlinn){
			data.Append("createNode blinn -n \"").Append(Node.MayaName).AppendLine("\";");
			
			// If Material is a regular material, write the attributes
			if(Node.MaterialType == MayaMaterialType.RegularMaterial){
				// Set color
				if(Node.UnityMaterial.HasProperty("_Color")){
					Color MainColor = Node.UnityMaterial.GetColor("_Color");
					data.Append("\tsetAttr \".c\" -type \"float3\" ").Append(MainColor.r.ToString()).Append(" ").Append(MainColor.g.ToString()).Append(" ").Append(MainColor.b.ToString()).AppendLine(";");
				}
	
				// Set specular color
				if(Node.UnityMaterial.HasProperty("_SpecColor")){
					Color SpecColor = Node.UnityMaterial.GetColor("_SpecColor");
					data.Append("\tsetAttr \".sc\" -type \"float3\" ").Append(SpecColor.r.ToString()).Append(" ").Append(SpecColor.g.ToString()).Append(" ").Append(SpecColor.b.ToString()).AppendLine(";");
				}
				
				// Set incandescence color
				if(Node.UnityMaterial.HasProperty("_EmissionColor")){
					Color EmissiveColor = Node.UnityMaterial.GetColor("_EmissionColor");
					data.Append("\tsetAttr \".ic\" -type \"float3\" ").Append(EmissiveColor.r.ToString()).Append(" ").Append(EmissiveColor.g.ToString()).Append(" ").Append(EmissiveColor.b.ToString()).AppendLine(";");
				}
			}
		}
		
		// --------------------------------------------------
		// File
		// --------------------------------------------------
		if(Node is mFile){
			string FullTexturePath = filePath + GetTextureNameExt(Node.UnityTexture);
			data.Append("createNode file -n \"").Append(Node.MayaName).AppendLine("\";");
				data.Append("\tsetAttr \".ftn\" -type \"string\" \"").Append(FullTexturePath).AppendLine("\";");
		}
		
		// --------------------------------------------------
		// TerrainFileAlpha
		// --------------------------------------------------
		if(Node is mTerrainFileAlpha){
			string FullTexturePath = filePath + ((mTerrainFileAlpha)Node).ImgName + ".png";
			data.Append("createNode file -n \"").Append(Node.MayaName).AppendLine("\";");
				data.Append("\tsetAttr \".ftn\" -type \"string\" \"").Append(FullTexturePath).AppendLine("\";");
		}
		
		// --------------------------------------------------
		// Ramp
		// --------------------------------------------------
		if(Node is mRamp){
			data.Append("createNode ramp -n \"").Append(Node.MayaName).AppendLine("\";");
				for(int i=0; i<((mRamp)Node).Colors.Count; i++){
					// Position
					data.Append("\tsetAttr \".cel[").Append(i.ToString()).AppendLine("].ep\" 0;");
					// Color
					data.Append("\tsetAttr \".cel[").Append(i.ToString()).Append("].ec\" -type \"float3\" ").Append(((mRamp)Node).Colors[i].r.ToString()).Append(" ").Append(((mRamp)Node).Colors[i].g.ToString()).Append(" ").Append(((mRamp)Node).Colors[i].b.ToString()).AppendLine(";");
				}
		}
		
		// --------------------------------------------------
		// Place2DTexture
		// --------------------------------------------------
		if(Node is mPlace2dTexture){
			data.Append("createNode place2dTexture -n \"").Append(Node.MayaName).AppendLine("\";");
				data.Append("\tsetAttr \".re\" -type \"float2\" ").Append(((mPlace2dTexture)Node).TexTiling.x).Append(" ").Append(((mPlace2dTexture)Node).TexTiling.y).AppendLine(";");
				data.Append("\tsetAttr \".of\" -type \"float2\" ").Append(((mPlace2dTexture)Node).TexOffset.x).Append(" ").Append(((mPlace2dTexture)Node).TexOffset.y).AppendLine(";");
		}
		
		// --------------------------------------------------
		// LayeredTexture
		// --------------------------------------------------
		if(Node is mLayeredTexture){
			int NumInputs = ((mLayeredTexture)Node).NumberOfInputs;
			
			data.Append("createNode layeredTexture -n \"").Append(Node.MayaName).AppendLine("\";");
				data.Append("\tsetAttr -s ").Append(NumInputs.ToString()).AppendLine(" \".cs\";");
				for(int i=0; i<NumInputs; i++){
					data.Append("\tsetAttr \".cs[").Append(i.ToString()).AppendLine("].a\" 1;");
					data.Append("\tsetAttr \".cs[").Append(i.ToString()).AppendLine("].bm\" 4;");
					data.Append("\tsetAttr \".cs[").Append(i.ToString()).AppendLine("].iv\" yes;");
				}
				data.AppendLine("\tsetAttr \".ail\" yes;");
		}
		
		// --------------------------------------------------
		// Bump2D
		// --------------------------------------------------
		if(Node is mBump2d){
			data.Append("createNode bump2d -n \"").Append(Node.MayaName).AppendLine("\";");
				// Set as "normal map"
				data.AppendLine("\tsetAttr \".bi\" 1;");
				// Provide 3d info (needs to be yes)
				data.AppendLine("\tsetAttr \".p3d\" yes;");
				data.Append("\tsetAttr \".bd\" ").Append(((mBump2d)Node).BumpAmount.ToString()).AppendLine(";");
		}
		
		// --------------------------------------------------
		// MaterialInfo
		// --------------------------------------------------
		if(Node is mMaterialInfo){
			data.Append("createNode materialInfo -n \"").Append(Node.MayaName).AppendLine("\";");
		}
		
		// --------------------------------------------------
		// SpotLight
		// --------------------------------------------------
		if(Node is mSpotLight){
			// Get Light data
			Light LightData = Node.UnityObject.gameObject.GetComponent<Light>();

			string LightShadow = "on";
			if(LightData.shadows == LightShadows.None) LightShadow = "off";		
		
			data.Append("createNode spotLight -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
				data.AppendLine("\tsetAttr -k off \".v\";");
				data .Append("\tsetAttr \".cl\" -type \"float3\" ").Append(LightData.color.r.ToString()).Append(" ").Append(LightData.color.g.ToString()).Append(" ").Append(LightData.color.b.ToString()).AppendLine(";");
				data.Append("\tsetAttr \".in\" ").Append(LightData.intensity).AppendLine(";");
				data.AppendLine("\tsetAttr \".de\" 1;");
				data.AppendLine("\tsetAttr \".dro\" 20;");
				data.Append("\tsetAttr \".ca\" ").Append(LightData.spotAngle).AppendLine(";");
				data.AppendLine("\tsetAttr \".pa\" 5;");
				data.Append("\tsetAttr \".urs\" ").Append(LightShadow).AppendLine(";");
		}
		
		// --------------------------------------------------
		// DirectionalLight
		// --------------------------------------------------
		if(Node is mDirectionalLight){
			// Get Light data
			Light LightData = Node.UnityObject.gameObject.GetComponent<Light>();

			string LightShadow = "on";
			if(LightData.shadows == LightShadows.None) LightShadow = "off";		
			
			data.Append("createNode directionalLight -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
				data.AppendLine("\tsetAttr -k off \".v\";");
				data .Append("\tsetAttr \".cl\" -type \"float3\" ").Append(LightData.color.r.ToString()).Append(" ").Append(LightData.color.g.ToString()).Append(" ").Append(LightData.color.b.ToString()).AppendLine(";");
				data.Append("\tsetAttr \".in\" ").Append(LightData.intensity).AppendLine(";");
				data.AppendLine("\tsetAttr \".de\" 1;");
				data.Append("\tsetAttr \".urs\" ").Append(LightShadow).AppendLine(";");
		}
		
		// --------------------------------------------------
		// PointLight
		// --------------------------------------------------
		if(Node is mPointLight){
			// Get Light data
			Light LightData = Node.UnityObject.gameObject.GetComponent<Light>();

			string LightShadow = "on";
			if(LightData.shadows == LightShadows.None) LightShadow = "off";		
			
			data.Append("createNode pointLight -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
				data.AppendLine("\tsetAttr -k off \".v\";");
				data .Append("\tsetAttr \".cl\" -type \"float3\" ").Append(LightData.color.r.ToString()).Append(" ").Append(LightData.color.g.ToString()).Append(" ").Append(LightData.color.b.ToString()).AppendLine(";");
				data.Append("\tsetAttr \".in\" ").Append(LightData.intensity).AppendLine(";");
				data.AppendLine("\tsetAttr \".de\" 1;");
				data.Append("\tsetAttr \".urs\" ").Append(LightShadow).AppendLine(";");
		}
		
		// --------------------------------------------------
		// AreaLight
		// --------------------------------------------------
		if(Node is mAreaLight){
			// Get Light data
			Light LightData = Node.UnityObject.gameObject.GetComponent<Light>();

			string LightShadow = "on";
			if(LightData.shadows == LightShadows.None) LightShadow = "off";		
			
			data.Append("createNode areaLight -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
				data.AppendLine("\tsetAttr -k off \".v\";");
				data .Append("\tsetAttr \".cl\" -type \"float3\" ").Append(LightData.color.r.ToString()).Append(" ").Append(LightData.color.g.ToString()).Append(" ").Append(LightData.color.b.ToString()).AppendLine(";");
				data.Append("\tsetAttr \".in\" ").Append(LightData.intensity).AppendLine(";");
				data.AppendLine("\tsetAttr \".de\" 1;");
				data.Append("\tsetAttr \".urs\" ").Append(LightShadow).AppendLine(";");
		}

		// --------------------------------------------------
		// Camera
		// --------------------------------------------------
		if(Node is mCamera){
			// Get camera properties
			Camera CamData = Node.UnityObject.gameObject.GetComponent<Camera>();
			
			// Calculate Field of View
			// Note - This is my hacky attempt at estimating Maya's field of view
			// There might be a better more mathematical way of doing this
			float FOV = CamData.fieldOfView * 0.3458333333333333f;
			
			data.Append("createNode camera -n \"").Append(Node.MayaName).Append("\" -p \"").Append(GetDAGPath(Node.Parent)).AppendLine("\";");
				data.AppendLine("\tsetAttr -k off \".v\";");
				data.AppendLine("\tsetAttr \".cap\" -type \"double2\" 1.41732 0.94488;");
				data.AppendLine("\tsetAttr \".ff\" 3;");
				data.Append("\tsetAttr \".fl\" ").Append(FOV).AppendLine(";");
				data.Append("\tsetAttr \".ncp\" ").Append(CamData.nearClipPlane).AppendLine(";");
				data.Append("\tsetAttr \".fcp\" ").Append(CamData.farClipPlane).AppendLine(";");
				data.AppendLine("\tsetAttr \".ow\" 30;");
				data.AppendLine("\tsetAttr \".imn\" -type \"string\" \"camera1\";");
				data.AppendLine("\tsetAttr \".den\" -type \"string\" \"camera1_depth\";");
				data.AppendLine("\tsetAttr \".man\" -type \"string\" \"camera1_mask\";");
		}
		
		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write Mesh (Used for Meshes & Skinned Meshes)
	// --------------------------------------------------
	// Unity categorizes Meshes and SkinnedMeshes
	// separate from each other, but we will write them out
	// to Maya the same way since they are all meshes.
	//
	// This method is a convenience method to not duplicate
	// code. Just pass in all the values of the mesh and
	// it will write it out.
	void WriteMesh(string filePath, string fileName, string MayaName, MayaNode Parent, bool IntermediateObject, Vector3[] verts, Vector2[] uvs, Vector2[] uvs2, Color[] colors, Vector3[] normals, int[] tris, string InstObjGroups, Vector4 tilingOffset, bool ExportLightmapUVs){
		StringBuilder data = new StringBuilder();
		
		// --------------------------------------------------
		// Write shape attributes
		// --------------------------------------------------
		data.Append("createNode mesh -n \"").Append(MayaName).Append("\" -p \"").Append(GetDAGPath(Parent)).AppendLine("\";");
		data.AppendLine("\tsetAttr -k off \".v\";");
		
		// --------------------------------------------------
		// Per-Face Material assignments
		// --------------------------------------------------
		data.Append(InstObjGroups);	
		
		// --------------------------------------------------
		// Intermediate Object
		// --------------------------------------------------
		// Set intermediate object flag to true (for skinned meshes)
		if(IntermediateObject) data.AppendLine("\tsetAttr \".io\" yes;");
		
		// --------------------------------------------------
		// Visible in Reflection / Refractions
		// --------------------------------------------------
		data.AppendLine("\tsetAttr \".vir\" yes;");
		data.AppendLine("\tsetAttr \".vif\" yes;");
		
		// --------------------------------------------------
		// Build UV, UV2, Color and Vertex lists
		// --------------------------------------------------
		// Note - From what I've seen, the vertex list, the UV list, the UV2 list and the colors list are
		// always the same size. So as an optimization lets just pick the vertex list as our iterator and
		// fill out all the data in 1 go to save time
		//
		// The only exception is the normals since Maya stores normals per-vertex per-face, where as the
		// other lists get referenced by the face definitions
		StringBuilder uvData = new StringBuilder();
		StringBuilder uv2Data = new StringBuilder();
		StringBuilder colorData = new StringBuilder();
		StringBuilder vertData = new StringBuilder();
		
		// Update progress bar
		EditorUtility.DisplayProgressBar(ProgressTitle, MayaName + " ( Writing UVs, Colors, Vertices )", CurrentProgress / MaxProgress);
		
		// Column counter used for nicely formatting the data in the Maya file
		// NOTE - We also use it below for the edge list as well
		int ColumnCounter = 0;

		for(int i=0; i<verts.Length; i++){
			// Format UV data
			if(uvs.Length > 0){
				if(ColumnCounter == 0) uvData.Append("\t\t");
				uvData.Append(uvs[i].x).Append(" ").Append(uvs[i].y).Append(" ");
				if(ColumnCounter == ColumnDataWidth) uvData.AppendLine();
			}
			
			// Format UV2 data
			if(uvs2.Length > 0){
				if(ColumnCounter == 0) uv2Data.Append("\t\t");
				uv2Data.Append((uvs2[i].x * tilingOffset.x) + tilingOffset.z).Append(" ").Append((uvs2[i].y * tilingOffset.y) + tilingOffset.w).Append(" ");
				if(ColumnCounter == ColumnDataWidth) uv2Data.AppendLine();
			}
			
			// Format color data
			if(colors.Length > 0){
				if(ColumnCounter == 0) colorData.Append("\t\t");
				colorData.Append(colors[i].r).Append(" ").Append(colors[i].g).Append(" ").Append(colors[i].b).Append(" ").Append(colors[i].a).Append(" ");
				if(ColumnCounter == ColumnDataWidth) colorData.AppendLine();
			}
	
			// Format vertex data
			if(ColumnCounter == 0) vertData.Append("\t\t");
			Vector3 MayaVert = MayaConvert.MayaTranslation(verts[i]);
			vertData.Append(MayaVert.x).Append(" ").Append(MayaVert.y).Append(" ").Append(MayaVert.z).Append(" ");
			if(ColumnCounter == ColumnDataWidth) vertData.AppendLine();
			
			// Increment column counter
			ColumnCounter++;
			if(ColumnCounter > ColumnDataWidth) ColumnCounter = 0;
		}

		// --------------------------------------------------
		// Write UV data
		// --------------------------------------------------
		if(uvs.Length > 0){
			data.AppendLine("\tsetAttr \".uvst[0].uvsn\" -type \"string\" \"map1\";");
			data.Append("\tsetAttr -s ").Append(verts.Length).Append(" \".uvst[0].uvsp[").Append(verts.Length > 1 ? "0:" : "").Append(verts.Length - 1).AppendLine("]\" -type \"float2\" ");
			data.Append(uvData.ToString());
			data.Append(";").AppendLine();
			data.AppendLine("\tsetAttr  \".cuvs\" -type \"string\" \"map1\";");	// Set the current uv set to the main uv set
			uvData = null;	
		}
	
		// --------------------------------------------------
		// Write UV2 data
		// --------------------------------------------------
		if(uvs2.Length > 0){
			data.AppendLine("\tsetAttr \".uvst[1].uvsn\" -type \"string\" \"lightmap\";");
			data.Append("\tsetAttr -s ").Append(verts.Length).Append(" \".uvst[1].uvsp[").Append(verts.Length > 1 ? "0:" : "").Append(verts.Length - 1).AppendLine("]\" -type \"float2\" ");
			data.Append(uv2Data.ToString());
			data.Append(";").AppendLine();
			uv2Data = null;
		}
			
		// --------------------------------------------------
		// Write Color data ( RGBA )
		// --------------------------------------------------
		if(colors.Length > 0){
			// Display vertex colors on file load
			data.AppendLine("\tsetAttr \".dcol\" yes;");
			// Set which color channel to display (Ambient + Diffuse)
			data.AppendLine("\tsetAttr \".dcc\" -type \"string\" \"Ambient+Diffuse\";");
			// Set the current color set
			data.AppendLine("\tsetAttr \".ccls\" -type \"string\" \"colorSet1\";");
			data.AppendLine("\tsetAttr \".clst[0].clsn\" -type \"string\" \"colorSet1\";");
			data.Append("\tsetAttr -s ").Append(verts.Length).Append(" \".clst[0].clsp[").Append(verts.Length > 1 ? "0:" : "").Append(verts.Length - 1).AppendLine("]\" ");
			data.Append(colorData.ToString());
			data.Append(";").AppendLine();
			colorData = null;
		}

		// --------------------------------------------------
		// Write Vertex data
		// --------------------------------------------------
		data.Append("\tsetAttr -s ").Append(verts.Length).Append(" \".vt[").Append(verts.Length > 1 ? "0:" : "").Append(verts.Length - 1).AppendLine("]\" ");
		data.Append(vertData.ToString());
		data.Append(";").AppendLine();
		vertData = null;
		
		// --------------------------------------------------
		// Edge Connections
		// --------------------------------------------------
		// Since Unity stores all polygons as triangles, this will be easy.
		// We will have 3 possible edge connections per face:
		// edgeA:(vert0->vert1) edgeB:(vert1->vert2) edgeC:(vert2->vert0)
		// Note - We have to check that the edge doesn't already exist in our
		// local list before storing the edge. No duplicates allowed!
		
		// Local edge list
		List<MayaEdge> EdgeList = new List<MayaEdge>();
		
		// Edge Index list:
		// We will fill this guy out as we go so its ready when writing the
		// polygon face data. This will be a list of indices that point to our
		// local edge list which describe the edges of a face
		List<int> EdgeIndexList = new List<int>();
		
		// Update progress bar
		EditorUtility.DisplayProgressBar(ProgressTitle, MayaName + " ( Writing Edge List )", CurrentProgress / MaxProgress);

		// NEW METHOD - Much much faster than the last method
		// Uses EdgeListCache, which is a copied version of the edge list
		// where the edges are formatted into a string "edge1_edge2". Using this
		// cached list at the same time as the edge list, we now perform the search
		// using List BinarySearch
		List<string> EdgeListCache = new List<string>();			// Copy list of edges as strings that we BinarySearch on
		StringBuilder EdgeKey = new StringBuilder();				// Used for fast building of strings
		int EdgeIndexNormal;										// Normal direction of edge indices (v1-v2) for edge check
		int EdgeIndexReversed;										// Reversed direction of edge indices (v2-v1) for edge check
		for(int v=0; v<tris.Length; v+=3){
			// --------------------------------------------------
			// Edge A
			// --------------------------------------------------
			// Check if edge exists with normal direction
			EdgeKey.Append(tris[v].ToString()).Append("_").Append(tris[v+1].ToString());
			EdgeIndexNormal = EdgeListCache.BinarySearch(EdgeKey.ToString());
			EdgeKey.Length = 0;
			// Check if edge exists with reversed direction
			EdgeKey.Append(tris[v+1].ToString()).Append("_").Append(tris[v].ToString());
			EdgeIndexReversed = EdgeListCache.BinarySearch(EdgeKey.ToString());
			EdgeKey.Length = 0;
			
			// If edge was not found, add edge, edge index and cached edge to lists
			if(EdgeIndexNormal < 0 && EdgeIndexReversed < 0){
				EdgeList.Add(new MayaEdge(tris[v], tris[v+1]));
				EdgeIndexList.Add(EdgeList.Count - 1);
				EdgeKey.Append(tris[v].ToString()).Append("_").Append(tris[v+1].ToString());
				EdgeListCache.Add(EdgeKey.ToString());
				EdgeKey.Length = 0;
			}
			// If we found the edge in normal direction, add index to EdgeIndexList
			if(EdgeIndexNormal > -1) EdgeIndexList.Add(EdgeIndexNormal);
			// If we found the edge in reversed direction, add index to EdgeIndexList
			if(EdgeIndexReversed > -1) EdgeIndexList.Add(EdgeIndexReversed);
			
			// --------------------------------------------------
			// Edge B
			// --------------------------------------------------
			// Check if edge exists with normal direction
			EdgeKey.Append(tris[v+1].ToString()).Append("_").Append(tris[v+2].ToString());
			EdgeIndexNormal = EdgeListCache.BinarySearch(EdgeKey.ToString());
			EdgeKey.Length = 0;
			// Check if edge exists with reversed direction
			EdgeKey.Append(tris[v+2].ToString()).Append("_").Append(tris[v+1].ToString());
			EdgeIndexReversed = EdgeListCache.BinarySearch(EdgeKey.ToString());
			EdgeKey.Length = 0;
			
			// If edge was not found, add edge, edge index and cached edge to lists
			if(EdgeIndexNormal < 0 && EdgeIndexReversed < 0){
				EdgeList.Add(new MayaEdge(tris[v+1], tris[v+2]));
				EdgeIndexList.Add(EdgeList.Count - 1);
				EdgeKey.Append(tris[v+1].ToString()).Append("_").Append(tris[v+2].ToString());
				EdgeListCache.Add(EdgeKey.ToString());
				EdgeKey.Length = 0;
			}
			// If we found the edge in normal direction, add index to EdgeIndexList
			if(EdgeIndexNormal > -1) EdgeIndexList.Add(EdgeIndexNormal);
			// If we found the edge in reversed direction, add index to EdgeIndexList
			if(EdgeIndexReversed > -1) EdgeIndexList.Add(EdgeIndexReversed);
			
			// --------------------------------------------------
			// Edge C
			// --------------------------------------------------
			// Check if edge exists with normal direction
			EdgeKey.Append(tris[v+2].ToString()).Append("_").Append(tris[v].ToString());
			EdgeIndexNormal = EdgeListCache.BinarySearch(EdgeKey.ToString());
			EdgeKey.Length = 0;
			// Check if edge exists with reversed direction
			EdgeKey.Append(tris[v].ToString()).Append("_").Append(tris[v+2].ToString());
			EdgeIndexReversed = EdgeListCache.BinarySearch(EdgeKey.ToString());
			EdgeKey.Length = 0;
			
			// If edge was not found, add edge, edge index and cached edge to lists
			if(EdgeIndexNormal < 0 && EdgeIndexReversed < 0){
				EdgeList.Add(new MayaEdge(tris[v+2], tris[v]));
				EdgeIndexList.Add(EdgeList.Count - 1);
				EdgeKey.Append(tris[v+2].ToString()).Append("_").Append(tris[v].ToString());
				EdgeListCache.Add(EdgeKey.ToString());
				EdgeKey.Length = 0;
			}
			// If we found the edge in normal direction, add index to EdgeIndexList
			if(EdgeIndexNormal > -1) EdgeIndexList.Add(EdgeIndexNormal);
			// If we found the edge in reversed direction, add index to EdgeIndexList
			if(EdgeIndexReversed > -1) EdgeIndexList.Add(EdgeIndexReversed);
		}

		// --------------------------------------------------
		// Combine edges into single string
		// --------------------------------------------------
		StringBuilder EdgesStr = new StringBuilder();
		ColumnCounter = 0;
		for(int i=0; i<EdgeList.Count; i++){
			if(ColumnCounter == 0) EdgesStr.Append("\t\t");
			EdgesStr.Append(EdgeList[i].StartEdge).Append(" ").Append(EdgeList[i].EndEdge).Append(" 0 ");
			if(ColumnCounter == ColumnDataWidth) EdgesStr.AppendLine();
						
			// Increment column counter
			ColumnCounter++;
			if(ColumnCounter > ColumnDataWidth) ColumnCounter = 0;
		}
		
		// Write data to file
		data.Append("\tsetAttr -s ").Append(EdgeList.Count).Append(" \".ed[").Append(EdgeList.Count > 1 ? "0:" : "").Append(EdgeList.Count - 1).AppendLine("]\" ");
		data.Append(EdgesStr.ToString());
		data.Append(";").AppendLine();
		EdgesStr = null;
		
		// --------------------------------------------------
		// Normals
		// --------------------------------------------------
		// The way Unity specifies the normals list is 1 normal per vertex
		// entry. But we need normals per-vertex per-face. So we go through the 
		// triangles list and find what vertices make up the face. We then use 
		// that to index into the normals array to find the normals per face
		
		// Update progress bar
		EditorUtility.DisplayProgressBar(ProgressTitle, MayaName + " ( Writing Normals )", CurrentProgress / MaxProgress);

		StringBuilder NormalsStr = new StringBuilder();
		for(int v=0; v<tris.Length; v+=3){
			// Get the normals and convert them into Maya translation
			Vector3 normalA = MayaConvert.MayaTranslation(normals[tris[v]]);
			Vector3 normalB = MayaConvert.MayaTranslation(normals[tris[v+1]]);
			Vector3 normalC = MayaConvert.MayaTranslation(normals[tris[v+2]]);
		
			// Not sure why this works, but we have to flip normal C and normal B for them to match
			// correctly in Maya. Weird
			NormalsStr.Append("\t\t").Append(normalA.x).Append(" ").Append(normalA.y).Append(" ").Append(normalA.z).Append(" ");
			NormalsStr.Append(normalC.x).Append(" ").Append(normalC.y).Append(" ").Append(normalC.z).Append(" ");
			NormalsStr.Append(normalB.x).Append(" ").Append(normalB.y).Append(" ").Append(normalB.z);
			
			// Add a return character if not the last entry into the normals list,
			// this way the last one has the semicolon next to the number instead of the next line
			if(v+3 < tris.Length - 1) NormalsStr.AppendLine();
		}
		
		// Write data to file
		data.Append("\tsetAttr -s ").Append(tris.Length).Append(" \".n[0:").Append(tris.Length - 1).AppendLine("]\" -type \"float3\"");
		data.Append(NormalsStr.ToString());
		data.Append(";").AppendLine();
		NormalsStr = null;

		// --------------------------------------------------
		// Faces
		// --------------------------------------------------
		// Now we need to tell Maya which edges make up each face. In Unity you can
		// specify the faces simply by giving 3 indexes into the vertex array, and it will build the 
		// face from that, but Maya is different. Maya needs edges specified in order to define a face.
		// So for each triangle go through the edges list and find the corresponding edges that
		// match the triangle vertices
		
		// Update progress bar
		EditorUtility.DisplayProgressBar(ProgressTitle, MayaName + " ( Writing Faces )", CurrentProgress / MaxProgress);
		
		// --------------------------------------------------
		// 1 or more faces check!
		// --------------------------------------------------
		// For an object that has more than 1 face, Maya will list the range
		// of faces like so: [0:N] 
		// BUT! if there are objects with just 1 face, Maya will list the range
		// like so: [0]
		// So do a check here and make sure the correct format is used
		string faceFormat = ""; if((tris.Length / 3) > 1) faceFormat = "0:";
		data.Append("\tsetAttr -s ").Append(tris.Length / 3).Append(" \".fc[").Append(faceFormat).Append((tris.Length / 3) - 1).AppendLine("]\" -type \"polyFaces\"");
		for(int i=0; i<tris.Length; i+=3){
			// --------------------------------------------------
			// Record the polygon face-edge data
			// --------------------------------------------------
			// NOTE! We reverse the order of the edge indices, from ABC to CBA
			// because Maya uses a counter-clockwise winding order for faces and Unity
			// gives us the data in clockwise winding order
			data.Append("\t\tf 3 ").Append(EdgeIndexList[i+2]).Append(" ").Append(EdgeIndexList[i+1]).Append(" ").Append(EdgeIndexList[i]);

			// Record the main UV data per face, if it exists and is requested.
			// Note - We don't completely reverse the order, but swap the second and 
			// last values so it displays correctly in Maya
			if(uvs.Length > 0){
				data.AppendLine();
				data.Append("\t\tmu 0 3 ").Append(tris[i]).Append(" ").Append(tris[i+2]).Append(" ").Append(tris[i+1]);
			}
			
			// Record the lightmap UV data per face, if it exists and is requested.
			// Same swapping mechanism as the main UV data
			if(uvs2.Length > 0){
				data.AppendLine();
				data.Append("\t\tmu 1 3 ").Append(tris[i]).Append(" ").Append(tris[i+2]).Append(" ").Append(tris[i+1]);
			}
		
			// Record vertex color per face, if it exists and is requested
			if(colors.Length > 0){
				data.AppendLine();
				data.Append("\t\tmc 0 3 ").Append(tris[i]).Append(" ").Append(tris[i+2]).Append(" ").Append(tris[i+1]);
			}

			data.AppendLine();
		}
		
		// Add trailing semicolon after face setup
		data.Append(";").AppendLine();

		// Write to file
		AppendToFile(filePath, fileName, data);
		data = null;
	}
	
	// --------------------------------------------------
	// Write Connections Section
	// --------------------------------------------------
	void WriteConnections(string filePath, string fileName){
		// Write data to file
		AppendToFile(filePath, fileName, Connections.ToString());
		
		Connections = null;
	}
	
	// --------------------------------------------------
	// Write to File - Append Data
	// --------------------------------------------------
	// When writing to a file, this will append data to the file
	public static void AppendToFile(string filePath, string fileName, string s){
		using (StreamWriter writer = new StreamWriter(filePath + fileName, true)){
			writer.Write(s);
		}
	}
	public static void AppendToFile(string filePath, string fileName, StringBuilder s){
		using(StreamWriter writer = new StreamWriter(filePath + fileName, true)){
			writer.Write(s.ToString());
		}
	}
	#endregion
		
	#region Texture Copying
	// --------------------------------------------------
	// Copy Textures
	// --------------------------------------------------
	// Go through our MaterialNodes and copy the textures from
	// our Unity project to the destination path
	void CopyTextures(string filePath, string fileName){
		for(int i=0; i<MaterialNodes.Count; i++){
			EditorUtility.DisplayProgressBar(ProgressTitle, "Copying Texture " + GetTextureNameExt(MaterialNodes[i].UnityTexture), CurrentProgress / MaxProgress);
			
			if(MaterialNodes[i] is mFile){
				// --------------------------------------------------
				// If this MayaNode is a texture
				// --------------------------------------------------
				// Build Paths
				string sourcePath = GetFullTexturePath(MaterialNodes[i].UnityTexture);
				string destPath = filePath + GetTextureNameExt(MaterialNodes[i].UnityTexture);

				// If the file doesn't already exist in the destination path
				if(!System.IO.File.Exists(destPath)){
					// Copy it
					FileUtil.CopyFileOrDirectory(sourcePath, destPath);
				}
			}
			
			if(MaterialNodes[i] is mBlinn){
				if(MaterialNodes[i].MaterialType == MayaMaterialType.TerrainMaterial){
					// --------------------------------------------------
					// Terrain Material
					// --------------------------------------------------
					// Since we have to generate the Terrain Textures, we check
					// for terrain materials instead of just textures

					// Get the Terrain Alpha Map(s)
					Texture2D[] AlphaMaps = MaterialNodes[i].UnityTerrainData.alphamapTextures;
					
					// Counter used for numbering the splatmaps
					int AlphaMapCounter = 0;
					
					// Go through every AlphaMap and separate each RGBA channel into a new texture
					for(int am=0; am<AlphaMaps.Length; am++){
						// Byte array to hold channel pixels
						byte[] bytes;
						
						// Create separate textures for each channel
						Texture2D RedTex = new Texture2D(AlphaMaps[am].width, AlphaMaps[am].height);
						Texture2D GreenTex = new Texture2D(AlphaMaps[am].width, AlphaMaps[am].height);
						Texture2D BlueTex = new Texture2D(AlphaMaps[am].width, AlphaMaps[am].height);
						Texture2D AlphaTex = new Texture2D(AlphaMaps[am].width, AlphaMaps[am].height);
						
						// Pixel arrays
						Color[] AlphaMapPixels = AlphaMaps[am].GetPixels(0);
						Color[] RedPixels = new Color[AlphaMapPixels.Length];
						Color[] GreenPixels = new Color[AlphaMapPixels.Length];
						Color[] BluePixels = new Color[AlphaMapPixels.Length];
						Color[] AlphaPixels = new Color[AlphaMapPixels.Length];
						
						// Fill out pixel arrays
						for(int p=0; p<AlphaMapPixels.Length; p++){
							RedPixels[p] = new Color(AlphaMapPixels[p].r, AlphaMapPixels[p].r, AlphaMapPixels[p].r, 1);
							GreenPixels[p] = new Color(AlphaMapPixels[p].g, AlphaMapPixels[p].g, AlphaMapPixels[p].g, 1);
							BluePixels[p] = new Color(AlphaMapPixels[p].b, AlphaMapPixels[p].b, AlphaMapPixels[p].b, 1);
							AlphaPixels[p] = new Color(AlphaMapPixels[p].a, AlphaMapPixels[p].a, AlphaMapPixels[p].a, 1);
						}
						
						// Set the new pixels to the Textures
						RedTex.SetPixels(RedPixels, 0);
						GreenTex.SetPixels(GreenPixels, 0);
						BlueTex.SetPixels(BluePixels, 0);
						AlphaTex.SetPixels(AlphaPixels, 0);
						
						// Write the textures out
						bytes = RedTex.EncodeToPNG();
						File.WriteAllBytes(filePath + MaterialNodes[i].UnityTerrainData.name + "_splatmap_" + (AlphaMapCounter) + ".png", bytes);
						bytes = GreenTex.EncodeToPNG();
						File.WriteAllBytes(filePath + MaterialNodes[i].UnityTerrainData.name + "_splatmap_" + (AlphaMapCounter+1) + ".png", bytes);
						bytes = BlueTex.EncodeToPNG();
						File.WriteAllBytes(filePath + MaterialNodes[i].UnityTerrainData.name + "_splatmap_" + (AlphaMapCounter+2) + ".png", bytes);
						bytes = AlphaTex.EncodeToPNG();
						File.WriteAllBytes(filePath + MaterialNodes[i].UnityTerrainData.name + "_splatmap_" + (AlphaMapCounter+3) + ".png", bytes);
						
						// Update AlphaMapCounter
						AlphaMapCounter += 4;
					}
				}
			}
			
			CurrentProgress++;
		}
	}
	#endregion	

	#region Maya Names
	// --------------------------------------------------
	// Get MayaNode DAG Path
	// --------------------------------------------------
	string GetDAGPath(MayaNode n){
		string dagpath = "|" + n.MayaName;
		bool DoIt = true;
		MayaNode refNode = n;
		while(DoIt){
			if(refNode.Parent == null) DoIt = false;
			else{
				refNode = refNode.Parent;
				dagpath = "|" + refNode.MayaName + dagpath;
			}
		}
		return dagpath;
	}
	
	// --------------------------------------------------
	// Clean Name
	// --------------------------------------------------
	// Given a string, this will remove illegal characters so
	// it fits within Maya naming conventions
	string CleanName(string name){
		name = name.Replace(" ", "_");
		name = name.Replace("-", "_");
		Regex rgx = new Regex("[^a-zA-Z0-9_]");
		return rgx.Replace(name, "");
	}
	
	// --------------------------------------------------
	// Renames DAG MayaNodes to conform to Maya standards
	// --------------------------------------------------
	// For DAGNodes, we use the full DAG path to rename duplicate named
	// nodes. For all others we just use the node name with no DAG path
	void RenameDAGNodes(){
		for(int i=0; i<DAGNodes.Count; i++){
			// Build DAG path
			string DAGPath = GetDAGPath(DAGNodes[i]);

			// --------------------------------------------------
			// Register DAGName
			// --------------------------------------------------
			// See if it has been registered yet
			if(DAGNames.ContainsKey(DAGPath)){
				DAGNames[DAGPath]++;
				DAGNodes[i].MayaName +=  "_" + DAGNames[DAGPath];
			}
			// If it hasn't, register it
			else{
				DAGNames.Add(DAGPath, 0);
			}
			
			// --------------------------------------------------
			// Also register just the name
			// --------------------------------------------------
			// We do this so when renaming AUXNodes and Material nodes
			// they don't clash with the DAGNodes
			if(GlobalNames.ContainsKey(DAGNodes[i].MayaName)){
				GlobalNames[DAGNodes[i].MayaName]++;
			}
			else{
				GlobalNames.Add(DAGNodes[i].MayaName, 0);
			}
		}
	}
	
	// --------------------------------------------------
	// Returns a unique numbered global name based on string
	// --------------------------------------------------
	string GetUniqueGlobalNodeName(string name){
		// First clean the name
		name = CleanName(name);
		
		string NodeNameNum = "";
		
		// Next see if it has been registered yet
		if(GlobalNames.ContainsKey(name)){
			GlobalNames[name]++;
			NodeNameNum = GlobalNames[name].ToString();
		}
		// If it hasn't, register it
		else{
			GlobalNames.Add(name, 0);
		}
		
		return name + NodeNameNum;
	}
	#endregion
	
	#region Memory Cleanup
	// --------------------------------------------------
	// Clear Variables - Free Memory
	// --------------------------------------------------
	void ResetVariables(){
		DAGNodes = new List<MayaNode>();
		DAGNodesBuffer = new List<MayaNode>();
		AUXNodes = new List<MayaNode>();
		Connections = new StringBuilder();
		MaterialNodes = new List<MayaNode>();
		DisplayLayerList = new List<mDisplayLayer>();
		
		DAGNames = new Dictionary<string, int>();
		GlobalNames = new Dictionary<string, int>();
		// Pre-Register known names
		// If we don't do this, then new versions of these
		// nodes wont get created and the exporter will use
		// the default ones in the Maya scene
		GlobalNames.Add("persp", 0);
		GlobalNames.Add("top", 0);
		GlobalNames.Add("front", 0);
		GlobalNames.Add("side", 0);
		GlobalNames.Add("lambert1", 0);
		GlobalNames.Add("particleCloud1", 0);
		GlobalNames.Add("shaderGlow1", 0);
		
		ProgressTitle = "Exporting Maya Ascii File";
		CurrentProgress = 0.0f;
		MaxProgress = 0.0f;
	}
	#endregion
}