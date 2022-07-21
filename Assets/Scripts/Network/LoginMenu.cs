using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    public InputField email;
    public InputField password;

    public Text message;

    public string authLoginUrl = "http://localhost:8000/api/auth/login";


    public void Login()
    {
        StartCoroutine(PostLoginForm());
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    IEnumerator PostLoginForm()
    {
        WWWForm form = new WWWForm();
        string emailValue = email.text;
        string passwordValue = password.text;

        form.AddField("email", emailValue);
        form.AddField("password", passwordValue);

        UnityWebRequest www = UnityWebRequest.Post(authLoginUrl, form); ;
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            LoginResponseError res = JsonUtility.FromJson<LoginResponseError>(www.downloadHandler.text);
            message.text = res.message;
            message.color = Color.red;
            Debug.Log(res.message);

            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            LoginResponseSuccess res = JsonUtility.FromJson<LoginResponseSuccess>(www.downloadHandler.text);
            //LoginResponseSuccess res = JsonConvert.DeserializeObject<LoginResponseSuccess>(www.downloadHandler.text);
            message.text = res.message;
            message.color = Color.green;
            
            Debug.Log(res.message);
            Debug.Log(www.downloadHandler.text);

            PlayerPrefs.SetInt("playerId", res.user.id);
            PlayerPrefs.SetString("playerName", res.user.name);
            PlayerPrefs.SetString("playerNickname", res.user.nickname);
            PlayerPrefs.SetString("playerToken", res.token);

            Debug.Log("Form upload complete!");

            SceneManager.LoadScene("MainLobby");
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [System.Serializable]
    public class LoginResponseSuccess
    {
        public string status;
        public User user;
        public string message;
        public string token;
    }

    [System.Serializable]
    public class User
    {
        public int id;
        public string email;
        public string nickname;
        public string name;
    }

    [System.Serializable]
    public class LoginResponseError
    {
        public bool status;
        public string message;
    }


}
