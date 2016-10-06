using UnityEngine;
using System.Collections;

public class vTriggerDragable : MonoBehaviour
{
    public string message = "Drag";
    public float targetDistance = 1.5f;
    public float IKLerp = 2f;
    public Transform IKLeftHand, IKRightHand;
    [HideInInspector]
    public vDraggableBox box;

	void Start ()
    {
        box = GetComponentInParent<vDraggableBox>();
	}	
	
}
