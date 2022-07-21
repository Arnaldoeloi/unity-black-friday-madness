using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;


public class DEPNetworkManager : MonoBehaviour
{

    WebSocket ws;

    public int userId;
    public string serverName;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.initializeWsConnection();

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendPlayerPosition", 0.0f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initializeWsConnection()
    {
        Debug.Log("Attempting websockets connection. ");
        ws = new WebSocket("ws://localhost:8081"+"/userId="+this.userId+"?serverName="+serverName);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };
    }

    void SendPlayerPosition()
    {
        Debug.Log("PlayerPosition" + playerPositionInJson());
        ws.Send("{\"eventName\":\"playerMovement\", \"payload\":{\"playerId\":\""+userId.ToString()+"\", \"position\":"+playerPositionInJson()+"}}");
    }

    public string playerPositionInJson()
    {
        return JsonUtility.ToJson(player.transform.position);
    }
}
