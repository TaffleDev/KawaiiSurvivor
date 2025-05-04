using System;
using UnityEngine;


[RequireComponent (typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Components")]
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private SpriteRenderer playerRenderer;
    private PlayerHealth playerHealth;
    private PlayerLevel  playerLevel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerHealth = GetComponent<PlayerHealth>();
        playerLevel  = GetComponent<PlayerLevel>();
        
        CharacterSelectionManager.OnCharacterSelected += CharacterSelectedCallback;

    }

    

    private void OnDestroy()
    {
        CharacterSelectionManager.OnCharacterSelected -= CharacterSelectedCallback;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + playerCollider.offset;
    }

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
    
    private void CharacterSelectedCallback(CharacterDataSO characterData)
    {
        playerRenderer.sprite = characterData.Sprite;
    }
}
