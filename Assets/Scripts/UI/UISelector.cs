using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelector : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private EventSystem eventSystem;

    private void Awake()
    {
        UIManager.OnPanelShown += PanelShownCallback;
    }

    private void OnDestroy()
    {
        UIManager.OnPanelShown -= PanelShownCallback;
        
    }

    private void PanelShownCallback(Panel panel)
    {
        if (panel.FirstSelectedObject != null)
            SetSelectedGameObjects(panel.FirstSelectedObject);
    }

    private void SetSelectedGameObjects(GameObject go) => eventSystem.SetSelectedGameObject(go);
}
