using System;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] 
    private int amount = 20;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponentInParent<Player>();
        player.AddHealth(amount);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.rotation.Set(0, 90 * Time.deltaTime, 0, transform.rotation.w);
    }
}
