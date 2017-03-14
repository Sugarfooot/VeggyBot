#pragma strict

var possiblesPops : Enemy[];
var numberOfPops : int;
var poppingGap : float = 1.5;

function Start () {

}

function Update () {

}

function StartPopping (){
	for (var i = 0; i < numberOfPops; i++){
		var rndEnemy = Random.Range(0,possiblesPops.Length);
		Instantiate (possiblesPops[rndEnemy].gameModel, transform.position, transform.rotation);
		yield WaitForSeconds(poppingGap);
	}
}