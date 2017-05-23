#pragma strict

var followPlayer : boolean = false;
static var player : Transform;

function Start () {
	yield WaitForSeconds(Time.deltaTime);
	player = GameObject.FindGameObjectWithTag("Player").transform;
}

function Update () {
	if (followPlayer){
		transform.LookAt(player);
	}

	if (Input.GetKeyDown("n")){
		followPlayer = !followPlayer;
	}
}