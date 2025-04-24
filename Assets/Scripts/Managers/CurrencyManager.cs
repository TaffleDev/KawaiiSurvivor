using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;


    [Header("Elements")]
    [field: SerializeField] public int Currency { get; private set; }



    [Header("Actions")]
    public static Action onUpdated;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
    private void Add500Currency()
    {
        AddCurrency(500);
    }

    public void AddCurrency(int amout)
    {
        Currency += amout;
        UpdateText();

        onUpdated?.Invoke();
    }

    public void UseCurrency(int price) => AddCurrency(-price);
    private void UpdateText()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyTexts)
        {
            text.UpdateText(Currency.ToString());
        }
    }

    public bool HasEnoughCurrency(int price)
    {
        return Currency >= price;
    }
}
