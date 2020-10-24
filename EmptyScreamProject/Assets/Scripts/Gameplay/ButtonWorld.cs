using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWorld : Interactable
{
    private Button button;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        button = GetComponent<Button>();
        OnInteract += ExecuteAction;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void ExecuteAction()
    {
        if(canInteract)
        {
            canInteract = false;
            button.onClick.Invoke();
        }
    }

    public void PlayInteractSound(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    private void OnDestroy()
    {
        OnInteract -= ExecuteAction;
    }
}
