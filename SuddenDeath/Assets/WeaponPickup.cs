using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    [SerializeField]
    PlayerWeapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponentInParent<Player>();
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        if (weaponManager.GetCurrentWeapon() != weapon)
        {
            weaponManager.SetNewWeapon(weapon);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 25f * Time.deltaTime);
    }
}
