#pragma strict

import UnityEngine.UI;
import UnityEngine.SceneManagement;

@Tooltip("Do you need to unlock the robot")
var unlockRobot : boolean = true;
// @Tooltip("Do you need to unlock hacks")
// var unlockHacks : boolean = false;
@Tooltip("Do you need to unlock the water jet")
var unlockJet : boolean = false;
// @Tooltip("Does this step trigger timer when completed")
// var launchTimer : boolean = false;
// @Tooltip("0=Batteries, 1=Run, 2=Flags, 3=ZoneCapture, 4=Search&Hack, 5=Loot, 6=SaveOrTrash, 7=FollowTarget, 8=FleeTarget, 9=RedLight,GreenLight")
// var startObjective : int = -1;
@Tooltip("Do some UI  (or something else) need to be shown when this step has been completed")
var elementToActivate : GameObject[];

@Tooltip("A tip to remind the player which action he has to perform in order to trigger the next tutorial step. Leave empty if no action is required")
var actionText : String = "";

@Tooltip("By doing one of these actions...")
var byEnteringTrigger : boolean = false;
// var launchTimerAtTrigger : boolean = false;
var byHittingAKey : boolean = false;
var justText : boolean = false;
var keysToPress : String[];

@Tooltip("The next step to trigger")
var nextStep : TutorialBlock;

@Tooltip("... the instructions for the next step are displayed")
var instructions : String;
var helperIcon : Sprite;
var associatedSprite : SpriteRenderer;
@Tooltip("How long will the instructions be displayed. 0 = infinite until an action is taken")
var displayTimeInstructions : float;

@Tooltip("Do some UI  (or something else) need to be hidden when this step has been completed")
var elementToDeactivate : GameObject[];
// @Tooltip("Does this step end current objective")
// var endObjective : boolean = false;
// @Tooltip("Does this step reset timer when completed")
// var resetTimer : boolean = false;
// @Tooltip("Does this step pause timer when completed")
// var pauseTimer : boolean = false;
// @Tooltip("Does this step add a point to blue score when completed")
// var winAPoint : boolean = false;
@Tooltip("Do you need to lock the robot to explain the next part")
var lockRobot : boolean = false;
@Tooltip("Do you need to lock the water jet")
var lockJet : boolean = false;

@Tooltip("Does this step end the current level")
var nextLevel : String = "";

//Is this step being executed
private var isBeingExecuted : boolean = false;

function Awake (){

}

function Start () {
	isBeingExecuted = true;
	if (actionText != ""){
		TutorialManager.Instance().DisplayActionText(actionText);
	}
// 	if(unlockHacks){
// 		TutorialManager.Instance().HackInTuto(true);
// 		Main_UIManager.Instance().UnlockHacks();
// 	}
	if (unlockRobot){
		TutorialManager.Instance().MoveInTuto(true);
		// TimerManager.Instance().GetLocalPlayer().GetComponent.<ControlPlayer>().UnlockPlayer();		// A ADAPTER A INVECTOR !!
	}
	if (unlockJet){
		TutorialManager.Instance().JetInTuto(true);
		// TimerManager.Instance().GetLocalPlayer().GetComponent.<Skills>().UnlockSkills();				// A ADAPTER A INVECTOR !!
	}
// 	if (launchTimer){
// 		TimerManager.Instance().UnpauseChrono();
// 	}
// 	if (startObjective != -1 && !endObjective){
// 		ObjectivesManager.Instance().SetObjective(startObjective);
// 	}
	if (elementToActivate.Length > 0){
		for (var i = 0; i < elementToActivate.Length; i++){
			elementToActivate[i].SetActive(true);
		}
	}
	if (justText && isBeingExecuted){
		if (displayTimeInstructions > 0.0){
			TutorialManager.Instance().DefineTutorialTextImageAndDelay (instructions, displayTimeInstructions, helperIcon);
		}
		if (nextStep.actionText != ""){
			yield WaitForSeconds(displayTimeInstructions);
		}
		else{
			yield WaitForSeconds(displayTimeInstructions + 1);
		}
		ValidateStep();
	}
}

