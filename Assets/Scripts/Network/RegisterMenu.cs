using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegisterMenu : MonoBehaviour
{
    public InputField email;
    public InputField name;
    public InputField nickname;
    public InputField password;

    public Text message;

    public string authRegisterUrl = "http://localhost:8000/api/auth/register";


    public void Register()
    {
        StartCoroutine(PostRegisterForm());
    }


    IEnumerator PostRegisterForm()
    {
        WWWForm form = new WWWForm();
        string emailValue = email.text;
        string passwordValue = password.text;
        string nameValue = name.text;
        string nicknameValue = nickname.text;

        form.AddField("email", emailValue);
        form.AddField("password", passwordValue);
        form.AddField("nickname", nicknameValue);
        form.AddField("name", nameValue);

        UnityWebRequest www = UnityWebRequest.Post(authRegisterUrl, form); ;
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            RegisterResponseError res = JsonUtility.FromJson<RegisterResponseError>(www.downloadHandler.text);
            message.text = "The email has already been taken.";
            

            message.color = Color.red;
            

            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            RegisterResponseSuccess res = JsonUtility.FromJson<RegisterResponseSuccess>(www.downloadHandler.text);
            //LoginResponseSuccess res = JsonConvert.DeserializeObject<LoginResponseSuccess>(www.downloadHandler.text);
            


            message.text = res.message;
            message.color = Color.green;

            Debug.Log(res.user.email);

            PlayerPrefs.SetInt("playerId", res.user.id);
            PlayerPrefs.SetString("playerName", res.user.name);
            PlayerPrefs.SetString("playerNickname", res.user.nickname);
            PlayerPrefs.SetString("playerToken", res.token);





            Debug.Log(www.downloadHandler.text);
            Debug.Log("Form upload complete!");

            SceneManager.LoadScene("MainLobby");
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [System.Serializable]
    public class RegisterResponseSuccess
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
    public class Error
    {
        public List<string> email;
        public List<string> name;
        public List<string> password;
        public List<string> nickname;
    }

    [System.Serializable]
    public class RegisterResponseError
    {
        public string status;
        public string message;
        public Error error;
    }

}
