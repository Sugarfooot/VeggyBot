using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AIManager : MonoBehaviour {

	public float damageAmount;

	[System.Serializable]
	public class Damage
	{
	    [Tooltip("Apply damage to the Character Health")]
	    public int value = 15;
	    [Tooltip("Apply damage even if the Character is blocking")]
	    public bool ignoreDefense;
	    [Tooltip("Activated Ragdoll when hit the Character")]
	    public bool activeRagdoll;
	    [HideInInspector] public Transform sender;
		[HideInInspector] public Vector3 hitPosition;
	    [HideInInspector] public int recoil_id =1;
	   	public string attackName;

		public Damage (int value, bool ignoreDefense, bool activeRagdoll, Transform sender, int recoil_ID,Vector3 hitPosition , string attackName = "")
	    {
	        this.value = value;
	        this.ignoreDefense = ignoreDefense;
	        this.activeRagdoll = activeRagdoll;
	        this.sender = sender;
	        this.recoil_id = recoil_ID;
	        this.attackName = attackName;
			this.hitPosition = hitPosition;
	    }

	    public Damage(Damage damage)
	    {
	        this.value = damage.value;
	        this.ignoreDefense = damage.ignoreDefense;
	        this.activeRagdoll = damage.activeRagdoll;
	        this.sender = damage.sender;
	        this.recoil_id = damage.recoil_id;
			this.attackName = damage.attackName;
			this.hitPosition = damage.hitPosition;
	    }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendDamage (float amount){
		SendMessage("TakeWaterDamage", amount);
	}
}
