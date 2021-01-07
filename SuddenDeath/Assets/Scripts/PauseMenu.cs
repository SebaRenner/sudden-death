using UnityEngine;
using Mirror;

public class PauseMenu : MonoBehaviour
{
    public static bool isOn = false;

    private NetworkManager networkManager;

    private void Start()
    {
        Debug.Log("EndmEEEEEEEEEEEEEEE");
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        //string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Debug.Log("Hello");
        networkManager.StopClient();
    }

}
