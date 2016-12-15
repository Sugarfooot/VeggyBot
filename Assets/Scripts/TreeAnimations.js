#pragma strict

private var rndDelay : float = 4.0;

function Start () {
	yield WaitForSeconds (Random.Range(0.1, rndDelay));
	GetComponent.<Animation>().Play();
}

function Update () {

}