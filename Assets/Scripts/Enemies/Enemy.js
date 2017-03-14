#pragma strict

import UnityEngine.UI;

@script CreateAssetMenu (fileName ="Enemy_", menuName = "New Enemy")

class Enemy extends ScriptableObject {
	
	var enemyName : String;
	var gameModel : GameObject;
	var description : String;
	var maxSoul : int;
	var maxIntuition : int;
	var amountWhenDead : int = 100;
	var amountWhenFixed : int = 15;
}