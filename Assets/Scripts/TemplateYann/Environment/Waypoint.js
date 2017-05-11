#pragma strict

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Enemy") && collider.GetComponent.<VeryFakeAI>().IsPatrolling()){
		collider.GetComponent.<VeryFakeAI>().TargetNewWaypoint();
		collider.GetComponent.<VeryFakeAI>().WaitAtWaypoint();
	}
}