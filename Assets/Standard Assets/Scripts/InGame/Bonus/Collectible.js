#pragma strict

@script RequireComponent(BoxCollider)

var rouage : boolean = false;
var eau25 : boolean = false;
var eau50 : boolean = false;
var eau100 : boolean = false;

function Start () {
	GetComponent.<BoxCollider>().isTrigger = true;
	if (rouage){
		eau25 = false;
		eau50 = false;
		eau100 = false;
	}
	else if (eau25){
		eau50 = false;
		eau100 = false;
	}
	else if (eau50){
		eau100 = false;
	}
	if (!rouage && !eau25 && !eau50 && !eau100){
		eau25 = true;
	}
}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		if (rouage){
			BonusManager.Instance().AddLifePart();
		}
		if (eau25){
			PlayerManager.Instance().AddWaterLevel(0.25);
		}
		if (eau50){
			PlayerManager.Instance().AddWaterLevel(0.5);
		}
		if (eau100){
			PlayerManager.Instance().AddWaterLevel(1.0);
		}
		Destroy(gameObject,Time.deltaTime);
	}
}