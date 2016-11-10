#pragma strict

import UnityEngine.UI;
import UnityEngine.SceneManagement;

var pauseMenu : GameObject;
var pauseCursorsParent : Transform;
var cursorInterval : float = 0.2;
private var cursorIdx : int = -1;
private var canMoveCursor : boolean = false;

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