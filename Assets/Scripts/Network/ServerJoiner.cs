using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

using WebSocketSharp;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate () {
            OnClick(param);
        });
    }
}

public class ServerJoiner : MonoBehaviour
{
    WebSocket ws;
    public string wsAddress = "ws://localhost:8081";


    public string nickname;
    public int userId;

    [SerializeField] public Server[] servers;

    public GameObject serverButtonTemplate;
    public GameObject serverListParent;

    //public serverList;


    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

        nickname = PlayerPrefs.GetString("playerNickname");
        userId = PlayerPrefs.GetInt("playerId");

        this.initializeWsConnection();
        //InvokeRepeating("UpdateChat", 0.0f, 0.1f);
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

    public void SpawnServerButtons()
    {
        Debug.Log("SpawnServerButtons");

        ClearServerButtonList();

        GameObject g;

        int N = servers.Length;

        for (int i = 0; i < N; i++)
        {

            g = Instantiate(serverButtonTemplate, transform);
            g.transform.SetParent(serverListParent.transform);

            Vector3 buttonScale = g.transform.localScale;
            buttonScale.x = 1;
            buttonScale.y = 1;
            buttonScale.z = 1;
            g.transform.localScale = buttonScale;
            
            g.transform.GetChild(0).GetComponent<Text>().text = servers[i].serverName;
            g.transform.GetChild(1).GetComponent<Text>().text = servers[i].hostNickname;
            g.transform.GetChild(2).GetComponent<Text>().text = "1/"+servers[i].maxPlayers.ToString();
            g.transform.GetChild(3).GetComponent<Text>().text = servers[i].serverTimeLimit.ToString();


            /*g.GetComponent <Button> ().onClick.AddListener (delegate() {
				ItemClicked (i);
			});*/
            g.GetComponent<Button>().AddEventListener(i, serverClicked);
        }

    }
    void serverClicked(int itemIndex)
    {
        Debug.Log("------------item " + itemIndex + " clicked---------------");
        PlayerPrefs.SetString("serverType", "client");
        PlayerPrefs.SetString("serverIp", servers[itemIndex].serverIp );
        PlayerPrefs.SetInt("serverPort", servers[itemIndex].serverPort);

        SceneManager.LoadScene("Mechanics");
        //Debug.Log("name " + servers[itemIndex].serverName);
    }

    public void ClearServerButtonList()
    {
        Debug.Log(serverListParent.transform.childCount);
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[serverListParent.transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in serverListParent.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }

        Debug.Log(serverListParent.transform.childCount);
    }

    private void initializeWsConnection()
    {
        var syncContext = System.Threading.SynchronizationContext.Current;

        Debug.Log("Attempting websockets connection. ");
        ws = new WebSocket(wsAddress);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            if (e.Data.Contains("availableGames"))
            {
                EventList res = JsonUtility.FromJson<EventList>(e.Data);
                Debug.Log(res.payload.games[0]);

                servers = res.payload.games.ToArray();

                // On socket listener thread
                syncContext.Post(_ =>
                {
                    // This code here will run on the main thread
                    SpawnServerButtons();
                }, null);
                //Invoke("SpawnServerButtons", 0);
                //Debug.Log(res.servers[0]);
            }

        };
    }


    public void JoinGame()
    {

        PlayerPrefs.SetString("serverType", "client");
        SceneManager.LoadScene("Mechanics");
    }


    [System.Serializable]
    public struct Server
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
        public Server payload;
    }

    [System.Serializable]
    public class ServerEventList
    {
        public List<Server> games;
    }
    [System.Serializable]
    public class EventList
    {
        public string eventName;
        public ServerEventList payload;
    }


    public string createServerEventToJson(CreateServerEvent createServerEvent)
    {
        return JsonUtility.ToJson(createServerEvent);
    }

    public string createServerListToJson(EventList createServerEventList)
    {
        return JsonUtility.ToJson(createServerEventList);
    }


}
