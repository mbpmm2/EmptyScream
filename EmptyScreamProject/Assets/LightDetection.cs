using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    private GameObject playerGO;
    private Player player;
    private Light lightComp;

    private bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameManager.Get().playerGO;
        player = GameManager.Get().playerGO.GetComponent<Player>();
        lightComp = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (transform.position - playerGO.transform.position) * -1f, out hit, lightComp.range))
        {

            if (hit.collider.tag == "Player")
            {
                if (!doOnce)
                {
                    player.lightsOnPlayer.Add(this.gameObject);
                    doOnce = true;
                }
                
            }
            else 
            {
                player.lightsOnPlayer.Remove(this.gameObject);
                doOnce = false;
            }

        }
    }
}
