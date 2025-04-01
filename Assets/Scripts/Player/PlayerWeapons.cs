using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private WeaponPosition[] weaponPositions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWeapons(WeaponDataSO selectedWeapon, int weaponLevel)
    {
        //Debug.Log("We're Selected " + selectedWeapon.Name + " with level : " + weaponLevel);
        weaponPositions[Random.Range(0, weaponPositions.Length)].AssignWeapon(selectedWeapon.Prefab, weaponLevel);

    }
}
