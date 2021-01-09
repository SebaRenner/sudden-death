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
    private Animator animator;

    private GameObject _weaponIns;

    public bool isReloading = false;

    void Start()
    {
       // EquipWeapon(primaryWeapon);
        animator = GetComponent<Animator>();
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetWeaponGraphics()
    {
        return currentGraphics;
    }

    public void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        SetMaxBullets();
        _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.transform);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.Log("No WeaponGraphics component on the weapon object: " + _weaponIns.name);
        }

        if (isLocalPlayer)
        {
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
        
        CmdOnReload();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        SetMaxBullets();

        isReloading = false;
    }

    public void SetMaxBullets()
    {
        if (currentWeapon != null)
        {
            currentWeapon.bullets = currentWeapon.maxBullets;
        }
    }

    public void SetNewWeapon(PlayerWeapon _weapon)
    {
        Destroy(_weaponIns);
        EquipWeapon(_weapon);
    }

    public void SetBaseWeapon()
    {
        Destroy(_weaponIns);
        EquipWeapon(primaryWeapon);
    }

    
    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        animator.SetTrigger("Reload");
    }
}