function Update () {

	//Checks if a key is pressed to validate the step
	if (byHittingAKey && isBeingExecuted){
		for (var i = 0; i < keysToPress.Length; i++){
			if (Input.GetKeyDown (keysToPress[i])){
				ValidateStep ();
			}
		}
	}
}

//Executed when player enters the trigger
function OnTriggerEnter (collider : Collider){
	if (byEnteringTrigger && isBeingExecuted){
		if (collider.CompareTag("Player")){
			// if (launchTimerAtTrigger){
			// 	TimerManager.Instance().UnpauseChrono();
			// }
			ValidateStep ();
		}
	}
}

function ValidateStep (){
	//Checks if a sprite was used in the room (or elsewhere) to lead the player, and deactivate it.
	if (associatedSprite != null){
		associatedSprite.enabled = false;
	}
	isBeingExecuted = false;
	TutorialManager.Instance().CleanActionText();

	//Deactivates the collider of this step if it has not been used in Key-press event
	if (!byHittingAKey && !justText){
		GetComponent.<Collider>().enabled = false;
	}

	if (elementToDeactivate.Length > 0){
		for (var j = 0; j < elementToDeactivate.Length; j++){
			elementToDeactivate[j].SetActive(false);
		}
	}

// 	if (resetTimer){
// 		TimerManager.Instance().ResetTimer();
// 		yield WaitForSeconds(Time.deltaTime);
// 	}
// 	if (pauseTimer){
// 		TimerManager.Instance().PauseChrono();
// 	}
// 	if (winAPoint){
// 		ScoreManager.Instance().WhoWinByTag(0);
// 	}
	if (lockRobot){
		TutorialManager.Instance().MoveInTuto(false);
		// TimerManager.Instance().GetLocalPlayer().GetComponent.<ControlPlayer>().LockPlayer();		// A ADAPTER A INVECTOR !!
	}
// 	if (lockHacks){
// 		TutorialManager.Instance().HackInTuto(false);
// 		Main_UIManager.Instance().LockHacks();
// 	}
	if (lockJet){
		TutorialManager.Instance().JetInTuto(false);
		// TimerManager.Instance().GetLocalPlayer().GetComponent.<Skills>().LockSkills();				// A ADAPTER A INVECTOR !!
	}
// 	if (endObjective && startObjective == -1){
// 		ObjectivesManager.Instance().SetObjective(startObjective);
// 	}
// 	else if (endObjective && startObjective != -1){
// 		startObjective = -1;
// 		ObjectivesManager.Instance().SetObjective(startObjective);
// 	}

	// Displays the instructions block (text + icon)
	if (!justText){
		if (displayTimeInstructions > 0.0){
			TutorialManager.Instance().DefineTutorialTextImageAndDelay (instructions, displayTimeInstructions, helperIcon);
			if (nextStep != null && nextStep.actionText != ""){
				yield WaitForSeconds(displayTimeInstructions);
			}
			else{
				yield WaitForSeconds(displayTimeInstructions + 1);
			}
		}
	}

// 	if (pickAHack){
// 		TutorialManager.Instance().MoveInTuto(true);
// 		Main_UIManager.Instance().HackSelection();
// 	}

	//If this event doesn't trigger this level's ending, triggers the next step...
	if (nextLevel == ""){
		if (nextStep != null){
			nextStep.TriggerBlock();
		}
	}
	// ...or else, ends the current level.
	else if (nextLevel != ""){
		EndLevel();
	}
}

//Enables the next block
function TriggerBlock (){
	gameObject.SetActive(true);
}

// Back to Home page
function EndLevel (){
	yield WaitForSeconds (3);
	SceneManager.LoadScene(nextLevel);
}