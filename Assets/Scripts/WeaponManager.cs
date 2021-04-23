using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private WeaponData primaryWeapon;

    private WeaponData currentWeapon;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(WeaponData _weapon)
    {
        currentWeapon = _weapon;

        GameObject weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if(currentGraphics == null)
        {
            Debug.LogError("Pas de script WeaponGraphics sur l'arme : " + weaponIns.name);
        }

        if(isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

}
