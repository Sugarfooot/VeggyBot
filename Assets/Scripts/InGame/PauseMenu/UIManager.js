#pragma strict

import UnityEngine.UI;
import UnityEngine.SceneManagement;

class UIManager extends MonoBehaviour{

	@Header("Vie")
	var vimage : Image;

	@Header("Réservoir")
	var tankImage : Image;
	var consumeSpeed : float;

	@Header("UI Générale")
	var pauseMenu : GameObject;
	var pauseCursorsParent : Transform;
	var cursorInterval : float = 0.2;
	var loadingScreen : GameObject;
	var loadingProgress : Image;
	private var cursorIdx : int = -1;
	private var canMoveCursor : boolean = false;

	@Header("End game")
	var nextLevel : String;

	private static var instance : UIManager;
	public static function Instance () : UIManager {
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
		cursorIdx = 0;
		// for (var i = 0; i < pauseCursorsParent.childCount; i++){
		// 	pauseCursorsParent.GetChild(i).gameObject.SetActive(false);
		// }
		// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
	}

	function Update () {
		if (Input.GetButtonDown("Start") || Input.GetKeyDown("escape")){
			TriggerPauseMenu();
		}

		if (canMoveCursor && pauseMenu.activeSelf){
			if (Input.GetAxis("LeftAnalogHorizontal") < 0){
				MoveCursorUp();
			}
			if (Input.GetAxis("LeftAnalogHorizontal") > 0){
				MoveCursorDown();
			}
			if (Input.GetButtonDown("A")){
				switch (cursorIdx){
					case 0:
						TriggerPauseMenu();
						break;
					case pauseCursorsParent.childCount - 1:
						TriggerPauseMenu();
						SceneManager.LoadScene("MenuAccueil");
						break;
				}
			}
		}
	}

	function FillWaterTank (){
		tankImage.fillAmount = 1.0;
	}

	function ConsumeWaterTank () : float{
		tankImage.fillAmount -= consumeSpeed;
		return tankImage.fillAmount;
	}

	function TriggerPauseMenu (){
		if (pauseMenu.activeSelf){
			canMoveCursor = false;
			pauseMenu.SetActive(false);
		}
		else if (!pauseMenu.activeSelf){
			cursorIdx = 0;
			pauseMenu.SetActive(true);
			canMoveCursor = true;
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Highlighted");
		}
	}

	function MoveCursorUp (){
		canMoveCursor = false;
		if (cursorIdx == 0){
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Normal");
			cursorIdx = pauseCursorsParent.childCount -1;
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Highlighted");
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
		}
		else {
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Normal");
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
			cursorIdx -= 1;
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Highlighted");
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
		}
		yield WaitForSeconds (cursorInterval);
		canMoveCursor = true;
	}

	function MoveCursorDown (){
		canMoveCursor = false;
		if (cursorIdx == pauseCursorsParent.childCount -1){
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Normal");
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
			cursorIdx = 0;
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Highlighted");
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
		}
		else {
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Normal");
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
			cursorIdx += 1;
			pauseCursorsParent.GetChild(cursorIdx).gameObject.GetComponent.<Animator>().SetTrigger("Highlighted");
			// pauseCursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
		}
		yield WaitForSeconds (cursorInterval);
		canMoveCursor = true;
	}

	function UpdateLifeGear (healthValue : float){
		vimage.fillAmount = healthValue / 100.0 ;
	}

	function LoadNextLevel (){
		loadingScreen.gameObject.SetActive(true);
		loadingProgress.fillAmount = SceneManager.LoadSceneAsync(nextLevel).progress;
	}
}