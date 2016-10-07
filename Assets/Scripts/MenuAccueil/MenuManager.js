#pragma strict

import UnityEngine.SceneManagement;
import UnityEngine.UI;

@Tooltip ("Niveau à charger quand le joueur clique sur JOUER")
var levelToLoad : String;

var cursorsParent : Transform;
var cursorInterval : float = 0.2;
private var cursorIdx : int = -1;
private var canMoveCursor : boolean = true;

function Start () {
	cursorIdx = 0;
	for (var i = 0; i < cursorsParent.childCount; i++){
		cursorsParent.GetChild(i).gameObject.SetActive(false);
	}
	cursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
}

function Update () {
	if (canMoveCursor){
		if (Input.GetAxis("LeftAnalogVertical") > 0){
			MoveCursorUp();
		}
		if (Input.GetAxis("LeftAnalogVertical") < 0){
			MoveCursorDown();
		}
		if (Input.GetButtonDown("A")){
			switch (cursorIdx){
				case 0:
					LoadFirstLevel();
					break;
				case cursorsParent.childCount - 1:
					ExitGame();
					break;
			}
		}
	}
}

function LoadFirstLevel (){
	SceneManager.LoadScene(levelToLoad);
}

function DisplaySettingsMenu (){

}

function LoadGame (){

}

function ExitGame (){
	Application.Quit();
}

function MoveCursorUp (){
	canMoveCursor = false;
	if (cursorIdx == 0){
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
		cursorIdx = cursorsParent.childCount -1;
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
	}
	else {
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
		cursorIdx -= 1;
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
	}
	yield WaitForSeconds (cursorInterval);
	canMoveCursor = true;
}

function MoveCursorDown (){
	canMoveCursor = false;
	if (cursorIdx == cursorsParent.childCount -1){
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
		cursorIdx = 0;
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
	}
	else {
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(false);
		cursorIdx += 1;
		cursorsParent.GetChild(cursorIdx).gameObject.SetActive(true);
	}
	yield WaitForSeconds (cursorInterval);
	canMoveCursor = true;
}