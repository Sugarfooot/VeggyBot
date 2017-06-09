#pragma strict

var gameManager : GameManager;
var musicToPlay : AudioClip;
var collidersToDisable : BoxCollider[];
var collidersToEnable : BoxCollider[];

function Start () {
	if (collidersToDisable.Length > 0){
		collidersToDisable += [GetComponent.<BoxCollider>()];
	}
	else {
		collidersToDisable = [];
	}
}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		gameManager.PlayMusic(musicToPlay);
		for (var i = 0; i < collidersToEnable.Length; i++){
			collidersToEnable[i].enabled = true;
		}
		for (var j = 0; j < collidersToEnable.Length; j++){
			collidersToDisable[j].enabled = false;
		}
	}
}