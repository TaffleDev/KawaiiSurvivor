using UnityEngine;

public static class ResourcesManager
{
    const string statIconsDataPath = "Data/Stat Icons";
    const string objectsDataPath = "Data/Objects/";
    const string weaponDataPath = "Data/Weapons/";
    const string characterDataPath = "Data/Charaters/";

    private static StatIcon[] statIcons;

    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {
            StatIconDataSO data = Resources.Load<StatIconDataSO>(statIconsDataPath);
            statIcons = data.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
            if (stat ==  statIcon.stat)
                return statIcon.icon;

        Debug.LogError("No Icon found for stat : " + stat);

        return null;
    }

    private static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get 
        {
            if (objectDatas == null)
            {
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectsDataPath);
            } 

            return objectDatas;
        }
        private set { }
    }

    public static ObjectDataSO GetRandomObject()
    {
        return Objects[Random.Range(0, Objects.Length)];
    }




    private static WeaponDataSO[] weaponDatas;
    public static WeaponDataSO[] Weapons
    {
        get
        {
            if (weaponDatas == null)
            {
                weaponDatas = Resources.LoadAll<WeaponDataSO>(weaponDataPath);
            }

            return weaponDatas;
        }
        private set { }
    }

    public static WeaponDataSO GetRandomWeapon()
    {
        return Weapons[Random.Range(0, Weapons.Length)];
    }
    
    
    
    
    
    private static CharacterDataSO[] characterDatas;
    public static CharacterDataSO[] Characters
    {
        get
        {
            if (characterDatas == null)
            {
                characterDatas = Resources.LoadAll<CharacterDataSO>(characterDataPath);
            }

            return characterDatas;
        }
        private set { }
    }
    

}
