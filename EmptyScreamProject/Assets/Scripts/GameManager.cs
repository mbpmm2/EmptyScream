using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    public GameObject playerGO;
    public bool restart;
    public float restartTime;
    private float restartTimer;
    private UIScreenChanger scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = GetComponent<UIScreenChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(restart)
        {
            restartTimer += Time.deltaTime;
            if(restartTimer >= restartTime)
            {
                RestartLevel();
            }
        }
    }

    public void RestartLevel()
    {
        scene.changeScene = true;
        scene.nameOfNewScene = SceneManager.GetActiveScene().name;
        scene.ChangeScreen();
    }
}
