#pragma strict

var timeToWait : float = 2.0;

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	// if (collider.CompareTag("Enemy") && collider.GetComponent.<VeryFakeAI>().IsPatrolling()){
	// 	collider.GetComponent.<VeryFakeAI>().WaitAtWaypoint(timeToWait);
	// 	GetComponent.<Collider>().enabled = false;
	// 	yield WaitForSeconds(timeToWait + 2);
	// 	GetComponent.<Collider>().enabled = true;
	// }
}