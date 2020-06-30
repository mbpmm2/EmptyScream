using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public LayerMask rayCastLayer;
    public float range;

    private GameObject playerGO;
    private Player player;
    public bool isLightOnPlayer;

    private bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameManager.Get().playerGO;
        player = GameManager.Get().playerGO.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (transform.position - playerGO.transform.position) * -1f, out hit, range, rayCastLayer))
        {

            if (hit.collider.tag == "Player")
            {
                if (!doOnce)
                {
                    player.lightsOnPlayer.Add(this.gameObject);
                    doOnce = true;
                    isLightOnPlayer = true;
                }
                
            }
            else 
            {
                player.lightsOnPlayer.Remove(this.gameObject);
                doOnce = false;
            }

        }
        else
        {
            if(isLightOnPlayer)
            {
                isLightOnPlayer = false;
                doOnce = false;
                player.lightsOnPlayer.Remove(gameObject);
            }
        }
    }

    private void OnDisable()
    {
        if (isLightOnPlayer)
        {
            isLightOnPlayer = false;
            doOnce = false;
            player.lightsOnPlayer.Remove(gameObject);
        }
    }
}
