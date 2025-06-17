using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class UIPart : MonoBehaviour
{
    // Почти , но спавниться от вращения анимации в разных местах
    //[Tooltip("Префаб объекта, идентичный этому, без скрипта GrabAndReplace")]    
    //public GameObject replacementPrefab;

    //private XRGrabInteractable sourceGrab;
    //private Animator sourceAnimator;

    //private void Awake()
    //{
    //    sourceGrab = GetComponent<XRGrabInteractable>();
    //    sourceGrab.selectEntered.AddListener(OnGrabbed);
    //    sourceAnimator = GetComponent<Animator>(); // если есть
    //}

    //private void OnDestroy()
    //{
    //    sourceGrab.selectEntered.RemoveListener(OnGrabbed);
    //}


    //private void OnGrabbed(SelectEnterEventArgs args)
    //{
    //    var objTransform = args.interactableObject.transform;
    //    // 2. Спавним копию в точности на этом месте и с теми же трансформами
    //    Vector3 pos = objTransform.position;
    //    Quaternion rot = objTransform.rotation;
    //    Vector3 scale = objTransform.localScale;

    //    GameObject copy = Instantiate(replacementPrefab, pos, rot);
    //    copy.transform.localScale = scale;

    //    // 3. Добавляем XRGrabInteractable на копию, если нет
    //    var grabCopy = copy.GetComponent<XRGrabInteractable>();
    //    if (grabCopy == null)
    //    {
    //        grabCopy = copy.AddComponent<XRGrabInteractable>();
    //    }

    //    // 4. Передаём захват интератору на копию
    //    if (args.interactorObject is IXRSelectInteractor selectInteractor)
    //    {
    //        var manager = sourceGrab.interactionManager;
    //        // отпускаем оригинал
    //        manager.SelectExit(selectInteractor, sourceGrab);
    //        // захватываем копию
    //        manager.SelectEnter(selectInteractor, grabCopy);
    //    }

    //    // 5. Можно удалить или деактивировать оригинал
    //    // gameObject.SetActive(false);
    //}
    [Tooltip("Префаб копии, которая будет оставаться на месте")]
    public GameObject clonePrefab;
    public Transform originalParent;
    public bool isClonable = true;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        originalParent = transform.parent;
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        MakeClone();
    }

    private void MakeClone()
    {
        if (isClonable)
        {
            // Сохраняем текущие параметры перед тем, как объект уйдёт в руку
            Vector3 originalPosition = transform.position;
            Quaternion originalRotation = transform.rotation;
            Vector3 originalScale = transform.localScale;

            GameObject clone = Instantiate(clonePrefab, originalPosition, originalRotation);
            clone.transform.localScale = originalScale;
            clone.transform.parent = originalParent;
            clone.GetComponent<UIPart>().originalParent = originalParent; 

            var animator = GetComponent<Animator>();
            if (animator != null) Destroy(GetComponent<Animator>());
            isClonable = false;


            //// Опционально добавляем XRGrabInteractable на клоне,
            //// чтобы его тоже можно было схватить
            //if (clone.GetComponent<XRGrabInteractable>() == null)
            //{
            //    clone.AddComponent<XRGrabInteractable>();
            //}

            //// Оригинал остаётся в руке, поэтому не делаем SelectExit/Enter 
        }
    }
}
