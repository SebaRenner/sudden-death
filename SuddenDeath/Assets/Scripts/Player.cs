using System;
using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
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

    public int kills;
    public int deaths;

    [SerializeField]
    private int maxHealth = 100;

    // Annotation SyncVar to synchronize health from server to all clients
    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(100, "its e me ");
        }
        
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
        if (sourcePlayer != null) {
            sourcePlayer.kills++;
            GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        deaths++;

        // disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        // disable gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        // disable collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        // Spawn the death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        Debug.Log(transform.name + " is dead!");

        // switch camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            // disable UI elements crosshair, healthbar and ammo
            for (int i = 0; i <= 2; i++)
            {
                GetComponent<PlayerSetup>().playerUIInstance.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

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

        // enable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        // enable gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        // enable collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }

        // switch camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            // Re-Activate UI elements crosshair, healthbar and ammo
            for (int i = 0; i <= 2; i++)
            {
                GetComponent<PlayerSetup>().playerUIInstance.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

    }

    public float GetHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }

    public void AddHealth(int amount)
    {
        if (currentHealth + amount < maxHealth)
        {
            currentHealth += amount;
        }
        else
        {
            currentHealth = maxHealth;
        }
    }
}
