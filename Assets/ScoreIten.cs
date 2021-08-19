using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIten : MonoBehaviour
{

    public int score = 10;
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
        if (other.GetComponent<MainPlayer>())
        {

            other.GetComponent<MainPlayer>().UpdateScore(score);

            Destroy(gameObject);
        }
    }

}
