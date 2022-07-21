using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using WebSocketSharp;


public class ServerJoiner : MonoBehaviour
{
    WebSocket ws;
    public string wsAddress = "ws://localhost:8081";


    public string nickname;
    public int userId;


    //public serverList;
    

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

        nickname = PlayerPrefs.GetString("playerNickname");
        userId = PlayerPrefs.GetInt("playerId");

        this.initializeWsConnection();
        //InvokeRepeating("UpdateChat", 0.0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RequestARefresh()
    {
        //string message = chatMessage.text;
        //chatMessage.text = "";
        CreateServerEvent serverEvent = new CreateServerEvent();
        //Payload payload;

        serverEvent.eventName = "listServers";

        ws.Send(createServerEventToJson(serverEvent));
    }

    private void initializeWsConnection()
    {
        Debug.Log("Attempting websockets connection. ");
        ws = new WebSocket(wsAddress);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            if (e.Data.Contains("availableGames"))
            {
                CreateServerEventList res = JsonUtility.FromJson<CreateServerEventList>(e.Data);
                Debug.Log(res.servers[0]);
            }

        };
    }


    public void JoinGame()
    {

        PlayerPrefs.SetString("serverType", "client");
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

    [System.Serializable]
    public class CreateServerEventList
    {
        public List<Payload> servers;
    }


    public string createServerEventToJson(CreateServerEvent createServerEvent)
    {
        return JsonUtility.ToJson(createServerEvent);
    }

    public string createServerListToJson(CreateServerEventList createServerEventList)
    {
        return JsonUtility.ToJson(createServerEventList);
    }


}
