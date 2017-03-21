#pragma strict

var pomoPrefab : GameObject;
var playerSpawnPoint : Transform;

function Start () {
	Instantiate(pomoPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
}

function Update () {
	
}