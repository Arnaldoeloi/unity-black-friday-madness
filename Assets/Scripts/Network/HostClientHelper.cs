using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;



using WebSocketSharp;


public class HostClientHelper : MonoBehaviour
{

    WebSocket ws;
    public string wsAddress = "ws://localhost:8081";

    public int maxPlayers = 2;



    // Start is called before the first frame update
    void Start()
    {
        initializeWsConnection();
        ServerSetup(); 

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ServerSetup()
    {
        UnityTransport netConfig = NetworkManager.Singleton.GetComponent<UnityTransport>();

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (PlayerPrefs.GetString("serverType") == "host")
            {
                NetworkManager.Singleton.StartHost();
                createServerOnList();
            }
            else if (PlayerPrefs.GetString("serverType") == "client")
            {

                string serverIp = PlayerPrefs.GetString("serverIp") != "" ? PlayerPrefs.GetString("serverIp") : "127.0.0.1";
                int serverPort = PlayerPrefs.GetInt("serverPort") != 0 ? PlayerPrefs.GetInt("serverPort") : 7777;

                netConfig.SetConnectionData(serverIp, (ushort)serverPort);
                NetworkManager.Singleton.StartClient();
            }
            else
            {
                NetworkManager.Singleton.StartHost();
                createServerOnList();
            }
        }
    }


    public void createServerOnList()
    {
        UnityTransport netConfig = NetworkManager.Singleton.GetComponent<UnityTransport>();

        CreateServerEvent createServerEvent = new CreateServerEvent();
        createServerEvent.eventName = "createNewServer";
        
        Payload payload = new Payload();

        maxPlayers = PlayerPrefs.GetInt("maxPlayers") != 0 ? PlayerPrefs.GetInt("maxPlayers") : 2;

        payload.playerId = PlayerPrefs.GetInt("playerId");
        payload.hostNickname = PlayerPrefs.GetString("playerNickname");
        payload.serverName = PlayerPrefs.GetString("serverName")!="" ? PlayerPrefs.GetString("serverName"): "Server";
        payload.serverTimeLimit = PlayerPrefs.GetInt("timeLimit") != 0 ? PlayerPrefs.GetInt("timeLimit") : 120;
        payload.maxPlayers = maxPlayers;
        payload.serverIp = netConfig.ConnectionData.Address;
        payload.serverPort = netConfig.ConnectionData.Port;

        createServerEvent.payload = payload;


        ws.Send(createServerEventToJson(createServerEvent));
    }


    private void initializeWsConnection()
    {
        Debug.Log("Attempting websockets connection. ");
        ws = new WebSocket(wsAddress+"/userId="+PlayerPrefs.GetInt("playerId")+"?name="+PlayerPrefs.GetString("playerName"+"?nickname="+PlayerPrefs.GetString("playerNickname")+"?serverName="+PlayerPrefs.GetString("serverName")));
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            //Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            if (e.Data.Contains("serverCreated"))
            {
                CreateServerEvent res = JsonUtility.FromJson<CreateServerEvent>(e.Data);
            }
            else if(e.Data.Contains("playerJoined"))
            {

            }

        };
    }

    [System.Serializable]
    public class Payload
    {
        public int playerId;
        public string hostNickname;
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

    public string createServerEventToJson(CreateServerEvent createServerEvent)
    {
        return JsonUtility.ToJson(createServerEvent);
    }
}
