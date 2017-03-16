#pragma strict

var popSpots : PopSpot[];

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Bill")){
		GetComponent.<Collider>().enabled = false;
		for (var i = 0; i < popSpots.Length; i++){
			popSpots[i].StartPopping();
		}
	}
}