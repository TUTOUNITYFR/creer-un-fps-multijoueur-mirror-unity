using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    public static string LoggedInUsername;

    public string lobbySceneName = "Lobby";

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LogIn(Text username)
    {
        LoggedInUsername = username.text;
        Debug.Log("Logged is as : " + LoggedInUsername);
        SceneManager.LoadScene(lobbySceneName);
    }

}
