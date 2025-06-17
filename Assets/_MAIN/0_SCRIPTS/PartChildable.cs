using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Zenject;

public class PartChildable : XRGrabInteractable
{
    [SerializeField] InputActionReference aButtonReference;
    private Transform parentTransform = null;


    private void OnTriggerEnter(Collider other)
    {

        //ebug.Log($"OnTriggerEnter {this}");
        if (other.CompareTag("DronParent"))
        {
            //Debug.Log($"DroneParent enter {this}");
            parentTransform  = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"OnTriggerExit");
        if (other.CompareTag("DronParent"))
        {
            //Debug.Log($"DroneParent exit {this}");
            parentTransform = null;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (parentTransform != null)
        {
            transform.parent = parentTransform;
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
    }
}
