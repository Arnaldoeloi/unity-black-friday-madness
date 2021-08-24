using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage= 10f;
    public int maxAmmo = 30;
    public int currentAmmo = 30;
    public float range = 100f;

    public Vector3[] eggsPositions;

    public Camera fpsCam;
    // Start is called before the first frame update
    void Start()
    {
        eggsPositions = new Vector3[maxAmmo];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !PauseMenu.paused){
            Shoot();
        }
    }

    void Shoot(){
        if (currentAmmo > 0){
            currentAmmo--;
            RaycastHit hit;
            if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
                Debug.Log(hit.transform.name);
            }
        }
    }

    public void Reload(int ammoAmount)
    {
        if (currentAmmo + ammoAmount > maxAmmo) currentAmmo = maxAmmo;
        else currentAmmo = currentAmmo + ammoAmount;
    }
}


