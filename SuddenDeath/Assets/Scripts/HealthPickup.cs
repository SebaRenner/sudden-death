using System;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] 
    private int amount = 20;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player.GetHealthPct() < 1f)
        {
            player.AddHealth(amount);
            Destroy(gameObject);
        } 
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 25f * Time.deltaTime);
    }
}
