﻿using UnityEngine;
using System.Collections;

public class vTriggerChangeCameraState : MonoBehaviour
{
    [Tooltip("Check if you want to lerp the state transitions, you can change the lerp value on the TPCamera - Smooth Follow variable")]
    public bool smoothTransition = true;
    public bool keepDirection = true;
    [Tooltip("Check your CameraState List and set the State here, use the same String value")]
    public string cameraState;
    [Tooltip("Set a new target for the camera, leave this field empty to return the original target (Player)")]
    public string customCameraPoint;

    public Color gizmoColor = Color.green;
    private Component comp = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // apply lerp transition between states
            Invector.CharacterController.vThirdPersonController.instance.smoothCameraState = smoothTransition;
            // change the camera state to a new string
            Invector.CharacterController.vThirdPersonController.instance.customCameraState = cameraState;
            // set new target for the camera
            Invector.CharacterController.vThirdPersonController.instance.customlookAtPoint = customCameraPoint;
            // activate custom camera state on the controller
            Invector.CharacterController.vThirdPersonController.instance.changeCameraState = (!string.IsNullOrEmpty(cameraState));
            Invector.CharacterController.vThirdPersonController.instance.keepDirection = keepDirection;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        comp = gameObject.GetComponent<BoxCollider>();        

        if (comp != null)
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.GetComponent<BoxCollider>().center = Vector3.zero;
            gameObject.GetComponent<BoxCollider>().size = Vector3.one;
        }
                
        Gizmos.matrix = transform.localToWorldMatrix;
        if (comp == null) gameObject.AddComponent<BoxCollider>();
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

    Vector3 getLargerScale(Vector3 value)
    {
        if (value.x > value.y || value.x > value.z)
            return new Vector3(value.x, value.x, value.x);
        if (value.y > value.x || value.y > value.z)
            return new Vector3(value.y, value.y, value.y);
        if (value.z > value.y || value.z > value.x)
            return new Vector3(value.z, value.z, value.z);

        return transform.localScale;
    }
}