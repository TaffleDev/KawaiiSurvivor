using UnityEngine;
using System;
using Tabsil.Sijil;

public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    public static CurrencyManager instance;
    
    private const string PremiumCurrencyKey = "PremiumCurrency";

    [Header("Elements")]
    [field: SerializeField] public int Currency { get; private set; }
    [field: SerializeField] public int PremiumCurrency { get; private set; }



    [Header("Actions")]
    public static Action onUpdated;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        
        // AddPremiumCurrency(PlayerPrefs.GetInt(PremiumCurrencyKey, 100), false);

        Candy.onCollected += CandyCollectedCallBack;
        Cash.onCollected  += CashCollectedCallBack;
    }

    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallBack;
        Cash.onCollected -= CashCollectedCallBack;
    }
    public void Load()
    {
        if (Sijil.TryLoad(this, PremiumCurrencyKey, out object premiumCurrencyValue))
        {
            AddPremiumCurrency((int)premiumCurrencyValue,false);
        }
        else {
            AddPremiumCurrency(100, false);
        }
    }

    public void Save()
    {
        Sijil.Save(this, PremiumCurrencyKey, PremiumCurrency);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [NaughtyAttributes.Button]
    private void Add500Currency() => AddCurrency(500);

    [NaughtyAttributes.Button]
    private void Add500PremiumCurrency() => AddPremiumCurrency(500);
    

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateVisuals();
    }
    public void AddPremiumCurrency(int amount, bool save = true)
    {
        PremiumCurrency += amount;
        UpdateVisuals();

		// PlayerPrefs.SetInt(PremiumCurrencyKey, PremiumCurrency);

    }

    private void UpdateVisuals()
    {
        UpdateText();

        onUpdated?.Invoke();
        Save();
    }

    
    private void UpdateText()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyTexts)
        {
            text.UpdateText(Currency.ToString());
        }
        
        PremiumCurrencyText[] premiumCurrencyTexts = FindObjectsByType<PremiumCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (PremiumCurrencyText text in premiumCurrencyTexts)
        {
            text.UpdateText(PremiumCurrency.ToString());
        }
        
    }
    #region Use, Check and Add Currency
    // Using Currency
    // Using Premium Currency
    // Check if you have enough Currency
    // Check if you have enough premium Currency
    // Call back to add Currency when picked up while playing
    //Call back to add Premium Currency when it drops doing gameplay

    public void UseCurrency(int price) => AddCurrency(-price);
    public void UsePremiumCurrency(int price) => AddPremiumCurrency(-price);
    public bool HasEnoughCurrency(int price) =>  Currency >= price;
    public bool HasEnoughPremiumCurrency(int price) =>  PremiumCurrency >= price;
    private void CandyCollectedCallBack(Candy candy) => AddCurrency(1);
    private void CashCollectedCallBack(Cash cash) => AddPremiumCurrency(1);
    #endregion

    
}
