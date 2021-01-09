using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour
{

    [SerializeField]
    PlayerWeapon weapon;

    [SerializeField]
    private float respawnTime = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponentInParent<Player>();
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        if (weaponManager.GetCurrentWeapon() != weapon)
        {
            weaponManager.SetNewWeapon(weapon);
            StartCoroutine(Respawn());
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 25f * Time.deltaTime);
    }

    private IEnumerator Respawn()
    {
        Deactive();
        yield return new WaitForSeconds(respawnTime);
        Activate();
    }

    private void Activate()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void Deactive()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }



}
