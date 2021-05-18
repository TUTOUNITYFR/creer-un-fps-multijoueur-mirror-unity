using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardItem : MonoBehaviour
{

    [SerializeField]
    Text usernameText;

    [SerializeField]
    Text killsText;

    [SerializeField]
    Text deathsText;

    public void Setup(Player player)
    {
        usernameText.text = player.username;
        killsText.text = "Kills : " + player.kills;
        deathsText.text = "Deaths : " + player.deaths;
    }

}
