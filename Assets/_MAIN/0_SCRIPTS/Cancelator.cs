using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cancelator : MonoBehaviour
{
    private class CancelableObject 
    {
        public GameObject lastGrabbedObject;
        public Vector3 savedPosition;
        public Quaternion savedRotation;
        public Vector3 savedScale;
        public Transform savedParent;

        public CancelableObject(GameObject obj)
        {            
            lastGrabbedObject = obj;
            savedPosition = obj.transform.position;
            savedRotation = obj.transform.rotation;
            savedScale = obj.transform.localScale;
            savedParent = obj.GetComponent<TwoHandScaler>().origParent;
            Debug.Log($"Saved  {savedPosition} {savedRotation} {savedScale} parent {savedParent.name} ");
        }
    }

    private readonly int _maxSteps = 10;
    private readonly LinkedList<CancelableObject> _deque = new LinkedList<CancelableObject>();

    private Stack<CancelableObject> cancelationStack = new();
    public InputActionProperty cancelAction; // Назначается в инспекторе

    private void Awake()
    {
    }


    private void OnEnable()
    {
        cancelAction.action.performed += OnCancelPerformed;
        cancelAction.action.Enable();
    }

    private void OnDisable()
    {
        cancelAction.action.performed -= OnCancelPerformed;
        cancelAction.action.Disable();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        CancelLastOperation();
    }

    public void RegisterObject(GameObject obj)
    {
        if (_deque.Count >= _maxSteps)
            _deque.RemoveFirst();    
        _deque.AddLast(new CancelableObject(obj));       

        //cancelationStack.Push(new CancelableObject(obj));
        //Debug.Log($"message {obj.transform.position} {obj.transform.rotation} {obj.transform.localScale}");
    }

    private bool CancelLastOperation()
    {
        if (_deque.Count == 0)
        {            
            return false;
        }
        var cancelableObject = _deque.Last.Value;
        cancelableObject.lastGrabbedObject.transform.parent = cancelableObject.savedParent;
        cancelableObject.lastGrabbedObject.transform.SetPositionAndRotation(cancelableObject.savedPosition, cancelableObject.savedRotation);
        cancelableObject.lastGrabbedObject.transform.localScale = cancelableObject.savedScale;
        _deque.RemoveLast();
        return true;

        //if (cancelationStack.TryPop(out var cancelableObject))
        //{
        //    cancelableObject.lastGrabbedObject.transform.parent = cancelableObject.savedParent;
        //    cancelableObject.lastGrabbedObject.transform.SetPositionAndRotation(cancelableObject.savedPosition, cancelableObject.savedRotation);
        //    cancelableObject.lastGrabbedObject.transform.localScale = cancelableObject.savedScale;
        //}


    }
}
