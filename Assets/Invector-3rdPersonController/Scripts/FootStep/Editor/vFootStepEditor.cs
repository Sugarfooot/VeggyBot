﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(vFootStepFromTexture),true)]
public class vFootStepEditor : Editor 
{
    GUISkin skin;
    public override void OnInspectorGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;
        
        if (serializedObject == null) return;
        
		EditorGUILayout.Space();
		GUILayout.BeginVertical ("FootStep System by Invector", "window");
       
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
                
		EditorGUILayout.Separator ();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animationType"));
		serializedObject.FindProperty ("debugTextureName").boolValue = EditorGUILayout.Toggle("Debug Texture Name", serializedObject.FindProperty("debugTextureName").boolValue);
		EditorGUILayout.Separator ();		
	
        if(serializedObject.FindProperty("animationType").enumValueIndex == (int) AnimationType.Humanoid)
        {
            GUILayout.BeginHorizontal("box");
            if (CheckColliders())
            {               
                EditorGUILayout.PropertyField(serializedObject.FindProperty("leftFootTrigger"), new GUIContent("", null, "leftFootTrigger"));
                EditorGUILayout.Separator();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rightFootTrigger"), new GUIContent("", null, "rightFootTrigger"));
            }
            else
            {
                EditorGUILayout.HelpBox("Can't Create FootStepTriggers", MessageType.Warning);
                CheckColliders();
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            DrawFootStepList();
        }
	     
        GUILayout.BeginVertical("Default FootStep Audio","window");
        GUILayout.Space(40);       
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultSurface"));     
        EditorGUILayout.HelpBox("This audio will play on any terrain or texture as the primary footstep.", MessageType.Info);
        GUILayout.EndVertical();

        GUILayout.BeginVertical("CustomSurfaces","window");
        GUILayout.Space(20);
        DrawMultipleSurface(serializedObject.FindProperty("customSurfaces"));
        EditorGUILayout.HelpBox("Create new CustomSurfaces on the 3rd Person Controller menu > Resources > New AudioSurface", MessageType.Info);
        GUILayout.EndVertical();

        GUILayout.EndVertical();
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
		EditorGUILayout.Space();
    }
    
    [MenuItem("3rd Person Controller/Component/FootStep")]
    static void MenuComponent()
    {
        if (Selection.activeGameObject)
            Selection.activeGameObject.AddComponent<vFootStepFromTexture>();
        else
            Debug.Log("Please select a GameObject to add the component.");
    }

    [MenuItem("3rd Person Controller/Resources/New AudioSurface")]
    static void NewAudioSurface()
    {
        vScriptableObjectUtility.CreateAsset<vAudioSurface>();
    }
    [InitializeOnLoadMethod]

    bool CheckColliders()
    {
        if (Selection.activeGameObject != null && PrefabUtility.GetPrefabType(Selection.activeGameObject) == PrefabType.Prefab || !Selection.activeGameObject.activeSelf)
            return true;

        //var _footStep = (FootStepFromTexture)target;
        var transform = (serializedObject.targetObject as vFootStepFromTexture).transform;
        if (transform == null) return false; 
        var animator = transform.GetComponent<Animator>();        
        var leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        vFootStepTrigger leftFoot_trigger = null;
        if (leftFoot != null)
            leftFoot_trigger = leftFoot.GetComponentInChildren<vFootStepTrigger>();
       
        if (leftFoot_trigger == null && leftFoot != null)
        {
            var lFoot = new GameObject("leftFoot_trigger");
            lFoot.tag = "Weapon";
            var collider = lFoot.AddComponent<SphereCollider>();
            collider.radius = 0.1f;
            leftFoot_trigger = lFoot.AddComponent<vFootStepTrigger>();
            leftFoot_trigger.transform.position = new Vector3(leftFoot.position.x,transform.position.y,leftFoot.position.z);
            leftFoot_trigger.transform.rotation = transform.rotation;
            leftFoot_trigger.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            leftFoot_trigger.transform.parent = leftFoot;          
            serializedObject.FindProperty("leftFootTrigger").objectReferenceValue = leftFoot_trigger;
            serializedObject.ApplyModifiedProperties();
        }
        serializedObject.FindProperty("leftFootTrigger").objectReferenceValue = leftFoot_trigger;

        if (leftFoot_trigger != null && leftFoot_trigger.GetComponent<Collider>() == null)
        {
           var collider = leftFoot_trigger.gameObject.AddComponent<SphereCollider>();
           collider.radius = 0.1f;           
        }            

        var rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        vFootStepTrigger rightFoot_trigger = null;
        if(rightFoot != null)
            rightFoot_trigger = rightFoot.GetComponentInChildren<vFootStepTrigger>();

        if (rightFoot_trigger == null && rightFoot != null)
        {
            var rFoot = new GameObject("rightFoot_trigger");
            rFoot.tag = "Weapon";
            var collider = rFoot.AddComponent<SphereCollider>();
            collider.radius = 0.1f;
            rightFoot_trigger = rFoot.gameObject.AddComponent<vFootStepTrigger>();
            rightFoot_trigger.transform.position = new Vector3(rightFoot.position.x, transform.position.y, rightFoot.position.z);
            rightFoot_trigger.transform.rotation = transform.rotation;
            rightFoot_trigger.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            rightFoot_trigger.transform.parent = rightFoot;
            serializedObject.FindProperty("rightFootTrigger").objectReferenceValue = rightFoot_trigger;
            serializedObject.ApplyModifiedProperties();
        }
        serializedObject.FindProperty("rightFootTrigger").objectReferenceValue = rightFoot_trigger;
        if (rightFoot_trigger != null &&  rightFoot_trigger.GetComponent<Collider>() == null)
        {
          var collider = rightFoot_trigger.gameObject.AddComponent<SphereCollider>();
          collider.radius = 0.1f;          
        }
        
        if (serializedObject.FindProperty("rightFootTrigger").objectReferenceValue != null && serializedObject.FindProperty("leftFootTrigger").objectReferenceValue != null) return true;
        return false;
    }

