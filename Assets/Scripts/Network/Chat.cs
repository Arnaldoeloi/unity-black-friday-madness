using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using WebSocketSharp;



public class Chat : MonoBehaviour
{
    WebSocket ws;
    public string wsAddress = "ws://localhost:8081";

    public string nickname;
    public int userId;

    public Text textMessage;

    public InputField chatMessage;

    public string lastMsg = null;



    


    // Start is called before the first frame update
    void Start()
    {
        nickname = PlayerPrefs.GetString("playerNickname");
        userId = PlayerPrefs.GetInt("playerId");

        this.initializeWsConnection();
        InvokeRepeating("UpdateChat", 0.0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendMessageToLobby()
    {
        string message = chatMessage.text;
        chatMessage.text = "";
        ws.Send("{\"eventName\":\"playerMessage\", \"payload\":{\"playerId\":\"" + userId.ToString() + "\", \"nickname\":\"" + nickname + "\", \"message\":\"" + message + "\"}}");
    }

    private void UpdateChat()
    {
        if (lastMsg == null) return;
        textMessage.text = textMessage.text + "\n" + lastMsg;
        Debug.Log(textMessage.text);
        lastMsg = null;
    }


    private void initializeWsConnection()
    {
        Debug.Log("Attempting websockets connection. ");
        ws = new WebSocket(wsAddress);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            if (e.Data.Contains("playerMessage"))
            {
                LobbyMessageEvent res = JsonUtility.FromJson<LobbyMessageEvent>(e.Data);
                string msgTxt = "<color=green>" + res.payload.nickname + "</color>: " + res.payload.message;
                lastMsg = msgTxt;
            }

        };
    }

    [System.Serializable]
    public class Payload
    {
        public string playerId;
        public string nickname;
        public string message;
    }

    [System.Serializable]
    public class LobbyMessageEvent
    {
        public string eventName;
        public Payload payload;
    }


}
