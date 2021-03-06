using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent (typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    private bool isOnCooldown = false;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();

    }

    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn) return;

        if (currentWeapon.bullets < currentWeapon.maxBullets)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                weaponManager.Reload();
                return;
            }
        }

        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (isOnCooldown) return;
                Shoot();
                if (currentWeapon.weaponName == "Sniper")
                {
                    StartCoroutine(SniperCooldown());
                }
            }
        } else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            } else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }

      
    }

    public void RemoteCancelInvoke()
    {
        CancelInvoke("Shoot");
    }

    private IEnumerator SniperCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(2f);
        isOnCooldown = false;
    }
    
    // is called on the server when a player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    // is called on all client when we need to do a shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetWeaponGraphics().muzzleFlash.Play();
    }

    // is called on the server when we hit something
    // takes the hit point and the noraml of the surface we hit
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    // is called on all clients
    // Here spawn the effects 
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetWeaponGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
    }

    [Client]
    void Shoot()
    {

        if (!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }

        if (currentWeapon.bullets <= 0)
        {
            weaponManager.Reload();
            return;
        }

        currentWeapon.bullets--;

        // We are shooting, call the OnShoot on the server
        CmdOnShoot();

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                var playerId = _hit.collider.gameObject.GetComponentInParent<Player>().gameObject.name;
                CmdPlayerShot(playerId, currentWeapon.damage, transform.name);
            }

            CmdOnHit(_hit.point, _hit.normal);
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage, string _sourceID)
    {
        Player _player = GameManager.instance.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }
}
