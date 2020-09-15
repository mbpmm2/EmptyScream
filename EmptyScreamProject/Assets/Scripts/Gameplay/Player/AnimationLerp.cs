using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AnimationLerp : MonoBehaviour
{
    public bool canChange;
    public GameObject cameraRig;
    public GameObject currentCamera;
    public float speedDivisionRotation;
    public Animator objectAnimator;
    public AnimatorClipInfo[] animationClip;

    public Quaternion rotationOffset;
    private Quaternion newRotation;

    // Start is called before the first frame update
    void Start()
    {
        //animationClip = objectAnimator.GetCurrentAnimatorClipInfo(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        animationClip = objectAnimator.GetCurrentAnimatorClipInfo(0);

        if(canChange)
        {
            if (animationClip[0].clip.name != "Idle") // check if any animation is playing
            {
                newRotation = cameraRig.transform.rotation;
                newRotation *= rotationOffset;

                currentCamera.transform.rotation = Quaternion.Lerp(currentCamera.transform.rotation, newRotation, 1 / speedDivisionRotation);
            }
        }

    }
}
