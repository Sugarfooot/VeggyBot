using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------
// MayaNode Class
// --------------------------------------------------
// Base class for processing Unity objects
public class MayaNode {
	public bool Shape = false;									// Whether we should export Transform Only (false) or Shape (true)
	
	public Transform UnityObject;								// The Unity Transform this MayaNode refers to
	public TerrainData UnityTerrainData;						// The Unity TerrainData this MayaNode refers to
	public Material UnityMaterial;								// The Unity Material this MayaNode refers to
	public Texture UnityTexture;								// The Unity Texture this MayaNode refers to
	
	public string MayaName;										// The Maya formatted version of this objects name
	
	public MayaMaterialType MaterialType;						// If this is a normal material or terrain material
	
	public MayaNode Parent;										// Parent of this MayaNode
	public List<MayaNode> Children = new List<MayaNode>();		// Children of this MayaNode
}

// --------------------------------------------------
// Transform Class
// --------------------------------------------------
public class mTransform : MayaNode {
	public bool LockTransform;
	
	// Sometimes it is necessary (lights for example) to
	// override what transform values Unity has with our
	// own values
	public bool UseTranslateOverride;
	public Vector3 TranslateOverride;
	public bool UseRotateOverride;
	public Vector3 RotateOverride;
	public bool UseScaleOverride;
	public Vector3 ScaleOverride;
}

// --------------------------------------------------
// Mesh Class
// --------------------------------------------------
public class mMesh : MayaNode {
	// Intermediate Object is used with Skinned Meshes
	public bool IntermediateObject = false;
}

// --------------------------------------------------
// Mesh Shell Class
// --------------------------------------------------
// This is a made up class to make it easier to export
// skinned meshes. This mesh class doesn't store any data
// itself, but gets passed all the data to it from upstream
public class mMeshShell : MayaNode {
	// Instance Object Groups Counter
	public int InstObjCounter = 0;
	
	public int GetInstObjCount(){
		int CountValue = InstObjCounter;
		InstObjCounter++;
		return CountValue;
	}
}

// --------------------------------------------------
// Blenshape Class
// --------------------------------------------------
public class mBlendShape : MayaNode {
	// What blendshape index this node refers to
	public int BlendShapeIndex;
	// What blendshape frame index this node refers to
	public int BlendShapeFrameIndex;
}

// --------------------------------------------------
// DirectionalLight Class
// --------------------------------------------------
public class mDirectionalLight : MayaNode {
}

// --------------------------------------------------
// PointLight Class
// --------------------------------------------------
public class mPointLight : MayaNode {
}

// --------------------------------------------------
// SpotLight Class
// --------------------------------------------------
public class mSpotLight : MayaNode {
}

// --------------------------------------------------
// AreaLight Class
// --------------------------------------------------
public class mAreaLight : MayaNode {
}

// --------------------------------------------------
// Joint Class
// --------------------------------------------------
public class mJoint : MayaNode {
}

// --------------------------------------------------
// SkinCluster Class
// --------------------------------------------------
public class mSkinCluster : MayaNode {
}

// --------------------------------------------------
// TweakSet Class
// --------------------------------------------------
public class mTweakSet : MayaNode {
}

// --------------------------------------------------
// MaterialInfo Class
// --------------------------------------------------
public class mMaterialInfo : MayaNode {
}

// --------------------------------------------------
// ShadingEngine Class
// --------------------------------------------------
public class mShadingEngine : MayaNode {
}

// --------------------------------------------------
// Blinn Class
// --------------------------------------------------
public class mBlinn : MayaNode {
}

// --------------------------------------------------
// Terrain Material Class
// --------------------------------------------------
public class mTerrainMaterial : MayaNode {
}

// --------------------------------------------------
// File Class
// --------------------------------------------------
public class mFile : MayaNode {
}

// --------------------------------------------------
// Bump2D Class
// --------------------------------------------------
public class mBump2d : MayaNode {
	public float BumpAmount = 1;
}

// --------------------------------------------------
// Ramp Class
// --------------------------------------------------
public class mRamp : MayaNode {
	public List<Color> Colors = new List<Color>();
}

// --------------------------------------------------
// LayeredTexture Class
// --------------------------------------------------
public class mLayeredTexture : MayaNode {
	public int NumberOfInputs = 0;
}

// --------------------------------------------------
// Place2dTexture Class
// --------------------------------------------------
public class mPlace2dTexture : MayaNode {
	// How many times the texture repeats
	public Vector2 TexTiling = new Vector2(1,1);
	// The offset of the texture
	public Vector2 TexOffset = new Vector2(0,0);
}

// --------------------------------------------------
// Tweak Class
// --------------------------------------------------
public class mTweak : MayaNode {
}

// --------------------------------------------------
// ObjectSet Class
// --------------------------------------------------
public class mObjectSet : MayaNode {
}

// --------------------------------------------------
// GroupId Class
// --------------------------------------------------
public class mGroupId : MayaNode {
}

// --------------------------------------------------
// GroupParts Class
// --------------------------------------------------
// This class is used by SkinClusters and Blendshapes
public class mGroupParts : MayaNode {
	// When used with blendshapes, this skips the submesh
	// check and forces all verts to be exported
	public bool ForceAllVerts = false;
	
	// Which submesh (index of submesh) this GroupParts
	// refers to
	public int SubMeshIndex = -1;
}

// --------------------------------------------------
// DisplayLayer Class
// --------------------------------------------------
public class mDisplayLayer : MayaNode {
	// The index of the lightmap this DisplayLayer refers to
	public int LightmapIndex = 0;
	
	// List of MayaNodes that belong to this display layer
	public List<MayaNode> Objects = new List<MayaNode>();
}

// --------------------------------------------------
// Camera Class
// --------------------------------------------------
public class mCamera : MayaNode {
}

// --------------------------------------------------
// Terrain Class
// --------------------------------------------------
// Terrains are just meshes, but we store them as a separate
// class for easier processing
public class mTerrain : MayaNode {
}

// --------------------------------------------------
// TerrainFileAlpha Class
// --------------------------------------------------
// Same thing as File, except we keep track of which
// terrain layer index this node references so we know
// which alpha map to use
public class mTerrainFileAlpha : MayaNode {
	public string ImgName;
}

public enum MayaMaterialType{
	RegularMaterial,
	TerrainMaterial
};