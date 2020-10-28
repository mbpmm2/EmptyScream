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
        if(OnHideEnd != null)
        {
            OnHideEnd();
        }
    }

    public void DrawAnimationTrigger()
    {
        if (OnDrawEnd != null)
        {
            OnDrawEnd();
        }
    }

    public void HitStartAnimationTrigger()
    {
        if (OnShootStart != null)
        {
            OnHitStart();
        }
    }

    public void HitEndAnimationTrigger()
    {
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
