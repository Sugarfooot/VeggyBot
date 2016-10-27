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
	}

	function Update () {
		if (Input.GetButtonDown("RB")){
			TriggerWaterConsumption();
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
		}
	}

	function Respawn (){
		transform.position = spawnPoint;
		// !! AJOUTER LE CHARGEMENT DES PLAYERPREFS AU MOMENT DU SPAWN !!
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

	function TriggerWaterConsumption (){
		yield WaitForSeconds (0.1);
		isShooting = true;
	}

	function OnTriggerEnter (collider : Collider){
		if (collider.CompareTag("Damage")){
			vieIdx --;
			vies[vieIdx].gameObject.SetActive(false);
			if(vieIdx == -1){
				Respawn();
			}
		}
	}
}