using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [Header("Elements")]
    public Weapon Weapon { get; private set; }
        
    public void AssignWeapon(Weapon weapon, int weaponLevel)
    {
        weapon = Instantiate(weapon, transform);

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        weapon.UpgradeTO(weaponLevel);
    }
}
