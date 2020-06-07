using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    public delegate void OnAnimationAction();
    public static OnAnimationAction OnHideEnd;
    public static OnAnimationAction OnDrawEnd;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }*/

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
}
