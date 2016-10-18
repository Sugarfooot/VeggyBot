#pragma strict

var plateformeMouvante : boolean = false;
var speed : float = 2.0;
var tempsDAttente : float = 2.0;
var waypoints : Transform;
private var localRB : Rigidbody;
private var waypointsIdx : int = 0;
private var move : boolean = false;

function Start () {
	localRB = GetComponent.<Rigidbody>();
	if (plateformeMouvante){
		waypoints.SetParent(null);
		SetTargetWaypoint();
	}
}

function Update (){

}

function FixedUpdate () {
	var localFwd : Vector3;
	if (waypointsIdx == 0){
		localFwd = waypoints.GetChild(waypointsIdx).position - waypoints.GetChild(waypoints.childCount - 1).position;
	}
	else if (waypointsIdx != 0){
		localFwd = waypoints.GetChild(waypointsIdx).position - waypoints.GetChild(waypointsIdx - 1).position;
	}
	if (move && plateformeMouvante){
		localRB.MovePosition(transform.position + localFwd.normalized * speed * Time.deltaTime);
	}
}

function SwitchOn (){
	move = true;
}

function SwitchOff (){
	move = false;
}

function SetTargetWaypoint (){
	waypoints.GetChild(waypointsIdx).gameObject.GetComponent.<Collider>().enabled = false;
	SwitchOff();
	yield WaitForSeconds(tempsDAttente);
	if (waypointsIdx == waypoints.childCount - 1){
		waypointsIdx = 0;
	}
	else if (waypointsIdx < waypoints.childCount - 1){
		waypointsIdx += 1;
	}
	waypoints.GetChild(waypointsIdx).gameObject.GetComponent.<Collider>().enabled = true;
	SwitchOn();
}

function OnTriggerEnter (collider : Collider){
	for (var i = 0; i < waypoints.childCount; i++){
		if (collider.gameObject == waypoints.GetChild(i).gameObject){
			SetTargetWaypoint();
			return;
		}
	}
}