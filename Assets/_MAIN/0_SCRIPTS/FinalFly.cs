using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FinalFly : MonoBehaviour
{

    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private Transform[] pathPointsTransforms1;
    [SerializeField] private Transform[] pathPointsTransforms2;
    private Vector3[] pathPoints1;
    private Vector3[] pathPoints2;
    private Transform dronTransform;

    private Animator dronAnimator;
    private float duration1 = 5;
    private float duration2 = 10;
    private float pause = 3;
    List<Tween> tweens = new List<Tween>();


    private void Awake()
    {
        pathPoints1 = new Vector3[pathPointsTransforms1.Length];
        for (int i = 0; i < pathPointsTransforms1.Length; i++)
        {
            pathPoints1[i] = pathPointsTransforms1[i].position;
        }
        pathPoints2 = new Vector3[pathPointsTransforms2.Length];
        for (int i = 0; i < pathPointsTransforms2.Length; i++)
        {
            pathPoints2[i] = pathPointsTransforms2[i].position;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var dron = socket.interactablesSelected[0];
            if (dron != null)
            {
                socket.enabled = false;
                dronAnimator = dron.transform.gameObject.GetComponent<Animator>();
                dronAnimator.SetTrigger("Fly");
                dronTransform = dron.transform;
                FlyByPath();
            }
        }
    }

    public void FlyByPath()
    {

        Debug.Log($"FlyByPath {this}");
        ////Vector3[] worldPath = pathPoints;

        //transform.DOPath(pathPoints, duration, PathType.CatmullRom, PathMode.Full3D)
        //    .SetEase(Ease.Linear);          // линейная скорость

        if (socket.interactablesSelected.Count>0)
        {
            var dron = socket.interactablesSelected[0]; 
            if (dron != null)
            {
                Debug.Log($"dron != null {this}");
                socket.enabled = false;
                dronAnimator = dron.transform.gameObject.GetComponent<Animator>();
                var holdersTransform = dron.transform.Find("Holders");
                var propellersTransform = holdersTransform.Find("Propellers");    
                var sockets = propellersTransform.GetComponentsInChildren<XRSocketInteractor>();
                foreach (var socket in sockets)
                {
                    //Debug.Log(child.name);
                    if (socket.interactablesSelected.Count !=0)
                    {

                        Tween rotationTween = socket.GetOldestInteractableSelected().transform
                                            .DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
                                            .SetEase(Ease.Linear)
                                            .SetLoops(-1, LoopType.Restart);
                        tweens.Add(rotationTween); 
                    }
                }
                //dronAnimator.SetTrigger("Fly");
                dronTransform = dron.transform;
            
            }
        }

        if (dronTransform!= null)
        {

            Debug.Log($"dronTransform!= null {this}");
            Sequence seq = DOTween.Sequence();
            seq.Append(dronTransform
                .DOPath(pathPoints1, duration1, PathType.CatmullRom)
                .SetEase(Ease.Linear)
            );
            seq.Append(dronTransform
                .DORotate(new Vector3(0, 90, 0), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
            );
            seq.AppendInterval(pause);

            seq.Append(dronTransform
                .DOPath(pathPoints2, duration2, PathType.CatmullRom)
                .SetEase(Ease.Linear)
            );
            seq.Join(dronTransform
                .DORotate(new Vector3(0, -90, 0), duration2, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine));

            seq.AppendInterval(pause);
            seq.OnComplete(OnSequenceComplete); 
        }
    }

    private void OnSequenceComplete()
    {

        Debug.Log($"tweeners count {tweens.Count}");
        foreach (var tween in tweens)
        {
            tween.Kill();
        }
        //dronAnimator.SetTrigger("Stop");
    }
}
