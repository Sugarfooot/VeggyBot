#pragma strict

var enemy : Enemy;

private var currentSoul : int;
private var currentIntuition : int;
// private var speed : float;
private var aiAnimator : Animator;
private var idleTime : float = 0.0;
var damageAmount : int = 8;
var attackDistance : float;
// var stickersPool : Album[];
// var dropItemsPool : DropItems;
// private var validDrops : ItemData[];
var playerSpotted : boolean = false;
var isPatroller : boolean = false;
var patrolWayParent : Transform;
private var targetPlayer : GameObject = null;
private var isStalking : boolean = false;
private var isDead : boolean = false;
private var navAgent : NavMeshAgent;
private var navIdx : int = -1;
var spottedMtl : Material;

function Start () {
	currentSoul = enemy.maxSoul;
	currentIntuition = enemy.maxIntuition;
	aiAnimator = GetComponent.<Animator>();
	if (isPatroller){
		// navAgent = GetComponent.<NavMeshAgent>();
		StartPatrolling();
		aiAnimator.SetTrigger("Walk");
	}
	// else if (!isPatroller){
	// 	targetPlayer = GameObject.FindGameObjectWithTag("Player");
	// 	isStalking = true;
	// 	aiAnimator.SetTrigger("Walk");
	// 	// FollowPlayer();
	// }
}

function Update () {
	// if (idleTime > 0.0){
	// 	idleTime -= Time.deltaTime;
	// 	if (idleTime <= 0.0){
	// 		StartPatrolling();
	// 	}
	// }

	if (isStalking && targetPlayer != null && !isDead){
		transform.LookAt(Vector3(targetPlayer.transform.position.x, transform.position.y, targetPlayer.transform.position.z));
		if (Vector3.Distance(targetPlayer.transform.position, transform.position) < attackDistance){
			Attack();
		}
		else{
			FollowPlayer();
		}
		// var targetDir = targetPlayer.transform.position - transform.position;
		
	 //    // The step size is equal to speed times frame time.
	    // var step = 5.0 * Time.deltaTime;
	    
	    // var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0);
	 //    // Move our position a step closer to the target.
	    // transform.rotation = Quaternion.LookRotation(newDir);
		// transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, speed * Time.deltaTime);
	}
	// else if (isPatroller && targetPlayer != null && !isDead){
	// 	transform.LookAt(Vector3(targetPlayer.transform.position.x, transform.position.y, targetPlayer.transform.position.z));
	// }

	if (isDead){
		transform.localScale -= Vector3(0.1, 0.1, 0.1);
	}
}

function OnTriggerEnter (collider : Collider){
	if (collider.CompareTag("Player") && !playerSpotted){
		playerSpotted = true;
		targetPlayer = collider.gameObject;
		transform.LookAt(Vector3(targetPlayer.transform.position.x, transform.position.y, targetPlayer.transform.position.z));
		aiAnimator.SetTrigger("SpotPlayer");
		yield WaitForSeconds(1.5);
		isStalking = true;
	}
}

function Attack (){
	isStalking = false;
	aiAnimator.SetTrigger("Attack");
	yield WaitForSeconds (1.5);
	isStalking = true;
}

function StartPatrolling (){
	// LeavePlayerBe();
	// SetNewPatrolDestination();
	// targetPlayer = patrolWayParent.GetChild(navIdx).gameObject;
	// aiAnimator.SetTrigger("Walk");
}

// function StopPatrolling (){
// 	navAgent.destination = transform.position;
// }

// function WaitAtWaypoint (waitTime : float) : IEnumerator{
// 	aiAnimator.SetTrigger("Idle");
// 	idleTime = waitTime;
// }

// function SetNewPatrolDestination (){
// 	navIdx ++;
// 	if (navIdx >= patrolWayParent.childCount){
// 		navIdx = 0;
// 	}
// }

// function IsPatrolling (){
// 	return isPatroller;
// }

function FollowPlayer (){
	// if (isPatroller){
	// 	StopPatrolling();
	// }
	// idleTime = 0.0;
	// isPatroller = false;
	// isStalking = true;
	// targetPlayer = GameObject.FindGameObjectWithTag("Player");
	aiAnimator.SetTrigger("Run");
}

// function LeavePlayerBe (){
// 	isStalking = false;
// }

function TakeDamage (amount : int){
	FollowPlayer();
	if (!isDead){
		currentSoul -= amount;
		if (currentSoul <= 0){
			currentSoul = 0;
			// LeavePlayerBe();
			Die();
		}
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
	// targetPlayer.GetComponent.<ExpManager>().AddExp(enemy.amountWhenDead);
	Destroy(gameObject,1.5);
}

function GetDamage (){
	return damageAmount;
}

// function OnDestroy (){
// 	var rndTypeRoll : int = Random.Range(0,101);
// 	if (rndTypeRoll >= 0 && rndTypeRoll < 15 && stickersPool.Length > 0){
// 		var rndAlbumIdx : int = Random.Range(0, stickersPool.Length);
// 		var rndSticker : int = Random.Range(0, stickersPool[rndAlbumIdx].albumStickers.Length);
// 		Instantiate(stickersPool[rndAlbumIdx].albumStickers[rndSticker].dropModel, transform.Find("DropPoint").transform.position, Quaternion.identity);
// 	}
// 	else if (rndTypeRoll >= 15 && rndTypeRoll < 71 && dropItemsPool.dropItems.Length > 0){
// 		validDrops = [];
// 		for (var i = 0; i < dropItemsPool.dropItems.Length; i++){
// 			var skillN : int = dropItemsPool.dropItems[i].skillNeeded;
// 			if (skillN == BasicCommonActions.Instance().GetCharacter().skill){
// 				validDrops += [dropItemsPool.dropItems[i]];
// 			}
// 		}
// 		if (validDrops.Length > 0){
// 			var rndItemIdx : int = Random.Range(0,validDrops.Length);
// 			Instantiate(validDrops[rndItemIdx].dropModel, transform.Find("DropPoint").transform.position, validDrops[rndItemIdx].dropModel.transform.rotation);
// 		}
// 	}
// }