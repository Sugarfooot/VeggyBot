#pragma strict

var projectile : ProjectileBDD;
private var damageAmount : int;
private var rb : Rigidbody;

function Start () {
	rb = GetComponent.<Rigidbody>();
}

function Update () {
	if (gameObject.CompareTag("SoftMarble")){
		if (rb.velocity.x < 1 && rb.velocity.y < 1 && rb.velocity.z < 1){
			damageAmount = 0;
		}
		else {
			damageAmount = 10;
		}
	}
}

function UpdateDamageAmount (addAmount : int){
	damageAmount = projectile.damagesAmount + addAmount;
}

function GetDamage (){
	return damageAmount;
}

function OnCollisionEnter (collision : Collision){
	if (gameObject.CompareTag("Projectile")){
		yield WaitForSeconds(Time.deltaTime);
		if (projectile.projectileName == "Marble" && collision.collider.CompareTag("Ground")){
			gameObject.tag = "SoftMarble";
		}
		else if (projectile.projectileName == "Shuriken"){
			Destroy(gameObject, Time.deltaTime);
		}
		else if (projectile.projectileName == "Candy"){
			gameObject.tag = "Untagged";
		}
		else if (projectile.projectileName == "Shock"){
			gameObject.tag = "Untagged";
		}
	}
	else if (gameObject.CompareTag("Untagged")){
		Destroy(gameObject, Time.deltaTime);
	}
}