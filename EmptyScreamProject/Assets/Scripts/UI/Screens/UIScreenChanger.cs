using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScreenChanger : MonoBehaviour
{
    [Header("Scene Change Settings")]
    public bool changeScene;
    public bool quickChange;
    public string nameOfNewScene;

    [Header("Screens Settings")]
    public GameObject screenToChangeTo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(quickChange)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                
                // CursorLockMode.None;
                ChangeScreen();
            }
        }
    }

    public void ChangeScreen()
    {
        if(changeScene)
        {

            if (GameManager.Get())
            {
                Destroy(GameManager.Get().gameObject);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            SceneManager.LoadScene(nameOfNewScene);

            AkSoundEngine.StopAll();
            //AkSoundEngine.PostEvent("nail_gun_reload", gameObject);

            /**/
        }
        else
        {
            screenToChangeTo.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
        
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
