#pragma strict

import UnityEngine.SceneManagement;

var pomoAnimator : Animator;
//var larry : LarryBehaviour;

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Larry")){
		pomoAnimator.SetTrigger("TakeOff");
		collider.GetComponent.<LarryBehaviour>().TakeOffAnim();
		yield WaitForSeconds (1.6);
		SceneManager.LoadScene("proto_02_blockOut");
		//SceneManager.LoadScene(SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1).name);
	}
}