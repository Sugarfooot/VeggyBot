#pragma strict

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		UIManager.Instance().FillWaterTank();
		Destroy(gameObject);
	}
}