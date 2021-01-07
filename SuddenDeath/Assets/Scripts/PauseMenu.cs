using UnityEngine;
using Mirror;

public class PauseMenu : MonoBehaviour
{
    public static bool isOn = false;
    private Player player;
    private PlayerUI playerUI;

    private NetworkManager networkManager;
    
    private void Start()
    {
        networkManager = NetworkManager.singleton;
        playerUI = transform.parent.gameObject.GetComponent<PlayerUI>();
        player = playerUI.GetPlayer();

    }

    public void LeaveRoom()
    {
        string _playerID = player.name;
        GameManager.UnRegisterPlayer(_playerID);
        networkManager.StopHost();
        Destroy(GameObject.Find("PlayerUI"));
    }
}
