﻿using UnityEngine;
using System.Collections;

public class vMagicPower : MonoBehaviour
{
    public vMeleeWeapon weapon;
    public bool rotateBySpawnPoint;
    public Transform spawnPoint;
    public Transform waterParent;
    public GameObject particle;
    public bool oneShot;
    public float timeToInvoke;

    private bool isActive;
    private vMeleeManager manager;
    private float time;

    void Start()
    {
        manager = GetComponentInParent<vMeleeManager>();
        time = timeToInvoke;
    }
    
    void LateUpdate()
    {
        if (weapon != null && weapon.isActive && !isActive)
        {
            if (oneShot && Input.GetButtonDown("RB"))
            {
                isActive = true;
                Invoke("DoEffect", timeToInvoke);
            }
            else if (time < 0)            
                DoEffect();            
            else
                time -= Time.deltaTime;
        }
        else if (isActive && weapon != null && !weapon.isActive && Input.GetButtonUp("RB"))
        {
            time = timeToInvoke;
            isActive = false;
        }
    }

    void DoEffect()
    {
        manager = GetComponentInParent<vMeleeManager>();
        if (manager != null && manager.applyDamage)
        {
            var go = Instantiate(particle, spawnPoint.position, rotateBySpawnPoint ? spawnPoint.rotation : manager.transform.rotation) as GameObject;
            if (go != null)
            {
                go.transform.SetParent(waterParent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.Euler(0,90,0);
                go.SendMessage("SetHitProperties", manager.hitProperties, SendMessageOptions.DontRequireReceiver);
                go.SendMessage("SetSender", manager.transform, SendMessageOptions.DontRequireReceiver);
            }        
        }       
    }

}
