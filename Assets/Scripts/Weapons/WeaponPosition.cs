using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [Header("Elements")]
    public Weapon Weapon { get; private set; }
        
    public void AssignWeapon(Weapon weaponPrefab, int weaponLevel)
    {
        Weapon = Instantiate(weaponPrefab, transform);

        Weapon.transform.localPosition = Vector3.zero;
        Weapon.transform.localRotation = Quaternion.identity;

        Weapon.UpgradeTO(weaponLevel);
    }
}
