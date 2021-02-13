using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string name = "Submachine Gun";
    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 0f;

    public GameObject graphics;
}
