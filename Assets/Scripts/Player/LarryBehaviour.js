#pragma strict

private var takeOffTarget : Transform = null;
private var larrymator : Animator;
private var moveToPomo : boolean = false;

function Start () {
	larrymator = GetComponent.<Animator>();
}

function Update () {
	if(moveToPomo && takeOffTarget != null){
		transform.position = Vector3.MoveTowards(transform.position, takeOffTarget.position, Time.deltaTime * 3);
	}
}

function TakeOff (){
	takeOffTarget = GameObject.Find("LarryEndPos").transform;
	moveToPomo = true;
}

function TakeOffAnim (){
	larrymator.SetTrigger("TakeOff");
}