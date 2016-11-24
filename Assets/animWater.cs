using UnityEngine;
using System.Collections;

public class liquidEnergyAnimC : MonoBehaviour {

	public ProceduralMaterial substance;
	public float _animWater = 1;

	void Start()
	{
		substance = GetComponent<Renderer>().sharedMaterial as ProceduralMaterial ;        
	}

	void LateUpdate()
	{        
		_animWater = _animWater + (2 * Time.smoothDeltaTime); 
		substance.SetProceduralFloat("animWater", _animWater);
		substance.RebuildTextures();
	}
}