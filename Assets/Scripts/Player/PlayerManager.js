#pragma strict

private var spawnPoint : Vector3;

function Start () {
	spawnPoint = transform.position;
}

function Update () {

}

function Respawn (){
	transform.position = spawnPoint;
}

function SetNewSpawnPoint (){
	spawnPoint = transform.position;
}