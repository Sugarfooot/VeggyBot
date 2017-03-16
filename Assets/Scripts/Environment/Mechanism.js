#pragma strict

@Header ("Mech Type")
var door : boolean = false;

function Start () {

}

function Update () {

}

function MechOn (){
	if (door){
		gameObject.SetActive(false);
		door = false;
	}
}