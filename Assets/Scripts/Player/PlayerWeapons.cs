using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private WeaponPosition[] weaponPositions;

   
    public bool TryAddWeapon(WeaponDataSO weapon, int level)
    {
        if (weaponPositions == null)
        {
            Debug.LogError("Weapon Position Array is null");
            return false;
        }


        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon != null)
                continue;

            weaponPositions[i].AssignWeapon(weapon.Prefab, level);
            return true;

        }

        return false;
    }

    public void RecycleWeapons(int weaponIndex)
    {
        
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (i != weaponIndex)
                continue;


            int recyclePrice = weaponPositions[i].Weapon.GetRecyclePrice();
            CurrencyManager.instance.AddCurrency(recyclePrice);
            
            weaponPositions[i].RemoveWeapon();
            
            return;
        }
        
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();

        foreach (WeaponPosition weaponPosition in weaponPositions)
        {
            if (weaponPosition.Weapon == null)
                weapons.Add(null);
            else
                weapons.Add(weaponPosition.Weapon);

        }


        return weapons.ToArray();
    }
}
