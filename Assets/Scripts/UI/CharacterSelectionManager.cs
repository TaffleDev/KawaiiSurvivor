using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Tabsil.Sijil;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Elements ")]
    [SerializeField] private Transform characterSelectionParent;
    [SerializeField] private CharacterButton characterSelectionPrefab;
    [SerializeField] private Image centerCharacterImage;
    [SerializeField] private CharacterInfoPanel characterInfo;

    
    [Header("Data")] 
    private CharacterDataSO[] characterDatas;
    private List<bool> unlockedStates = new List<bool>();
    private const string UnlockedStatesKey = "unlockedStatesKey";
    private const string LastSelectedCharacterKey = "LastSelectedCharacterKey";

    [Header("Settings")]
    private int selectedCharacterIndex;
    private int lastSelectedCharacterIndex;

    [Header("Actions")]
    public static Action<CharacterDataSO> OnCharacterSelected;
    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterInfo.Button.onClick.RemoveAllListeners();
        characterInfo.Button.onClick.AddListener(PurchaseSelectedCharacter);
        
        CharacterSelectedCallback(lastSelectedCharacterIndex);
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
        CharacterDataSO characterData = characterDatas[index];

        CharacterButton characterButtonInstance = Instantiate(characterSelectionPrefab, characterSelectionParent);
        characterButtonInstance.Configure(characterData.Sprite, unlockedStates[index]);
        
        characterButtonInstance.Button.onClick.RemoveAllListeners();
        characterButtonInstance.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        selectedCharacterIndex = index;
        
        CharacterDataSO characterData = characterDatas[index];

        if (unlockedStates[index])
        {
            lastSelectedCharacterIndex = index;
            characterInfo.Button.interactable = false;
            Save();
            
            OnCharacterSelected?.Invoke(characterData);
        }
        else
        {
            characterInfo.Button.interactable =
                CurrencyManager.instance.HasEnoughPremiumCurrency(characterData.PurchasePrice);
        }
        
        centerCharacterImage.sprite = characterData.Sprite;
        characterInfo.Configure(characterData, unlockedStates[index]);
        
    }

    private void PurchaseSelectedCharacter()
    {
        int price = characterDatas[selectedCharacterIndex].PurchasePrice;
        CurrencyManager.instance.UsePremiumCurrency(price);
        
        // save the unlocked state of the character
        unlockedStates[selectedCharacterIndex] = true;
        // Update the concerned character visuals
        
        characterSelectionParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().Unlock();
        // Update the character info
        CharacterSelectedCallback(selectedCharacterIndex);
        
        Save();
    }

    public void Load()
    {
        characterDatas = ResourcesManager.Characters;

        for (int i = 0; i < characterDatas.Length; i++)
            unlockedStates.Add(i == 0);

        if (Sijil.TryLoad(this, UnlockedStatesKey, out object unlockedStatesObject))
            unlockedStates = (List<bool>)unlockedStatesObject;
        
        if(Sijil.TryLoad(this, LastSelectedCharacterKey, out object lastSelectedCharacterObject))
            lastSelectedCharacterIndex = (int)lastSelectedCharacterObject;
        
        Initialize();
        
        // CharacterSelectedCallback(lastSelectedCharacterIndex);

    }

    public void Save()
    {
        Sijil.Save(this, UnlockedStatesKey, unlockedStates);
        Sijil.Save(this, LastSelectedCharacterKey, lastSelectedCharacterIndex);
    }
}
