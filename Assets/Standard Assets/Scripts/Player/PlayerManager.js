#pragma strict

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
		if (Input.GetButtonDown("RB")){
			TriggerWaterConsumption();
			animator.SetTrigger("MeleeAttack");
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
			if (Input.GetAxis("LeftAnalogHorizontal") > 0){
				transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
			}
			else if (Input.GetAxis("LeftAnalogHorizontal") < 0){
				transform.Rotate(-Vector3.up * Time.deltaTime * rotateSpeed);
			}
		}
		if (Input.GetButtonUp("RB") || (!Input.GetButton("RB") && isShooting)){
			isShooting = false;
			animator.ResetTrigger("MeleeAttack");
            animator.SetTrigger("StopAttack");
            for (var water : GameObject in GameObject.FindGameObjectsWithTag("WaterDamage")){
            	Destroy(water);
            }
		}
	}

	function Respawn (){
		transform.position = spawnPoint;
		vieIdx = PlayerPrefs.GetInt("Lives",vieIdx);
		for (var i = -1; i < vieIdx; i++){
			vies[i+1].gameObject.SetActive(true);
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
	}
}