    void DrawFootStepList()
    {
        var footStepList = serializedObject.FindProperty("footStepTriggers");
        if(footStepList!=null)
        {
            GUILayout.BeginVertical("Triggers","window");
            GUILayout.Space(30);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("New", EditorStyles.miniButton))
            {
                footStepList.arraySize++;
                var go = new GameObject("Trigger-"+footStepList.arraySize.ToString("00"), typeof(vFootStepTrigger),typeof(SphereCollider));
                go.GetComponent<SphereCollider>().radius = 0.05f;
                go.transform.position = (target as vFootStepFromTexture).transform.position;
                go.layer = LayerMask.NameToLayer("Ignore Raycast");
                go.transform.parent = (target as vFootStepFromTexture).transform;
                
                footStepList.GetArrayElementAtIndex(footStepList.arraySize - 1).objectReferenceValue = go.GetComponent<vFootStepTrigger>();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            for(int i=0;i<footStepList.arraySize; i++)
            {
                if (!DrawFootStepElement(footStepList, footStepList.GetArrayElementAtIndex(i), i)) break;
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }

    bool DrawFootStepElement(SerializedProperty list,SerializedProperty footStepElement,int index)
    {
        GUILayout.BeginHorizontal("box");
        EditorGUILayout.PropertyField(footStepElement,new GUIContent(""));
       if( GUILayout.Button("-",EditorStyles.miniButtonMid,GUILayout.MaxWidth(15)))
        {
            if((footStepElement.objectReferenceValue as vFootStepTrigger)!=null)
            {
                DestroyImmediate((footStepElement.objectReferenceValue as vFootStepTrigger).gameObject);
                list.DeleteArrayElementAtIndex(index);
            }                
            
            list.DeleteArrayElementAtIndex(index);
            GUILayout.EndHorizontal();
            return false;
        }
        GUILayout.EndHorizontal();
        return true; 
    }

    void DrawSingleSurface(SerializedProperty surface,bool showListNames)
    {
        //GUILayout.BeginVertical("window");
        EditorGUILayout.PropertyField(surface.FindPropertyRelative("source"), false);
		EditorGUILayout.PropertyField(surface.FindPropertyRelative("name"), new GUIContent("Surface Name"), false);

        if (showListNames)
            DrawSimpleList(surface.FindPropertyRelative("TextureOrMaterialNames"),false);

        DrawSimpleList(surface.FindPropertyRelative("audioClips"),true);
        //GUILayout.EndVertical();
    }

    void DrawMultipleSurface(SerializedProperty surfaceList)
    {
        //GUILayout.BeginVertical();
        EditorGUILayout.PropertyField(surfaceList,new GUIContent("Surfaces"));
        if(surfaceList.isExpanded)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                surfaceList.arraySize++;
            }
            if (GUILayout.Button("Clear"))
            {
                surfaceList.arraySize = 0;
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            for (int i = 0; i < surfaceList.arraySize; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginHorizontal("box");
                EditorGUILayout.Space();
                if (i < surfaceList.arraySize && i >= 0)
                {
                    GUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(surfaceList.GetArrayElementAtIndex(i),
                        new GUIContent(surfaceList.GetArrayElementAtIndex(i).objectReferenceValue != null ? surfaceList.GetArrayElementAtIndex(i).objectReferenceValue.name : "Surface " + (i + 1).ToString("00")));                   
                    EditorGUILayout.Space();
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("-"))
                {
                    surfaceList.DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();
            }
            //GUILayout.EndVertical();
        }           
    }

    void DrawTextureNames(SerializedProperty textureNames)
    {
        for (int i = 0; i < textureNames.arraySize; i++)        
            EditorGUILayout.PropertyField(textureNames.GetArrayElementAtIndex(i), true);      
    }

    void DrawSimpleList(SerializedProperty list,bool useDraBox)
    {
        EditorGUILayout.PropertyField(list);

        if (list.isExpanded)
        {
            if (useDraBox)
                DrawDragBox(list);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                list.arraySize++;
            }
            if (GUILayout.Button("Clear"))
            {
                list.arraySize=0;
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            
            for (int i = 0; i < list.arraySize; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-"))
                {
                    RemoveElementAtIndex(list, i);
                }

                if (i < list.arraySize && i >= 0)
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent("", null, ""));
                
                GUILayout.EndHorizontal();
            }
        }       
    }

    private void RemoveElementAtIndex(SerializedProperty array, int index)
    {
        if (index != array.arraySize - 1)
        {
            array.GetArrayElementAtIndex(index).objectReferenceValue = array.GetArrayElementAtIndex(array.arraySize - 1).objectReferenceValue;
        }
        array.arraySize--;
    }

    void DrawDragBox(SerializedProperty list)
    {
        //var dragAreaGroup = GUILayoutUtility.GetRect(0f, 35f, GUILayout.ExpandWidth(true));
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.skin.box.normal.textColor = Color.white;                
        GUILayout.Box("Drag your audio clips here!", "box", GUILayout.MinHeight(50), GUILayout.ExpandWidth(true));
        var dragAreaGroup = GUILayoutUtility.GetLastRect();

        switch (Event.current.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dragAreaGroup.Contains(Event.current.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (Event.current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (var dragged in DragAndDrop.objectReferences)
                    {
                        var clip = dragged as AudioClip;                       
                        if (clip == null)
                            continue;
                        list.arraySize++;
                        list.GetArrayElementAtIndex(list.arraySize - 1).objectReferenceValue = clip;                                             
                    }
                }
			serializedObject.ApplyModifiedProperties();
                Event.current.Use();
                break;
        }       
    }
}
[CustomEditor(typeof(vAudioSurface),true)]
public class AudioSurfaceEditor:Editor
{
    GUISkin skin;
  
    public override void OnInspectorGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;
        
        if (serializedObject == null) return;

        GUILayout.BeginVertical("Audio Surface", "window"); 
        GUILayout.Space(40);
        
        DrawSingleSurface(serializedObject, true);        
        GUILayout.BeginVertical( "box");
        GUILayout.Box("Optional Parameter", GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSource"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("audioMixerGroup"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("particleObject"), false);

        GUILayout.EndVertical();
        GUILayout.EndVertical();        
       
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }   

    void DrawSingleSurface(SerializedObject surface, bool showListNames)
    {
        if (showListNames)
            DrawSimpleList(surface.FindProperty("TextureOrMaterialNames"), false);
        DrawSimpleList(surface.FindProperty("audioClips"), true);      
    }

    void DrawSimpleList(SerializedProperty list, bool useDraBox)
    {
        var name = list.name;
        GUILayout.BeginVertical("box");
        GUILayout.Box(name, GUILayout.ExpandWidth(true));

        switch (list.name)
        {
            case "TextureOrMaterialNames":
                name = "Texture  or  Material  names";               
                EditorGUILayout.HelpBox("Leave this field empty and assign to the defaultSurface to play on any surface or type a Material name and assign to a customSurface to play only when the sphere hit a mesh using it.", MessageType.Info);
                break;
            case "audioClips":
                EditorGUILayout.HelpBox("You can lock the inspector to drag and drop multiple audio files.", MessageType.Info);
                name = "Audio  Clips";                
                break;
               
        }
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();       
        EditorGUILayout.PropertyField(list, false);        
        //GUILayout.Box(list.arraySize.ToString("00"));       
        GUILayout.EndHorizontal();
       
        if (list.isExpanded)
        {
            if (useDraBox)
                DrawDragBox(list);
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                list.arraySize++;
            }
            if (GUILayout.Button("Clear"))
            {
                list.arraySize = 0;
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            for (int i = 0; i < list.arraySize; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-"))
                {
                    RemoveElementAtIndex(list, i);
                }

                if (i < list.arraySize && i >= 0)
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent("", null, ""));

                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndVertical(); 
    }

    private void RemoveElementAtIndex(SerializedProperty array, int index)
    {
        if (index != array.arraySize - 1)
        {
            array.GetArrayElementAtIndex(index).objectReferenceValue = array.GetArrayElementAtIndex(array.arraySize - 1).objectReferenceValue;
        }
        array.arraySize--;
    }

    void DrawDragBox(SerializedProperty list)
    {
        //var dragAreaGroup = GUILayoutUtility.GetRect(0f, 35f, GUILayout.ExpandWidth(true));
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.skin.box.normal.textColor = Color.white;
        //GUILayout.BeginVertical("window");
        GUILayout.Box("Drag your audio clips here!", "box", GUILayout.MinHeight(50), GUILayout.ExpandWidth(true));
        var dragAreaGroup = GUILayoutUtility.GetLastRect();
        //GUILayout.EndVertical();
        switch (Event.current.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dragAreaGroup.Contains(Event.current.mousePosition))
                    break;
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (Event.current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (var dragged in DragAndDrop.objectReferences)
                    {
                        var clip = dragged as AudioClip;
                        if (clip == null)
                            continue;
                        list.arraySize++;
                        list.GetArrayElementAtIndex(list.arraySize - 1).objectReferenceValue = clip;
                    }
                }
                serializedObject.ApplyModifiedProperties();
                Event.current.Use();
                break;
        }
    }
}