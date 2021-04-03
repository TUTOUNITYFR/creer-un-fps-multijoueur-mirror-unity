using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public MatchSettings matchSettings;

    public static GameManager instance;

    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }

        Debug.LogError("Plus d'une instance de GameManager dans la scène");
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if(sceneCamera == null)
        {
            return;
        }

        sceneCamera.SetActive(isActive);
    }

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
}
