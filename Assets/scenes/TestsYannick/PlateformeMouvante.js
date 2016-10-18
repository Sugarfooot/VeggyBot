#pragma strict

var plateformeMouvante : boolean = false;
var speed : float = 2.0;
var waypoints : Transform;
private var waypointsIdx : int = -1;
private var move : boolean = false;

function Start () {
	if (plateformeMouvante){
		waypoints.SetParent(null);
		SetTargetWaypoint();
		SwitchOn();
	}
}

function Update () {
	if (move && plateformeMouvante){
		if (transform.position != waypoints.GetChild(waypointsIdx).position){
			transform.position = Vector3.MoveTowards(transform.position,waypoints.GetChild(waypointsIdx).position,speed * Time.deltaTime);
		}
		else if (transform.position == waypoints.GetChild(waypointsIdx).position){
			if (waypointsIdx == waypoints.childCount - 1){
				ResetTargetWaypoints();
			}
			else if (waypointsIdx < waypoints.childCount - 1){
				SetTargetWaypoint();
			}
		}
	}
}

function SwitchOn (){
	move = true;
}

function SwitchOff (){
	move = false;
}

function SetTargetWaypoint (){
	waypointsIdx += 1;
}

function ResetTargetWaypoints (){
	waypointsIdx = 0;
}