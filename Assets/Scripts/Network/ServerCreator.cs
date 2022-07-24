using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ServerCreator : MonoBehaviour
{

    public string nickname;
    public int userId;

    public InputField serverNameInputField;
    public Dropdown maxPlayersDropdown;
    public Dropdown timeLimitDropdown;


    private string serverName;
    private int maxPlayers;
    private int timeLimit;


    // Start is called before the first frame update
    void Start()
    {
        nickname = PlayerPrefs.GetString("playerNickname");
        userId = PlayerPrefs.GetInt("playerId");

        //InvokeRepeating("UpdateChat", 0.0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendMessageToLobby()
    {
        //string message = chatMessage.text;
        //chatMessage.text = "";
        CreateServerEvent serverEvent;
        Payload payload;
        

        //serverEvent.eventName = "createServerEvent";

        //ws.Send("{\"eventName\":\"playerMessage\", \"payload\":{\"playerId\":\"" + userId.ToString() + "\", \"nickname\":\"" + nickname + "\", \"message\":\"" + message + "\"}}");
    }


    public void CreateGameAndHost()
    {
        serverName = serverNameInputField.text;
        maxPlayers = (int)maxPlayersDropdown.value;
        timeLimit = (int)timeLimitDropdown.value;

        PlayerPrefs.SetString("serverType", "host");
        PlayerPrefs.SetString("serverName", serverName);
        PlayerPrefs.SetInt("maxPlayers", int.Parse(maxPlayersDropdown.options[maxPlayers].text));
        PlayerPrefs.SetInt("timeLimit", int.Parse(timeLimitDropdown.options[timeLimit].text));
        
        SceneManager.LoadScene("Mechanics");
    }


    [System.Serializable]
    public class Payload
    {
        public int playerId;
        public string nickname;
        public string serverName;
        public int serverTimeLimit;
        public int maxPlayers;
        public string serverIp;
        public int serverPort;
    }

    [System.Serializable]
    public class CreateServerEvent
    {
        public string eventName;
        public Payload payload;
    }


}
