#pragma strict

var enemyDamaging : VeryFakeAI;

function Start () {

}

function Update () {

}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player")){
		collider.GetComponent.<PlayerManager>().TakeDamage(enemyDamaging.damageAmount);
	}
}