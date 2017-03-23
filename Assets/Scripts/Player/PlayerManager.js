#pragma strict

import UnityEngine.UI;

public class PlayerManager extends MonoBehaviour {

	var waterIndicators : Image[];
	private var isShooting : boolean = false;
	private var spawnPoint : Vector3;
	private var waterLevel : float = 0;
	var shootConsumptionSpeed : float = 0.1;
	var vies : Image[];
	private var vieIdx : int = 0;
	static var maxVieIdx : int = 0;
	private var animator : Animator;
	var rotateSpeed : float = 20.0;
	private var levelEnd : boolean = false;
	var hose : ParticleSystem;
	private var transformToLock : Transform[] = []; 
	private var enemyLockedIdx : int = -1;
	
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
		vieIdx = maxVieIdx;
		animator = GetComponent.<Animator>();
	}

	function Update () {

		if (levelEnd){
			return;
		}

		if (Input.GetButtonDown("RB")){
			TriggerWaterConsumption();
			animator.SetBool("Attack", true);
		}
		if (Input.GetButton("RB")){
			if (waterLevel < 0){
				waterLevel = 0;
			}
			if (isShooting && waterLevel > 0){
				waterLevel -= shootConsumptionSpeed * Time.deltaTime;
				for (var i = 0; i < waterIndicators.Length; i++){
					waterIndicators[i].fillAmount = waterLevel;
				}
			}
			// if (Input.GetAxis("LeftAnalogHorizontal") > 0){
			// 	transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
			// }
			// else if (Input.GetAxis("LeftAnalogHorizontal") < 0){
			// 	transform.Rotate(-Vector3.up * Time.deltaTime * rotateSpeed);
			// }

			if (transformToLock.Length > 0){
				transform.LookAt(transformToLock[enemyLockedIdx]);
			}
		}
		if (Input.GetButtonUp("RB") || (!Input.GetButton("RB") && isShooting)){
			isShooting = false;
			UpdateLockableEnemiesIdx();
			// animator.ResetTrigger("MeleeAttack");
            animator.SetBool("Attack", false);
            hose.Stop();
            // animator.SetTrigger("StopAttack");
            // for (var water : GameObject in GameObject.FindGameObjectsWithTag("WaterDamage")){
            // 	Destroy(water);
            // }
		}
	}

	function Respawn (){
		transform.position = spawnPoint;
		vieIdx = PlayerPrefs.GetInt("Lives",vieIdx);
		if (vies.Length > 0){
			for (var i = -1; i < vieIdx; i++){
				vies[i+1].gameObject.SetActive(true);
			}
		}
	}

	function SetNewSpawnPoint (){
		spawnPoint = transform.position;
		PlayerPrefs.SetInt("Lives",vieIdx);
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

	function TriggerWaterConsumption (){
		yield WaitForSeconds (0.1);
		isShooting = true;
		hose.Play();
	}

	function OnTriggerEnter (collider : Collider){
		if (collider.CompareTag("Damage")){
			collider.enabled = false;
			vies[vieIdx].gameObject.SetActive(false);
			vieIdx --;
			if(vieIdx == -1){
				yield WaitForSeconds(0.5);
				Respawn();
				yield WaitForSeconds(Time.deltaTime);
				collider.enabled = true;
			}
		}
		if (collider.CompareTag("Enemy")){
			if (transformToLock.Length == 1){
				enemyLockedIdx = 0;
			}
			transformToLock += [collider.transform];
		}
	}

	function OnTriggerExit (collider : Collider){
		if (collider.CompareTag("Enemy")){
			UpdateLockableEnemies(collider.transform);
		}
	}

	function UpdateLockableEnemies (removedEnemyTrs : Transform){
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