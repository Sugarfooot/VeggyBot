#pragma strict

var pomoPrefab : GameObject;
var playerSpawnPoint : Transform;
var musicSrc : AudioSource;

function Start () {
	Instantiate(pomoPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
}

function Update () {
	
}

function PlayMusic (clp : AudioClip){
	musicSrc.clip = clp;
	musicSrc.Play();
}