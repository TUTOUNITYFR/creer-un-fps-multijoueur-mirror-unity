using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private WeaponData theWeapon;

    [SerializeField]
    private float respawnDelay = 30f;

    private GameObject pickUpGraphics;
    private bool canPickUp;

    void Start()
    {
        ResetWeapon();
    }

    void ResetWeapon()
    {
        pickUpGraphics = Instantiate(theWeapon.graphics, transform);
        pickUpGraphics.transform.position = transform.position;
        canPickUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && canPickUp)
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            EquipNewWeapon(weaponManager);
        }
    }

    void EquipNewWeapon(WeaponManager weaponManager)
    {
        // Détruit l'arme actuelle du joueur
        Destroy(weaponManager.GetCurrentGraphics().gameObject);

        // Equipe la nouvelle arme
        weaponManager.EquipWeapon(theWeapon);

        canPickUp = false;
        Destroy(pickUpGraphics);

        StartCoroutine(DelayResetWeapon());
    }

    IEnumerator DelayResetWeapon()
    {
        yield return new WaitForSeconds(respawnDelay);
        ResetWeapon();
    }
}
