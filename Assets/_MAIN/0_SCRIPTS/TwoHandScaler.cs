using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Zenject;

[RequireComponent(typeof(XRGrabInteractable))]
public class TwoHandScaler : MonoBehaviour
{
    [SerializeField] private bool lockRotationWhileScaling = true;

    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor firstInteractor;
    private XRBaseInteractor secondInteractor;

    private float initialDistance;
    private Vector3 initialScale;

    // Cached grab settings
    private bool initialMatchAttachRotation;
    private bool initialMatchAttachPosition;
    private bool initialTrackRotation;
    private bool initialTrackPosition;

    public Transform origParent;

    private Cancelator _cancelator;  // todo как его получить по Zenject после инстанцирования нового префаба



    [Inject]
    public void Construct(Cancelator cancelator)
    {
        //_cancelator = cancelator;
    }


    private void Awake()
    {
        origParent = transform.parent;
        _cancelator = FindFirstObjectByType<Cancelator>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);

        initialMatchAttachRotation = grabInteractable.matchAttachRotation;
        initialMatchAttachPosition = grabInteractable.matchAttachPosition;
        initialTrackRotation = grabInteractable.trackRotation;
        initialTrackPosition = grabInteractable.trackPosition;
    }

    private void OnHoverExited(HoverExitEventArgs arg0)
    {
       
    }

    private void OnHoverEntered(HoverEnterEventArgs arg0)
    {
        origParent = transform.parent;
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        var interactor = args.interactorObject as XRBaseInteractor;
        if (firstInteractor == null)
        {
            _cancelator.RegisterObject(this.gameObject); 
            firstInteractor = interactor;
        }
        else if (secondInteractor == null)
        {
            secondInteractor = interactor;
            BeginTwoHandedScaling();
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        var interactor = args.interactorObject as XRBaseInteractor;

        if (interactor == secondInteractor)
        {
            secondInteractor = null;
            grabInteractable.matchAttachPosition = initialMatchAttachPosition;
            //grabInteractable.trackRotation = initialTrackRotation;
            //grabInteractable.trackPosition = initialTrackPosition;
        }
        else if (interactor == firstInteractor)
        {
            firstInteractor = secondInteractor;
            secondInteractor = null;
            grabInteractable.matchAttachPosition = initialMatchAttachPosition;
            //grabInteractable.trackRotation = initialTrackRotation;
            //grabInteractable.trackPosition = initialTrackPosition;
        }

        if (firstInteractor == null && secondInteractor == null)
        {
            EndTwoHandedScaling();
        }
    }

    private void BeginTwoHandedScaling()
    {
        initialScale = transform.localScale;
        initialDistance = Vector3.Distance(
            firstInteractor.transform.position,
            secondInteractor.transform.position
            );

        grabInteractable.matchAttachPosition = false;
        if (lockRotationWhileScaling)
            grabInteractable.matchAttachRotation = false;
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
    }

    private void Update()
    {
        if (firstInteractor != null && secondInteractor != null)
        {
            float currentDistance = Vector3.Distance(
                firstInteractor.transform.position,
                secondInteractor.transform.position
            );
            float scaleFactor = currentDistance / initialDistance;
            transform.localScale = initialScale * scaleFactor;
        }
    }
    private void EndTwoHandedScaling()
    {
        grabInteractable.matchAttachPosition = initialMatchAttachPosition;
        grabInteractable.matchAttachRotation = initialMatchAttachRotation;
        grabInteractable.trackPosition = initialTrackPosition;
        grabInteractable.trackRotation = initialTrackRotation;
        initialDistance = 0f;
        initialScale = transform.localScale;
    }
}
