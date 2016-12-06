using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GBSettings))]
public class GBSettingsEditor : Editor {
	
	private bool m_AdvancedMenuVisible = false;
	
	public override void OnInspectorGUI ()
	{	
		Color myCol = Color.green; //Color myCol = new Color(0.8f,0.60f,1.3f,1.0f);
		GUILayout.Space(10);
		GUI.color = myCol;
		GUILayout.Label ("Geometry Brush Settings","BoldLabel");
		GUI.color = Color.white;
		GUILayout.Space(4);
		GBSettings gbSettingsScript;
		if ( GBUtils.GetGBSettingsScript( out gbSettingsScript ) == false ) return;
		//gbSettingsScript.parentObject = (GameObject)EditorGUILayout.ObjectField( "Parent object", gbSettingsScript.parentObject, typeof(GameObject), true );
		//GUILayout.Space(4);
		if ( GUILayout.Button ( "Open Geometry Brush Manager", GUILayout.Height (36) ) ){
			GBWindow.OpenWindow();	
		}
		EditorGUILayout.HelpBox( "To enable or disable the tool, open up the geometry brush window.", MessageType.Info );
		GUILayout.Space(10);
		
		m_AdvancedMenuVisible = EditorGUILayout.BeginToggleGroup("Advanced Options", m_AdvancedMenuVisible);
		EditorGUILayout.HelpBox( "The following fields control the possible min-max range of the values in the geometry brush window.", MessageType.Info );
		
		gbSettingsScript.minBrushSize = EditorGUILayout.FloatField( "Min Brush Size", gbSettingsScript.minBrushSize );
		gbSettingsScript.maxBrushSize = EditorGUILayout.FloatField( "Max Brush Size", gbSettingsScript.maxBrushSize );
		
		gbSettingsScript.minMinScale = EditorGUILayout.FloatField( "Lowest Min Scale", gbSettingsScript.minMinScale );
		gbSettingsScript.maxMinScale = EditorGUILayout.FloatField( "Highest Min Scale", gbSettingsScript.maxMinScale );
		
		gbSettingsScript.minMaxScale = EditorGUILayout.FloatField( "Lowest Max Scale", gbSettingsScript.minMaxScale );
		gbSettingsScript.maxMaxScale = EditorGUILayout.FloatField( "Highest Max Scale", gbSettingsScript.maxMaxScale );
		
		gbSettingsScript.minYOffset = EditorGUILayout.FloatField( "Min Y Offset", gbSettingsScript.minYOffset );
		gbSettingsScript.maxYOffset = EditorGUILayout.FloatField( "Max Y Offset", gbSettingsScript.maxYOffset );
		
		gbSettingsScript.minSpacing = EditorGUILayout.FloatField( "Min Spacing", gbSettingsScript.minSpacing );
		gbSettingsScript.maxSpacing = EditorGUILayout.FloatField( "Max Spacing", gbSettingsScript.maxSpacing );
		
		EditorGUILayout.EndToggleGroup();
		
		if (GUI.changed){
        	EditorUtility.SetDirty (gbSettingsScript);
		}
	}
}