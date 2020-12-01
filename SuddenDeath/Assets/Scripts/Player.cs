using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    // Annotation SyncVar to synchronize health from server to all clients
    [SyncVar]
    private int currentHealth;

    void Awake()
    {
        SetDefaults();
    }


    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;
        Debug.Log(transform.name + " new has " + currentHealth + " health.");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }

}
