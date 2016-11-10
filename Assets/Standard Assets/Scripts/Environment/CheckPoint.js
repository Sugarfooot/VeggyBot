#pragma strict

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		GetComponent.<Collider>().enabled = false;
		collider.GetComponent.<PlayerManager>().SetNewSpawnPoint();
	}
}