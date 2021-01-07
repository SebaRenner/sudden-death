using UnityEngine;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SyncVar]
    public string username = "Loading...";

    [SerializeField]
    private int maxHealth = 100;

    // Annotation SyncVar to synchronize health from server to all clients
    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        // TODO: Hacky solution in my eyes
        this.username = transform.name;
        SetDefaults();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(100);
        }
        */
    }


    // Annotation to send update to all clients conncected to the server
    [ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourceID)
    {
        if (isDead) return;
        currentHealth -= _amount;
        Debug.Log(transform.name + " new has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die(_sourceID);
        }

    }

    private void Die(string _sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if(sourcePlayer != null) {
            GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }
        //GameManager.instance.onPlayerKilledCallback.Invoke("Player", "Notme");

        // disabled components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        Debug.Log(transform.name + " is dead!");


        // Call respawn method
        StartCoroutine(Respawn());

    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        

        Debug.Log(transform.name + " respawned");

    }


    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }

    }

    public float GetHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }

}
