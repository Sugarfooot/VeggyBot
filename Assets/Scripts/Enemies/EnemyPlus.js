#pragma strict

@Header ("Triggered :")
var onDeath : boolean = false;
var triggeredMechanism : Mechanism;

function Start () {

}

function Update () {

}

function TriggerMechanism (){
	if (triggeredMechanism != null){
		triggeredMechanism.MechOn();
	}
}