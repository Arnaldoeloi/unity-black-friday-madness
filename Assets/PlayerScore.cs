using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerNameText;
    public int score = 0;
    public string playerName = "Player";

    public AudioSource painSound;

    public Text matchLastUpdateText;

    public List<ScoreItem> itemsCollected;

    public void Awake()
    {
        playerNameText.text = playerName;
    }

    public void UpdateScore(int n)
    {
        score += n;
        if (score < 0) score = 0; //failsafe
        UpdateText();
    }

    public void UpdateText()
    {
        scoreText.text = score.ToString();
    }

    public int GetPoints()
    {
        return score;
    }

    public void DropRandomItem()
    {
        painSound.Play();
        if(itemsCollected.Count >= 1)
        {
            int itemIndex = Random.Range(0, (itemsCollected.Count - 1));

            matchLastUpdateText.text = playerName + " dropped " + itemsCollected[itemIndex].nameItem + " worth $" + itemsCollected[itemIndex].score;
            //Debug.Log("Dropping item "+itemsCollected[itemIndex].nameItem + ". Should deduct "+ itemsCollected[itemIndex].score +" from score.");  
            
            float randomZ = Random.Range(-10f, 10f); 
            float randomX = Random.Range(-10f, 10f);

            UpdateScore(-itemsCollected[itemIndex].score);

            Vector3 dropItemPosition = new Vector3(transform.position.x + randomX, 8.3f, transform.position.z + randomZ);

            itemsCollected[itemIndex].transform.position = dropItemPosition;
            itemsCollected.RemoveAt(itemIndex);
        }
    }
}
