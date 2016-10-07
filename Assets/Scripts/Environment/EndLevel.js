#pragma strict

import UnityEngine.SceneManagement;

var nextLevel : String;

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		GetComponent.<Collider>().enabled = false;
		print ("CONGRATULATIONS !!");
		SceneManager.LoadScene(nextLevel);
	}
}