using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    public delegate void OnAnimationAction();
    public static OnAnimationAction OnHideEnd;
    public static OnAnimationAction OnDrawEnd;
    public static OnAnimationAction OnShootStart;
    public static OnAnimationAction OnShootEnd;
    public static OnAnimationAction OnReloadStart;
    public static OnAnimationAction OnReloadEnd;
    public static OnAnimationAction OnHitStart;
    public static OnAnimationAction OnHitEnd;
    public static OnAnimationAction OnHitImpact;
    public static OnAnimationAction OnHeal;
    public static OnAnimationAction OnImmune;

    public AnimationLerp lerp;

    // Start is called before the first frame update
    void Start()
    {
        lerp = GetComponent<AnimationLerp>();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }*/

    public void HitImpact()
    {
        if (OnHitImpact != null)
        {
            OnHitImpact();
        }
    }

    public void ShootStartAnimationTrigger()
    {
        if (OnShootStart != null)
        {
            OnShootStart();
        }
    }

    public void ShootEndAnimationTrigger()
    {
        if (OnShootEnd != null)
        {
            OnShootEnd();
        }
    }

    public void ReloadStartAnimationTrigger()
    {
        if (OnReloadStart != null)
        {
            OnReloadStart();
        }
    }

    public void ReloadEndAnimationTrigger()
    {
        if (OnReloadEnd != null)
        {
            OnReloadEnd();
        }
    }

    public void HideAnimationTrigger()
    {
        /*if(lerp)
        {
            lerp.canChange = true;
        }*/

        if(OnHideEnd != null)
        {
            OnHideEnd();
        }
    }

    public void DrawAnimationTrigger()
    {
        /*if (lerp)
        {
            lerp.canChange = false;
        }*/

        if (OnDrawEnd != null)
        {
            Debug.Log("nice2");
            OnDrawEnd();
        }
    }

    public void HitStartAnimationTrigger()
    {
        /*if (lerp)
        {
            lerp.canChange = true;
        }*/

        if (OnShootStart != null)
        {
            OnHitStart();
        }
    }

    public void HitEndAnimationTrigger()
    {
        /*if (lerp)
        {
            lerp.canChange = false;
            lerp.canLerp = true;
        }*/

        if (OnShootEnd != null)
        {
            OnHitEnd();
        }
    }

    public void StartHeal()
    {
        if(OnHeal != null)
        {
            OnHeal();
        }
    }

    public void StartImmunity()
    {
        if (OnImmune != null)
        {
            OnImmune();
        }
    }
}
