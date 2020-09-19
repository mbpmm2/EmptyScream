using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AnimationLerp : MonoBehaviour
{
    public bool canChange;
    public bool canLerp;
    public GameObject cameraRig;
    public GameObject currentCamera;
    public float speedDivisionRotation;
    public Animator objectAnimator;
    public AnimatorClipInfo[] animationClip;

    public Quaternion rotationOffset;
    private Quaternion newRotation;
    private Quaternion finalRotation;
    private Quaternion initialRotation;

    public float timer;
    public float animationMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        //animationClip = objectAnimator.GetCurrentAnimatorClipInfo(0);
        initialRotation = currentCamera.transform.rotation;
        //initialRotation *= rotationOffset;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        animationClip = objectAnimator.GetCurrentAnimatorClipInfo(0);
        initialRotation = currentCamera.transform.rotation;

        if (canChange)
        {
            newRotation = cameraRig.transform.rotation;
            newRotation *= rotationOffset;

            currentCamera.transform.rotation = Quaternion.Lerp(currentCamera.transform.rotation, newRotation, 1 / speedDivisionRotation);
            finalRotation = currentCamera.transform.rotation;

            timer = 0;
        }
        else
        {
            if(canLerp)
            {
                timer += Time.deltaTime;

                if (timer >= 1)
                {
                    timer = 1;
                    canLerp = false;
                }

                currentCamera.transform.rotation = Quaternion.Lerp(finalRotation, initialRotation, timer * animationMultiplier);
            }
            
        }

    }
}
