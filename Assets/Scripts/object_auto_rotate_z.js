#pragma strict

var rotateAmount : float = 5.0;

function Start () {

}

function Update () {

	this.transform.eulerAngles.x += (Time.deltaTime * rotateAmount);

}