using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BumpyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Header("Elements")]
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        
        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, new Vector2(1.1f, .9f), .6f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        
        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, Vector2.one, .6f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        
        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, Vector2.one, .6f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);
    }

    
}
