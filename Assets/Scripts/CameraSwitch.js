var cams : GameObject[];
var keys : String[];
private var mainCam : GameObject;

function Start() {
	yield WaitForSeconds(Time.deltaTime);
	mainCam = Camera.main.gameObject;
	mainCam.SetActive(false);
	DisableAllCamsButOne(cams[0]);
}

function Update() {
	for (var i = 0; i < keys.Length; i++){
		if (Input.GetKeyDown(keys[i])){
			DisableAllCamsButOne(cams[i]);
		}
	}

	if (Input.GetKeyDown("m")){
		DisableAllCams();
		Camera.main.gameObject.SetActive(true);
	}
}

function DisableAllCams (){
	for (var i = 0; i < cams.Length; i++){
		cams[i].SetActive(false);
	}
}

function DisableAllCamsButOne (cam : GameObject){
	for (var i = 0; i < cams.Length; i++){
		cams[i].SetActive(false);
	}
	cam.SetActive(true);
}