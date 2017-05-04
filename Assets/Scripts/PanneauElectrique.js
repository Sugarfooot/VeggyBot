#pragma strict

var panelHP : float;
var wastedPanel : GameObject;
var activePanel : MeshRenderer;
var animatorsToActivate : Animator[];
var isWet : boolean = false;

function Start () {

}

function Update () {
	if (isWet){
		panelHP -= Time.deltaTime * 5;
		if (panelHP <= 0){
			TriggerDeactivation();
		}
	}

	isWet = false;
}

function OnParticleCollision (weaponObject : GameObject){
	if (weaponObject.CompareTag("PlayerWeapon") && activePanel.enabled){
		// TriggerDeactivation();
		isWet = true;
	}
}

function TriggerDeactivation (){
	isWet = false;
	GetComponent.<Collider>().enabled = false;
	activePanel.enabled = false;
	wastedPanel.SetActive(true);
	for (var meca : Animator in animatorsToActivate){
		meca.SetTrigger("Activate");
	}
}