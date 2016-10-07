#pragma strict

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		collider.GetComponent.<PlayerManager>().SetNewSpawnPoint();
	}
	GetComponent.<Collider>().enabled = false;
}