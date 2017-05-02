#pragma strict

var trsToLookAt : Transform;

function Start () {

}

function Update () {
	transform.LookAt(Vector3(trsToLookAt.position.x, transform.position.y,trsToLookAt.position.z));
}