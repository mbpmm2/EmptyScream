using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLerp : MonoBehaviour
{
    public GameObject cameraRig;
    public GameObject currentCamera;
    public float speedDivisionPosition;
    public float speedDivisionRotation;
    public Animator objectAnimator;
    public AnimatorClipInfo[] animationClip;

    public Vector3 offset;
    private Vector3 initialPosition;
    private Vector3 newPosition;
    public Quaternion initialRotation;
    private Quaternion newRotation;

    // Start is called before the first frame update
    void Start()
    {
        animationClip = objectAnimator.GetCurrentAnimatorClipInfo(0);
        initialPosition = currentCamera.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        newRotation = cameraRig.transform.rotation;
        newRotation *= Quaternion.Euler(23, 180, 0);
        newPosition = cameraRig.transform.position + offset;

        //currentCamera.transform.position = Vector3.Lerp(initialPosition, newPosition, animationClip[0].weight / speedDivisionPosition);
        currentCamera.transform.rotation = Quaternion.Lerp(initialRotation, newRotation, animationClip[0].weight / speedDivisionRotation);
    }
}
