using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;


    [Header("Elements")]
    [field: SerializeField] public int Currency { get; private set; }

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

    public void AddCurrency(int amout)
    {
        Currency += amout;
        UpdateText();
    }

    private void UpdateText()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyTexts)
        {
            text.UpdateText(Currency.ToString());
        }
    }
}
