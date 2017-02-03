﻿#if GAIA_PRESENT && UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

namespace Gaia.GX.Dogmatic
{
    //<summary>
    //AQUAS setup for Gaia
    //</summary>
    public class AQUAS_Gaia : MonoBehaviour
    {
#region Generic informational methods
        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Dogmatic";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "AQUAS";
        }
#endregion

        //<summary>
        //Gives some info on AQUAS
        //</summary>
        public static void GX_About()
        {
            EditorUtility.DisplayDialog("About AQUAS", "AQUAS Water is a feature rich water solution for all types of platforms, environments and games.", "OK");
        }

        //<summary>
        //Adds AQUAS to the Scene
        //</summary>
        public static void GX_AddAquas() 
        {
            //Get scene info
            GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();

            //Find camera
            Camera camera = Camera.main;

            if (camera == null)
            {
                camera = FindObjectOfType<Camera>();
            }

            //Add the AQUAS prefab to the scene

            //Check if AQUAS is imported
            GameObject aquasPrefab = Gaia.Utils.GetAssetPrefab("AQUASWater");

            if (aquasPrefab == null)
            {
                EditorUtility.DisplayDialog("Unable to locate AQUAS", "Unable to locate AQUAS - Aborting", "Ok");
                return;
            }

            //Check if AQUAS is already in the scene
            if (GameObject.Find("AQUAS Waterplane") != null)
            {
                EditorUtility.DisplayDialog("Can't add water", "AQUAS is already in the scene", "Ok");
                return;
            }

            //Try to add AQUAS
            GameObject aquasObj = Instantiate(aquasPrefab);

            if (aquasObj == null)
            {
                EditorUtility.DisplayDialog("Can't add water", "Unable to create AQUAS object", "Ok");
                return;
            }

            //Position the waterplane correctly
            else
            {
                aquasObj.name = "AQUAS Waterplane";
                aquasObj.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);
                aquasObj.transform.localScale = sceneInfo.m_sceneBounds.extents*150;
                
                if(camera.gameObject.GetComponent<AQUAS_Camera>()==null)
                {
                    camera.gameObject.AddComponent<AQUAS_Camera>();
                }

                //Check if Tenkoku is in the scene
                if (GameObject.Find("Tenkoku DynamicSky") != null)
                {

                }

                else 
                {
                    GameObject refProbe = new GameObject("Reflection Probe");
                    refProbe.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);
                    refProbe.AddComponent<ReflectionProbe>();
                    refProbe.GetComponent<ReflectionProbe>().intensity = 0.3f;
                    refProbe.transform.SetParent(aquasObj.transform);
                    refProbe.GetComponent<ReflectionProbe>().mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
                }

                //Add caustics
                GameObject primaryCausticsPrefab = Gaia.Utils.GetAssetPrefab("PrimaryCausticsProjector");
                GameObject primaryCausticsObj = Instantiate(primaryCausticsPrefab);
                primaryCausticsObj.name = "PrimaryCausticsProjector";
                GameObject secondaryCausticsPrefab = Gaia.Utils.GetAssetPrefab("SecondaryCausticsProjector");
                GameObject secondaryCausticsObj = Instantiate(secondaryCausticsPrefab);
                secondaryCausticsObj.name = "SecondaryCausticsProjector";

