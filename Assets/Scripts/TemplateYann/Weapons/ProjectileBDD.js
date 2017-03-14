#pragma strict

import UnityEngine.UI;

@script CreateAssetMenu (fileName ="Projectile_", menuName = "New Projectile")

class ProjectileBDD extends ScriptableObject {
	
	var projectileName : String;
	var projectileIcon : Sprite;
	var disabledIcon : Sprite;
	var projectileDescription : String;
	var damagesAmount : int;
}