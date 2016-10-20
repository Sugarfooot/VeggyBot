#pragma strict

import UnityEngine.UI;

public class TutorialManager extends MonoBehaviour {

	//Tutorial variables
	var tutorialText : Text;
	var actionText : Text;
	var tutorialImage : Image;
	private var tutorialString : String = "";
	private var displayDelay : float = 0.0;
	private var canJetInTuto : boolean = false;
	private var canMoveInTuto : boolean = false;
	
	private static var instance : TutorialManager;
	public static function Instance () : TutorialManager {
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
		JetInTuto (false);
		MoveInTuto(true);
	}

	function Update () {

	}

	function DefineTutorialTextImageAndDelay (thisText : String, delay : float, thisSprite : Sprite){
		StopCoroutine("DisplayAndHideTutorialSection");
		CleanTutorialText();
		CleanTutorialImage();
		tutorialString = thisText;
		if (thisSprite != null){
			tutorialImage.sprite = thisSprite;
		}
		displayDelay = delay;
		StartCoroutine("DisplayAndHideTutorialSection");
	}

	function DisplayAndHideTutorialSection (){
		tutorialText.text = tutorialString;
		tutorialText.gameObject.GetComponent.<LocalisedText>().setNewText(tutorialString);
		if (tutorialImage.sprite != null){
			tutorialImage.enabled = true;
		}
		yield WaitForSeconds (displayDelay);
		CleanTutorialText();
		CleanTutorialImage();
	}

	function DisplayActionText (thisText : String){
		actionText.text = thisText;
		actionText.gameObject.GetComponent.<LocalisedText>().setNewText(thisText);
	}

	function CleanActionText (){
		actionText.text = "";
	}

	function CleanTutorialText (){
		tutorialText.text = "";
	}

	function CleanTutorialImage (){
		tutorialImage.enabled = false;
		tutorialImage.sprite = null;
	}

	function GetTutorialInstructions (){
		return tutorialText.text;
	}

	//Can player use the water jet
	function JetInTuto (state : boolean){
		canJetInTuto = state;
	}

	function CanJetInTuto (){
		return canJetInTuto;
	}

	//Can plyer move in this part of the tutorial
	function MoveInTuto (state : boolean){
		canMoveInTuto = state;
	}

	function CanMoveInTuto (){
		return canMoveInTuto;
	}
}