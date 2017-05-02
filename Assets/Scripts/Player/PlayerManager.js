#pragma strict

import UnityEngine.UI;

public class PlayerManager extends MonoBehaviour {

	var pomoCharacter : Character;
	var rightArmIK : Transform;
	private var currentHealth : float;
	private var bDamaged : boolean = true;
	var waterIndicators : Image[];
	private var canShoot : boolean = true;
	private var isShooting : boolean = false;
	private var spawnPoint : Vector3;
	private var waterLevel : float = 0;
	var shootConsumptionSpeed : float = 0.1;
	var vimage : Image;
	private var animator : Animator;
	var rotateSpeed : float = 20.0;
	private var levelEnd : boolean = false;
	var hose : ParticleSystem;
	private var transformToLock : Transform[] = [];
	private var enemyLockedIdx : int = -1;
	private var lockOn : Transform = null;
	private var isDead : boolean = false; 

	var refTrs : Transform;
	var armTrs : Transform;
	
	private static var instance : PlayerManager;
	public static function Instance () : PlayerManager {
		return instance;
	}

	function Awake (){
		if (instance != null){
			Destroy (gameObject);
		}
		else {
			instance = this;
		}
	}

	function Start () {
		spawnPoint = transform.position;
		animator = GetComponent.<Animator>();
		currentHealth = pomoCharacter.maxSoul;
	}

	function Update () {
		if (levelEnd || isDead){
			return;
		}

		if (Input.GetButtonDown("RB") && UIManager.Instance().ConsumeWaterTank() > 0 && canShoot){
			if (transformToLock.Length > 0){
				lockOn = GetClosestEnemy();
			}
			if (lockOn != null){
				transform.LookAt(Vector3(lockOn.position.x, transform.position.y, lockOn.position.z));
			}
			StartShooting();
		}

		if (Input.GetButton("RB") && isShooting && !canShoot){
			if (UIManager.Instance().ConsumeWaterTank() <= 0){
				StopShooting();
			}
		}
		if (Input.GetButtonUp("RB") || (!Input.GetButton("RB") && isShooting)){
			StopShooting();
		}
	}

	function GetClosestEnemy () : Transform{
		var closestTrs : Transform = transformToLock[0];
		for (var trs : Transform in transformToLock){
			if (Vector3.Distance(transform.position, trs.position) < Vector3.Distance(transform.position, closestTrs.position)){
				closestTrs = trs;
			}
		}
		return closestTrs;
	}

	function OnAnimatorIK (){
		if (transformToLock.Length > 0 && isShooting && lockOn != null){
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3(lockOn.position.x, lockOn.position.y + 1, lockOn.position.z));
			if (armTrs.localRotation.eulerAngles.y - refTrs.localRotation.eulerAngles.y > 140 && armTrs.localRotation.eulerAngles.y - refTrs.localRotation.eulerAngles.y < 340){
				lockOn = null;
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
			}
		}
	}

	function StartShooting (){
		canShoot = false;
		isShooting = true;
		animator.SetBool("Attack", true);
		hose.Play();
	}

	function StopShooting (){
		canShoot = true;
		isShooting = false;
		animator.SetBool("Attack", false);
        hose.Stop();
	}

	function Respawn (){
		isDead = true;
		animator.SetTrigger("Death");
		StopShooting();
		yield WaitForSeconds(1.49);
		animator.SetTrigger("Idle");
		currentHealth = pomoCharacter.maxSoul;
		UIManager.Instance().UpdateLifeGear(currentHealth);
		bDamaged = true;
		transform.position = spawnPoint;
		isDead = false;
	}

	function SetNewSpawnPoint (){
		spawnPoint = transform.position;
	}

	function AddWaterLevel (quantity : float){
		waterLevel += quantity;
		if (waterLevel > 1){
			waterLevel = 1;
		}
		for (var i = 0; i < waterIndicators.Length; i++){
			waterIndicators[i].fillAmount = waterLevel;
		}
	}

	function OnTriggerEnter (collider : Collider){
		if (collider.CompareTag("Damage")){
			collider.enabled = false;
			yield WaitForSeconds(0.5);
			collider.enabled = true;
		}
		if (collider.CompareTag("Enemy")){
			// if (transformToLock.Length == 1){
			// 	enemyLockedIdx = 0;
			// }
			transformToLock += [collider.transform];
			// UpdateLockableEnemies();
		}
	}

	function OnTriggerExit (collider : Collider){
		if (collider.CompareTag("Enemy")){
			RemoveLockableEnemies(collider.transform);
			print ("Dead or too far");
		}
	}

	function UpdateLockableEnemies (){
		var tmpArray : Transform[] = [];
		for (var trs : Transform in transformToLock){
			if (trs != null){
				tmpArray += [trs];
			}
		}
		transformToLock = tmpArray;
	}

	function OnParticleCollision (weaponObject : GameObject){
		if (weaponObject.CompareTag("EnemyWeapon")){
			if (bDamaged){
				TakeDamage(10);
			}
		}
	}

	function TakeDamage (amount : int){
		if (bDamaged){
			bDamaged = false;
			currentHealth -= amount * 1.0;
			UIManager.Instance().UpdateLifeGear(currentHealth);
			if (currentHealth <= 0){
				Respawn();
				return;
			}
			yield WaitForSeconds (0.5);
			bDamaged = true;
		}
	}

	function RemoveLockableEnemies (removedEnemyTrs : Transform){
		if (transformToLock.Length == 1){
			transformToLock = [];
			enemyLockedIdx = -1;
			return;
		}
		var tmpTrs : Transform[] = transformToLock;
		transformToLock = [];
		for (var trs in tmpTrs){
			if (trs != removedEnemyTrs){
				transformToLock += [trs];
			}
		}
	}

	function UpdateLockableEnemiesIdx (){
		if (transformToLock.Length > 0){
			if (enemyLockedIdx + 1 >= transformToLock.Length){
				enemyLockedIdx = 0;
			}
			else {
				enemyLockedIdx++;
			}
		}
	}

	function TakeOff (){
		animator.SetTrigger("TakeOff");
		levelEnd = true;
	}
}