#pragma strict

var panelHP : int;
var wastedPanel : GameObject;
var activePanel : MeshRenderer;
var isWet : boolean = false;

function Start () {

}

function Update () {
	if (isWet){
		panelHP -= Time.deltaTime;
		if (panelHP <= 0){
			TriggerDeactivation();
		}
	}

	isWet = false;
}

function OnParticleCollision (particles : GameObject){
	if (particles.CompareTag("WaterPlayer")){
		isWet = true;
	}
}

function TriggerDeactivation (){
	GetComponent.<Collider>().enabled = false;
	activePanel.enabled = false;
	wastedPanel.SetActive(true);
}