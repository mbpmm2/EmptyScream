using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimations : MonoBehaviour
{
    public delegate void OnAnimationAction();
    public static OnAnimationAction OnBlock;
    public static OnAnimationAction OnDisableBlock;
    public static OnAnimationAction OnCanUse;
    public static OnAnimationAction OnAnimationEnd;

    //private AnimationLerp lerp;

    // Start is called before the first frame update
    void Start()
    {
        //lerp = GetComponent<AnimationLerp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Block()
    {
        if(OnBlock != null)
        {
            Debug.Log("Blocking DElegate");
            OnBlock();
        }
    }

    public void DisableBlock()
    {
        if(OnDisableBlock != null)
        {
            OnDisableBlock();
        }
    }

    public void CanUse()
    {
        if (OnCanUse != null)
        {
            OnCanUse();
        }
    }

    public void AnimationEnd()
    {
        if(OnAnimationEnd != null)
        {
            OnAnimationEnd();
        }
    }
}
