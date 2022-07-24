using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : NetworkBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerNameText;


    public NetworkVariable<int> score;

    public string displayName;

    public NetworkVariable<Unity.Collections.FixedString64Bytes> playerName = new NetworkVariable<Unity.Collections.FixedString64Bytes>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public AudioSource painSound;

    public Text matchLastUpdateText;

    public List<ScoreItem> itemsCollected;

    public TextMesh playerNameAbvHead;

    void Start()
    {
        score.Value = 0;
        if (IsOwner)
        {
            playerName.Value = PlayerPrefs.GetString("playerNickname");
        }
        displayName = playerName.Value.ToString();
    }

    public void OnNicknameChanged(Unity.Collections.FixedString64Bytes previous, Unity.Collections.FixedString64Bytes current)
    {

        Debug.Log("NicknameChanged: " + playerName.Value.ToString());
        displayName = current.ToString();
        if (playerNameAbvHead != null)
        {
            playerNameAbvHead.text = displayName;
        }
    }

    public override void OnNetworkSpawn()
    {

        playerName.OnValueChanged += OnNicknameChanged;

        Debug.Log("OnNetworkSpawn");
    }

    public void UpdateScore(int n)
    {
        score.Value += n;
        if (score.Value < 0) score.Value = 0; //failsafe
        UpdateText();
    }

    public void UpdateText()
    {
        if(scoreText!=null)
            scoreText.text = score.ToString();
    }

    public int GetPoints()
    {
        return score.Value;
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
