#pragma strict

var enemy : Enemy;

private var currentSoul : float;
var damagesFromPlayer : float = 0.5;
private var currentIntuition : int;
private var aiAnimator : Animator;
private var idleTime : float = 0.0;
var damageAmount : int = 8;
var attackDistance : float;
var playerSpotted : boolean = false;
var isPatrolling : boolean = false;
var waitTimeAtWaypoint : float = 3.0;
var patrolWayParent : Transform;
var particleAttack : ParticleSystem;
private var targetPlayer : GameObject = null;
private var targetWaypoint : Transform = null;
private var isStalking : boolean = false;
private var isDead : boolean = false;
private var bDamaged : boolean = true;
private var navIdx : int = -1;
var spottedMtl : Material;
private var audioSrc : AudioSource;

function Start () {
	currentSoul = enemy.maxSoul;
	currentIntuition = enemy.maxIntuition;
	aiAnimator = GetComponent.<Animator>();
	audioSrc = GetComponent.<AudioSource>();
	if (isPatrolling && patrolWayParent != null){
		navIdx = 0;
		StartPatrolling();
		aiAnimator.SetTrigger("Walk");
	}
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
	else if (isPatrolling){
		transform.LookAt(Vector3(targetWaypoint.position.x, transform.position.y, targetWaypoint.position.z));
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
	StopPatrolling();
	playerSpotted = true;
	transform.LookAt(Vector3(targetPlayer.transform.position.x, transform.position.y, targetPlayer.transform.position.z));
	aiAnimator.SetTrigger("SpotPlayer");
	yield WaitForSeconds(1.1);
	isStalking = true;
}

function Attack (){
	isStalking = false;
	aiAnimator.SetTrigger("Attack");
	yield WaitForSeconds (0.3);
	if (particleAttack != null){
		particleAttack.Play();
	}
	yield WaitForSeconds (1.1);
	isStalking = true;
}

function FollowPlayer (){
	aiAnimator.SetTrigger("Run");
}

function StartPatrolling (){
	if (patrolWayParent.childCount > 0){
		targetWaypoint = patrolWayParent.GetChild(navIdx);
	}
}

function StopPatrolling (){
	isPatrolling = false;
}

function IsPatrolling (){
	return isPatrolling;
}

function WaitAtWaypoint (){
	isPatrolling = false;
	yield WaitForSeconds(waitTimeAtWaypoint);
	isPatrolling = true;
}

function TargetNewWaypoint (){
	if (navIdx < patrolWayParent.childCount - 1){
		navIdx++;
	}
	else if (navIdx == patrolWayParent.childCount - 1){
		navIdx = 0;
	}
	targetWaypoint = patrolWayParent.GetChild(navIdx);
}

function TakeDamage (amount : float){
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
	if (weaponObject.CompareTag("PlayerWeapon")){
		TakeDamage(damagesFromPlayer);
	}
}

function Die (){
	isDead = true;
	gameObject.layer = 9;
	gameObject.tag = "Untagged";
	if (aiAnimator != null){
		aiAnimator.ResetTrigger("Walk");
		aiAnimator.SetTrigger("Death");
	}
	audioSrc.Play();
	PlayerManager.Instance().RemoveLockableEnemies(transform);
	Destroy(gameObject,0.5);
}

function GetDamage (){
	return damageAmount;
}