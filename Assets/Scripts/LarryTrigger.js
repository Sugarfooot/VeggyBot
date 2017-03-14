#pragma strict

var pomoAnimator : Animator;
var larry : LarryBehaviour;

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Larry")){
		pomoAnimator.SetTrigger("TakeOff");
		larry.TakeOffAnim();
		yield WaitForSeconds (1.6);
		//UIManager.Instance().LoadNextLevel();
	}
}