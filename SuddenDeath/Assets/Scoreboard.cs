using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField]
    GameObject playerScoreboardItem;

    [SerializeField]
    Transform playerList;

    void OnEnable()
    {
        Player[] players = GameManager.GetAllPlayers();

        foreach (Player player in players)
        {
            GameObject itemGO = (GameObject)Instantiate(playerScoreboardItem, playerList);
            PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
            if (item != null)
            {
                item.Setup(player.username, player.kills, player.deaths);
            }
        }

    }

    void OnDisable()
    {
        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
    }


}
