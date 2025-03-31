using UnityEngine;

public class ColourHolder : MonoBehaviour
{
    public static ColourHolder instance;


    [Header("Elemetns")]
    [SerializeField] private PaletteSO palette;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    
    public static Color GetColour(int level)
    {
        level = Mathf.Clamp(level, 0, instance.palette.LevelColours.Length);

        return instance.palette.LevelColours[level];
    }

    public static Color GetOutlineColour(int level)
    {

        level = Mathf.Clamp(level, 0, instance.palette.LevelOutlineColours.Length);
        return instance.palette.LevelOutlineColours[level];
    }
}
