using UnityEngine;

[System.Serializable]
public class PlayerWeapon : MonoBehaviour
{
    public string weaponName;
    public int damage;
    public float range;
    public float fireRate;
    public int maxBullets;
    public float reloadTime;

    [HideInInspector]
    public int bullets;

    public GameObject graphics;

}
