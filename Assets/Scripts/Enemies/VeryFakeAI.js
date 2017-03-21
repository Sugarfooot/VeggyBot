#pragma strict

var enemy : Enemy;

private var currentSoul : int;
private var currentIntuition : int;
private var aiAnimator : Animator;
private var idleTime : float = 0.0;
var damageAmount : int = 8;
var attackDistance : float;
var playerSpotted : boolean = false;
var isPatroller : boolean = false;
var patrolWayParent : Transform;
private var targetPlayer : GameObject = null;
private var isStalking : boolean = false;
private var isDead : boolean = false;
private var bDamaged : boolean = true;
private var navAgent : NavMeshAgent;
private var navIdx : int = -1;
var spottedMtl : Material;

function Start () {
	currentSoul = enemy.maxSoul;
	currentIntuition = enemy.maxIntuition;
	aiAnimator = GetComponent.<Animator>();
	// if (isPatroller){
	// 	StartPatrolling();
	// 	aiAnimator.SetTrigger("Walk");
	// }
}

function Update () {

	if (isStalking && targetPlayer != null && !isDead){
		transform.LookAt(Vector3(targetPlayer.transform.position.x, transform.position.y, targetPlayer.transform.position.z));
		if (Vector3.Distance(targetPlayer.transform.position, transform.position) < attackDistance){
			Attack();
		}
		else{
			FollowPlayer();
		}
	}

	if (isDead){
		transform.localScale -= Vector3(0.1, 0.1, 0.1);
	}
}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player") && !playerSpotted){
		targetPlayer = collider.gameObject;	
		SpotPlayer();
	}
}

function SpotPlayer (){
	playerSpotted = true;
	transform.LookAt(Vector3(targetPlayer.transform.position.x, transform.position.y, targetPlayer.transform.position.z));
	aiAnimator.SetTrigger("SpotPlayer");
	yield WaitForSeconds(1.5);
	isStalking = true;
}

function Attack (){
	isStalking = false;
	aiAnimator.SetTrigger("Attack");
	yield WaitForSeconds (1.5);
	isStalking = true;
}

function FollowPlayer (){
	aiAnimator.SetTrigger("Run");
}

function TakeDamage (amount : int){
	if (!isStalking){
		SpotPlayer();
	}
	if (!isDead && bDamaged){
		currentSoul -= amount;
		if (currentSoul <= 0){
			currentSoul = 0;
			Die();
			return;
		}
		bDamaged = false;
	}
	yield WaitForSeconds (0.5);
	bDamaged = true;
}

function OnParticleCollision (weaponObject : GameObject){
	TakeDamage(10);
}

function Die (){
	isDead = true;
	gameObject.layer = 9;
	gameObject.tag = "Untagged";
	if (aiAnimator != null){
		aiAnimator.ResetTrigger("Walk");
		aiAnimator.SetTrigger("Death");
	}
	Destroy(gameObject,0.5);
}

function GetDamage (){
	return damageAmount;
}