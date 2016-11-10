#pragma strict

function Start () {
	gameObject.tag = "Enemy";
}

function Update () {

}

function OnDestroy (){
	BonusManager.Instance().AddSavedVeggy();
}