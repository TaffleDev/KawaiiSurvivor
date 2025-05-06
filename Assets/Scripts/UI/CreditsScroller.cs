using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CreditsScroller : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private float scrollSpeed;
    private RectTransform rt;


    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rt.anchoredPosition = rt.anchoredPosition.With(y: 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rt.anchoredPosition += Vector2.up * (Time.deltaTime * scrollSpeed);
    }
}
