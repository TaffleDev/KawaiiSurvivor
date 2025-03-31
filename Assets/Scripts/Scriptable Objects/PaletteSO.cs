using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "Scriptable Objects/New Palette", order = 0)]
public class PaletteSO : ScriptableObject
{
    [field: SerializeField] public Color[] LevelColours {  get; private set; }
    [field: SerializeField] public Color[] LevelOutlineColours {  get; private set; }
}
