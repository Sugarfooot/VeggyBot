#pragma strict

import UnityEngine.SceneManagement;
import UnityEngine.UI;

@Tooltip ("Niveau à charger quand le joueur clique sur JOUER")
var levelToLoad : String;

function Start () {

}

function Update () {

}

function LoadFirstLevel (){
	SceneManager.LoadScene(levelToLoad);
}

function DisplaySettingsMenu (){

}

function LoadGame (){

}

function ExitGame (){
	
}