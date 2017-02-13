#pragma strict

var pomo : Transform;
var larrymator : Animator;
private var moveToPomo : boolean = false;

function Start () {

}

function Update () {
	if(moveToPomo){
		transform.position = Vector3.MoveTowards(transform.position, pomo.position, Time.deltaTime * 3);
	}
}

function TakeOff (){
	moveToPomo = true;
}

function TakeOffAnim (){
	larrymator.SetTrigger("TakeOff");
}