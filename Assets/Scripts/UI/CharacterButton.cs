using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class CharacterButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject lockObject;
    
    public Button Button
    {
        get
        {
            return GetComponent<Button>(); }
        
        private set{}
    }
    public void Configure(Sprite characterIcon)
    {
        characterImage.sprite = characterIcon;
        
        
    }
}
