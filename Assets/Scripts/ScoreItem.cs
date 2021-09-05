using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItem : MonoBehaviour
{

    public int score = 10;
    public string nameItem = "Item";
    public AudioSource pickUpSound;

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
        Debug.Log(other.name);
        if (other.GetComponent<PlayerScore>())
        {
            PlayerScore playerScore = other.GetComponent<PlayerScore>();

            playerScore.itemsCollected.Add(this);
            playerScore.UpdateScore(score);
            pickUpSound.Play();
            transform.position = new Vector3(0,-100,0);
            //Destroy(gameObject);
        }
    }

}
