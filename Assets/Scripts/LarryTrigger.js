#pragma strict

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
	}
}