                primaryCausticsObj.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);
                secondaryCausticsObj.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);

                primaryCausticsObj.GetComponent<Projector>().orthographicSize = sceneInfo.m_sceneBounds.extents.x;
                secondaryCausticsObj.GetComponent<Projector>().orthographicSize = sceneInfo.m_sceneBounds.extents.x;

                primaryCausticsObj.transform.SetParent(aquasObj.transform);
                secondaryCausticsObj.transform.SetParent(aquasObj.transform);

                //Set some material parameters based on whether Tenkoku is in the scene or not
                if (GameObject.Find("Tenkoku DynamicSky") != null)
                {
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_EnableCustomFog", 1);
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_Specular", 0.5f);
                }

                else 
                {
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_EnableCustomFog", 0);
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_Specular", 1);
                }
            }
        }

        //<summary>
        //Informs the user how to set up a river with Gaia
        //</summary>
        public static void GX_HowToSetUpRivers()
        {
            EditorUtility.DisplayDialog("How to set up rivers with Gaia", "To set up rivers use only the river-specific buttons - even for the underwater effects. Exporting a river reference correctly requires a river plane, NOT a round water plane.", "Got It!");
        }

        //<summary>
        //Adds a river plane to the Scene
        //</summary>
        public static void GX_AddRiver()
        {
            //Get scene info
            GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();

            //Find camera
            Camera camera = Camera.main;

            if (camera == null)
            {
                camera = FindObjectOfType<Camera>();
            }

            //Try to add AQUAS river
            GameObject aquasObj = GameObject.CreatePrimitive(PrimitiveType.Plane);

            if (aquasObj == null)
            {
                EditorUtility.DisplayDialog("Can't add water", "Unable to create AQUAS object", "Ok");
                return;
            }

            //Position the waterplane correctly
            else
            {
                aquasObj.name = "AQUAS Waterplane";
                aquasObj.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);
                aquasObj.transform.localScale = sceneInfo.m_sceneBounds.extents/5;
                DestroyImmediate(aquasObj.GetComponent<MeshCollider>());
                aquasObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                aquasObj.GetComponent<Renderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS/Materials/Water/Desktop&Web/River.mat", typeof(Material));
                aquasObj.AddComponent<AQUAS_Reflection>();
                aquasObj.AddComponent<AQUAS_RenderQueueEditor>();

                if (camera.gameObject.GetComponent<AQUAS_Camera>() == null)
                {
                    camera.gameObject.AddComponent<AQUAS_Camera>();
                }

                //Check if Tenkoku is in the scene
                if (GameObject.Find("Tenkoku DynamicSky") != null)
                {

                }

                else
                {
                    GameObject refProbe = new GameObject("Reflection Probe");
                    refProbe.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);
                    refProbe.AddComponent<ReflectionProbe>();
                    refProbe.GetComponent<ReflectionProbe>().intensity = 0.3f;
                    refProbe.transform.SetParent(aquasObj.transform);
                    refProbe.GetComponent<ReflectionProbe>().mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
                }

                //Add caustics
                GameObject primaryCausticsPrefab = Gaia.Utils.GetAssetPrefab("PrimaryCausticsProjector");
                GameObject primaryCausticsObj = Instantiate(primaryCausticsPrefab);
                primaryCausticsObj.name = "PrimaryCausticsProjector";
                GameObject secondaryCausticsPrefab = Gaia.Utils.GetAssetPrefab("SecondaryCausticsProjector");
                GameObject secondaryCausticsObj = Instantiate(secondaryCausticsPrefab);
                secondaryCausticsObj.name = "SecondaryCausticsProjector";

                primaryCausticsObj.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);
                secondaryCausticsObj.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel, sceneInfo.m_centrePointOnTerrain.z);

                primaryCausticsObj.GetComponent<Projector>().orthographicSize = sceneInfo.m_sceneBounds.extents.x;
                secondaryCausticsObj.GetComponent<Projector>().orthographicSize = sceneInfo.m_sceneBounds.extents.x;

                primaryCausticsObj.transform.SetParent(aquasObj.transform);
                secondaryCausticsObj.transform.SetParent(aquasObj.transform);

                //Set some material parameters based on whether Tenkoku is in the scene or not
                if (GameObject.Find("Tenkoku DynamicSky") != null)
                {
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_EnableCustomFog", 1);
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_Specular", 0.5f);
                }

                else
                {
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_EnableCustomFog", 0);
                    aquasObj.GetComponent<Renderer>().sharedMaterial.SetFloat("_Specular", 1);
                }
            }
        }

        public static void GX_CreateRiverReference()
        {
            //Get scene info
            GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();

            //Find camera
            Camera camera = Camera.main;

            //Check if required objects (camera & river plane) are specified and abort if not
            if (camera == null)
            {
                EditorUtility.DisplayDialog("Camera Missing!", "Please specify the main camera before creating a river reference.", "OK");
                return;
            }

            if (GameObject.Find("AQUAS Waterplane") == null)
            {
                EditorUtility.DisplayDialog("River Plane Missing", "Please add a river plane before creating a river reference.", "OK");
                return;
            }

            #region Screenshot setup
            //Cache the camera's parameters
            Vector3 cameraPosition = camera.transform.position;
            Quaternion cameraRotation = camera.transform.rotation;
            bool projection = camera.GetComponent<Camera>().orthographic;
            float orthographicSize = camera.GetComponent<Camera>().orthographicSize;

            //Set the camera's position and parameters for taking a screenshot
            camera.transform.position = new Vector3(sceneInfo.m_centrePointOnTerrain.x, sceneInfo.m_seaLevel+(camera.GetComponent<Camera>().farClipPlane-50), sceneInfo.m_centrePointOnTerrain.z);
            camera.transform.rotation = Quaternion.Euler(90, 0, 0);
            camera.GetComponent<Camera>().orthographic = true;
            camera.GetComponent<Camera>().orthographicSize = sceneInfo.m_sceneBounds.extents.x;

            //Cache the material from the river plane and set the material with the reference texture on it
            Material cachedMaterial = GameObject.Find("AQUAS Waterplane").GetComponent<UnityEngine.Renderer>().sharedMaterial;
            Material referenceTexMat = (Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS/Materials/RiverRef.mat", typeof(Material));
            GameObject.Find("AQUAS Waterplane").GetComponent<UnityEngine.Renderer>().sharedMaterial = referenceTexMat;
            #endregion

            #region Take & save screenshot
            //Take a screenshot from the river plane and save it to use as a reference texture to help paint flowmaps
            //Source: http://answers.unity3d.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
            RenderTexture rt = new RenderTexture(1024, 1024, 24);

            camera.GetComponent<Camera>().targetTexture = rt;

            Texture2D riverReference = new Texture2D(1024, 1024, TextureFormat.RGB24, false);

            camera.GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            riverReference.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
            camera.GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);

            byte[] bytes = riverReference.EncodeToJPG();
            string filename = string.Format("{0}/RiverReferences/Reference_{1}.jpg", Application.dataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            if (!System.IO.Directory.Exists(string.Format("{0}/RiverReferences", Application.dataPath)))
            {
                System.IO.Directory.CreateDirectory(string.Format("{0}/RiverReferences", Application.dataPath));
            }

            System.IO.File.WriteAllBytes(filename, bytes);
            #endregion

            #region Revert scene to original state
            camera.transform.position = cameraPosition;
            camera.transform.rotation = cameraRotation;
            camera.GetComponent<Camera>().orthographic = projection;
            camera.GetComponent<Camera>().orthographicSize = orthographicSize;

            GameObject.Find("AQUAS Waterplane").GetComponent<UnityEngine.Renderer>().sharedMaterial = cachedMaterial;
            #endregion

            AssetDatabase.Refresh();
        }

        //<summary>
        //Adds Underwater Effects to the scene
        //</summary>
        public static void GX_AddUnderwaterEffects()
        {

            //Collect Scene info
            GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();

            GameObject underwaterPrefab = Gaia.Utils.GetAssetPrefab("UnderWaterCameraEffects");

            //Check if AQUAS is already in the scene
            if (GameObject.Find("AQUAS Waterplane") == null)
            {
                EditorUtility.DisplayDialog("AQUAS is not in the scene", "Please add water first", "Ok");
                return;
            }

            //Try to add underwater effects
            else
            {

                if (GameObject.Find("UnderWaterCameraEffects") != null)
                {
                    EditorUtility.DisplayDialog("Can't add underwater effects", "Underwater Effects are already in the scene", "Ok");
                    return;
                }


                GameObject underwaterObj = Instantiate(underwaterPrefab);
                underwaterObj.name = "UnderWaterCameraEffects";

                //Find camera
                Camera camera = Camera.main;

                if (camera == null)
                {
                    camera = FindObjectOfType<Camera>();
                }

                camera.farClipPlane = sceneInfo.m_sceneBounds.extents.x * 2;

                //Underwater effects setup
                underwaterObj.transform.SetParent(camera.transform);
                underwaterObj.transform.localPosition = new Vector3(0, 0, 0);
                underwaterObj.transform.localEulerAngles = new Vector3(0, 0, 0);
                underwaterObj.GetComponent<AQUAS_LensEffects>().gameObjects.mainCamera = camera.gameObject;
                underwaterObj.GetComponent<AQUAS_LensEffects>().gameObjects.waterPlanes[0] = GameObject.Find("AQUAS Waterplane");

                if (GameObject.Find("AQUAS Waterplane").GetComponent<MeshFilter>().sharedMesh.name == "Plane" || GameObject.Find("AQUAS Waterplane").GetComponent<MeshFilter>().sharedMesh.name == "Plane Instance")
                {
                    underwaterObj.GetComponent<AQUAS_LensEffects>().gameObjects.useSquaredPlanes = true;
                }

                else
                {
                    underwaterObj.GetComponent<AQUAS_LensEffects>().gameObjects.useSquaredPlanes = false;
                }

                //Add and configure image effects neccessary for AQUAS
                if (camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>() == null)
                {
                    camera.gameObject.AddComponent<UnityStandardAssets.ImageEffects.Bloom>();
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().bloomIntensity = 0.4f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = true;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().bloomThreshold = 0.5f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().bloomBlurIterations = 1;
                }

                else
                {
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().bloomIntensity = 0.4f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = true;
                }

                if (camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>() == null)
                {
                    camera.gameObject.AddComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().downsample = 0;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = 3;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = 2;
                }

                else
                {
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().downsample = 0;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = 3;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = 2;
                }

                if (camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>() == null)
                {
                    camera.gameObject.AddComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>();
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().enabled = true;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity = 0;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().blur = 0.54f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().blurDistance = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().chromaticAberration = 0;
                }

                else
                {
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().enabled = true;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity = 0;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().blur = 0.54f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().blurDistance = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().chromaticAberration = 0;
                }

                if (camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>() == null)
                {
                    camera.gameObject.AddComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>();
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().enabled = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().dx11Grain = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().monochrome = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().intensityMultiplier = 1.86f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().generalIntensity = 0;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().blackIntensity = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().whiteIntensity = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().midGrey = 0.2f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().softness = 0.276f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().tiling = new Vector3(512, 512, 512);
                }

                else
                {
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().enabled = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().dx11Grain = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().monochrome = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().intensityMultiplier = 1.86f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().generalIntensity = 0;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().blackIntensity = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().whiteIntensity = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().midGrey = 0.2f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().softness = 0.276f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>().tiling = new Vector3(512, 512, 512);
                }

                if (camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>() == null)
                {
                    camera.gameObject.AddComponent<UnityStandardAssets.ImageEffects.SunShafts>();
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().enabled = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunShaftIntensity = 0.6f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunShaftBlurRadius = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().radialBlurIterations = 3;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().maxRadius = 0.1f;
                }

                else
                {
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().enabled = false;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunShaftIntensity = 0.6f;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunShaftBlurRadius = 1;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().radialBlurIterations = 3;
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().maxRadius = 0.1f;
                }

                if (camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.GlobalFog>() == null)
                {
                    camera.gameObject.AddComponent<UnityStandardAssets.ImageEffects.GlobalFog>();
                    camera.gameObject.GetComponent<UnityStandardAssets.ImageEffects.GlobalFog>().enabled = false;
                }

                if (camera.gameObject.GetComponent<AudioSource>() == null)
                {
                    camera.gameObject.AddComponent<AudioSource>();
                }

                //Add borders to the scene for correct fog

                if (GameObject.Find("Borders") != null)
                {
                    return;
                }

                GameObject borders = new GameObject();
                borders.name = "Borders";

                GameObject borderLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
                borderLeft.name = "Left Border";
                borderLeft.transform.localScale = new Vector3(sceneInfo.m_sceneBounds.extents.x * 2, sceneInfo.m_seaLevel, 0.1f);
                borderLeft.transform.position = new Vector3(sceneInfo.m_sceneBounds.center.x, sceneInfo.m_seaLevel - (sceneInfo.m_seaLevel / 2), sceneInfo.m_sceneBounds.extents.z);

                GameObject borderRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
                borderRight.name = "Right Border";
                borderRight.transform.localScale = new Vector3(sceneInfo.m_sceneBounds.extents.x * 2, sceneInfo.m_seaLevel, 0.1f);
                borderRight.transform.position = new Vector3(sceneInfo.m_sceneBounds.center.x, sceneInfo.m_seaLevel - (sceneInfo.m_seaLevel / 2), -sceneInfo.m_sceneBounds.extents.z);

                GameObject borderTop = GameObject.CreatePrimitive(PrimitiveType.Cube);
                borderTop.name = "Top Border";
                borderTop.transform.localScale = new Vector3(0.1f, sceneInfo.m_seaLevel, sceneInfo.m_sceneBounds.extents.z * 2);
                borderTop.transform.position = new Vector3(sceneInfo.m_sceneBounds.extents.x, sceneInfo.m_seaLevel - (sceneInfo.m_seaLevel / 2), sceneInfo.m_sceneBounds.center.z);

                GameObject borderBottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
                borderBottom.name = "Bottom Border";
                borderBottom.transform.localScale = new Vector3(0.1f, sceneInfo.m_seaLevel, sceneInfo.m_sceneBounds.extents.z * 2);
                borderBottom.transform.position = new Vector3(-sceneInfo.m_sceneBounds.extents.x, sceneInfo.m_seaLevel - (sceneInfo.m_seaLevel / 2), -sceneInfo.m_sceneBounds.center.z);

                borderLeft.transform.SetParent(borders.transform);
                borderRight.transform.SetParent(borders.transform);
                borderTop.transform.SetParent(borders.transform);
                borderBottom.transform.SetParent(borders.transform);

                borders.transform.SetParent(GameObject.Find("AQUAS Waterplane").transform);

            }
        }

        //<summary>
        //Add hints dialog
        //</summary>
        public static void GX_Hints()
        {
            EditorUtility.DisplayDialog("Hints", "Using AQUAS with custom lighting systems like Tenkoku: When using AQUAS with custom lighting systems like Tenkoku or TOD, you might have to adjust the specular value on the water material. Custom lighting systems sometimes use z-buffer-based fog, which AQUAS cannot receive. If AQUAS doesn't receive fog, enable the custom fog property on the water material.", "Got It!");
        }
    }
}
#endif
