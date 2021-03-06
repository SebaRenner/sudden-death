using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    private NetworkManager networkManager;

    public void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        } else
        {

            // Disable player graphics for local player 
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // Create player ui 
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            // Configure PlayerUI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No playerui component on playerui prefab");
            }
            ui.SetPlayer(GetComponent<Player>());

        }

        networkManager = NetworkManager.singleton;
        string username = networkManager.GetComponent<NetworkManagerHUD>().GetUsername();

        GetComponent<Player>().Setup();

        CmdSetUsername(transform.name, username);

    }

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        RpcSetUsername(playerID, username);
    }

    [ClientRpc]
    void RpcSetUsername(string playerID, string username)
    {
        Player player = GameManager.instance.GetPlayer(playerID);
        if (player != null)
        {
            player.username = username;
        }
    } 

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.instance.RegisterPlayer(_netID, _player);
    }

    void DisableComponents ()
    {
        foreach (Behaviour b in componentsToDisable)
        {
            b.enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }


    public void onDisable()
    {
        Destroy(playerUIInstance);

        GameManager.instance.SetSceneCameraActive(true);
        GameManager.instance.UnRegisterPlayer(transform.name);
    }
}
