#pragma strict

public class BonusManager extends MonoBehaviour {

	private var lifePartNbr : int = 0;
	private var savedVeggies : int = 0;
	private var maxVeggies : int = 0;

	private static var instance : BonusManager;
	public static function Instance () : BonusManager {
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

	function Start (){
		if (maxVeggies == 0){
			maxVeggies = GameObject.FindGameObjectsWithTag("Enemy").Length;
		}
	}

	function Update (){

	}

	function AddLifePart (){
		lifePartNbr ++;
		if (lifePartNbr > 4){
			lifePartNbr = 0;
		}
	}

	function GetLifePartNbr (){
		return lifePartNbr;
	}

	function AddSavedVeggy (){
		savedVeggies ++;
	}

	function GetMaxVeggies (){
		return maxVeggies;
	}

	function GetSavedVeggies (){
		return savedVeggies;
	}
}