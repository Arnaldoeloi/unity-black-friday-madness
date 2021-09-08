using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggProjectile : MonoBehaviour
{
    public AudioSource createdSound;
    public AudioSource collisionSound;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(createdSound, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        //Instantiate(collisionSound, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        //createdSound.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit "+ collision.collider.name +" with an impulse of "+ collision.impulse.magnitude);

        if (collision.collider.GetComponent<PlayerScore>())
        {
            Debug.Log("Should drop item on floor of "+ collision.collider.name);
            collision.collider.GetComponent<PlayerScore>().DropRandomItem();
        }
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        //collisionSound.GetComponent<AudioSource>().Play();

        //Destroy(createdSound);
        //Destroy(collisionSound);
        //Instantiate(explosionPrefab, position, rotation);
        Destroy(gameObject);
    }
}
