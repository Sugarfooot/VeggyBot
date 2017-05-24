var cams : Transform;
var keys : String[];
private var mainCam : GameObject;

function Start() {
	yield WaitForSeconds(Time.deltaTime);
	mainCam = Camera.main.gameObject;
	mainCam.SetActive(false)
	DisableAllCamsButOne(cams.GetChild(0).gameObject);
}

function Update() {
	for (var i = 0; i < keys.Length; i++){
		if (Input.GetKeyDown(keys[i])){
			DisableAllCamsButOne(cams.GetChild(i).gameObject);
		}
	}

	if (Input.GetKeyDown("m")){
		DisableAllCams();
		Camera.main.gameObject.SetActive(true);
	}
}

function DisableAllCams (){
	for (var i = 0; i < cams.childCount; i++){
		cams.GetChild(i).gameObject.SetActive(false);
	}
}

function DisableAllCamsButOne (cam : GameObject){
	for (var i = 0; i < cams.childCount; i++){
		cams.GetChild(i).gameObject.SetActive(false);
	}
	cam.SetActive(true);
}