using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string name = "Glock";
    public int damage = 5;
    public float range = 100f;
    public float fireRate = 10f;
    public int maxBullets = 30;
    public float reloadTime = 1.5f;

    [HideInInspector]
    public int bullets;

    public GameObject graphics;

    public PlayerWeapon()
    {
        bullets = maxBullets;
    }

}
