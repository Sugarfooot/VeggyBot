#pragma strict

import UnityEngine.SceneManagement;

var larry : LarryBehaviour;

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		GetComponent.<Collider>().enabled = false;
		collider.GetComponent.<PlayerManager>().enabled = false;
		collider.GetComponent.<Animator>().SetFloat("Speed",0.0);
		collider.SendMessage("FreezeMoves");
		larry.TakeOff();
	}
}

