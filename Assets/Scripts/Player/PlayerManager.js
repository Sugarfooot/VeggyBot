#pragma strict

private var spawnPoint : Transform;

function Start () {
	spawnPoint.position = transform.position;
}

function Update () {

}

function Respawn (){
	transform.position = spawnPoint.position;
}

function SetNewSpawnPoint (){
	spawnPoint.position = transform.position;
}