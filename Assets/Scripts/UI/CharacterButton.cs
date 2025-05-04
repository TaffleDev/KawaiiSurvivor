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
    public void Configure(Sprite characterIcon, bool unlocked)
    {
        characterImage.sprite = characterIcon;
        
        if (unlocked)
            Unlock();
        else 
            Lock();
    }

    public void Lock()
    {
        lockObject.SetActive(true);
        characterImage.color = Color.grey;

    }
    
    public void Unlock()
    {
        lockObject.SetActive(false);
        characterImage.color = Color.white;
    }
}
