using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Selector : MonoBehaviour
{

    [SerializeField] private XRBaseInteractor[] interactors;
    [SerializeField] private InputActionReference aActionRef;
    private IPropertiesChangable hoveredPart;
    private IPropertiesChangable selectedPart;

    public event Action<IPropertiesChangable> OnPartSelected;

    void OnEnable()
    {
        
        aActionRef.action.performed += OnSelectPressed;

        foreach (var interactor in interactors)
        {
            interactor.hoverEntered.AddListener(OnHoverEntered);
            interactor.hoverExited.AddListener(OnHoverExited);
        }
    }

    void OnDisable()
    {
        aActionRef.action.performed -= OnSelectPressed;
        aActionRef.action.Disable();

        foreach (var inter in interactors)
        {
            inter.hoverEntered.RemoveListener(OnHoverEntered);
            inter.hoverExited.RemoveListener(OnHoverExited);
        }
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        var part = args.interactableObject.transform.GetComponent<IPropertiesChangable>();
        if (part == null) return;

        hoveredPart = part;
        hoveredPart.Highlight(true);
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        var part = args.interactableObject.transform.GetComponent<IPropertiesChangable>();
        if (part == null) return;

        part.Highlight(false);
        if (hoveredPart == part) hoveredPart = null;
    }

    void OnSelectPressed(InputAction.CallbackContext ctx)
    {

        Debug.Log($"OnSelectPressed {this}");
        if (hoveredPart == null) return;

        
        //if (selectedPart != null) selectedPart.Highlight(false);

        
        selectedPart = hoveredPart;
        selectedPart.Highlight(true);

        OnPartSelected?.Invoke(selectedPart);
    }
}
