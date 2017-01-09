using UnityEngine;
using System.Collections;

public class WaterDamage : MonoBehaviour {

	public float damageAmount;
	public bool damaging = true;

	// Use this for initialization
	void Start () {
		damaging = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision (GameObject go){
		if (go.CompareTag("Enemy")){
			if (damaging){
				go.GetComponent<AIManager>().SendDamage(damageAmount);
				CD();
			}
		}
	}

	IEnumerator CD (){
		damaging = false;
		yield return new WaitForSeconds(1.0f);
		damaging = true;
	}
}
