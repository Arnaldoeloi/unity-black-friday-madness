using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public int amount = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MainPlayer>().gun)
        {
            Gun gun = other.GetComponent<MainPlayer>().gun;
            gun.Reload(amount);
            Destroy(gameObject);
        }
    }


}
