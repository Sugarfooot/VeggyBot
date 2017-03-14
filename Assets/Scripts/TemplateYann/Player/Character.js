#pragma strict

import UnityEngine.UI;

@script CreateAssetMenu (fileName ="Character_", menuName = "New Character")

class Character extends ScriptableObject {
	
	enum Skills {Construction = 0, Dexterity = 1, TreasureHunting = 2, Wooden = 3, Electric = 4, Writing = 5, None = 6}

	var name : String;
	var description : String;
	var trombi : Sprite;
	var maxSoul : float;
	var maxIntuition : float;
	var skill : Skills = 0;
	var projectileCandy : ProjectileBDD;
	var dartDamage : int = 5;
	var marbleDamage : int = 18;
	var plasticHammerDamage : int = 12;
	var plasticDaggersDamage : int = 7;
	var shurikenDamage : int = 9;
	var candiesOnFire : boolean = false;
	var throwingOnFire : boolean = false;
}