using UnityEngine;
using Mirror;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    private bool isReloading = false;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetWeaponGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.Log("No WeaponGraphics component on the weapon object: " + _weaponIns.name);
        }

        if (isLocalPlayer)
        {
            //_weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
            Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public void Reload()
    {
        if (isReloading) return;

        StartCoroutine(Reload_Coroutine());
    }

    private IEnumerator Reload_Coroutine()
    {
        Debug.Log("Reloading...");

        isReloading = true;

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.bullets = currentWeapon.maxBullets;

        isReloading = false;
    }
  
}
