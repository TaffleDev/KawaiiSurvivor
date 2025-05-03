using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Elements ")]
    [SerializeField] private Transform characterSelectionParent;
    [SerializeField] private CharacterButton characterSelectionPrefab;
    [SerializeField] private Image centerCharacterImage;
    
    [Header("Data")] 
    private ChatacterDataSO[] characterDatas;

    private void Awake()
    {
        characterDatas = ResourcesManager.Characters;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Initialize()
    {
        for (int i = 0; i < characterDatas.Length; i++)
        {
            CreateCharacterButton(i);
        }
    }

    private void CreateCharacterButton(int index)
    {
        ChatacterDataSO characterData = characterDatas[index];

        CharacterButton characterButtonInstance = Instantiate(characterSelectionPrefab, characterSelectionParent);
        characterButtonInstance.Configure(characterData.Sprite);
        
        characterButtonInstance.Button.onClick.RemoveAllListeners();
        characterButtonInstance.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        centerCharacterImage.sprite = characterDatas[index].Sprite;
    }
}
