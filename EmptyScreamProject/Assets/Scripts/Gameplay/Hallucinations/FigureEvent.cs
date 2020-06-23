using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureEvent : HallucinationEvent
{
    [System.Serializable]
    public struct RaycastInfo
    {
        public LayerMask rayCastLayer;
        public float rayDistance;
        public RaycastHit raycastsHit;
        public Vector3 start;
        public Vector3 dir;
        public bool isTouching;
    }

    public bool onScreen;
    public bool canTeleport;
    public bool canUse;
    public float waitingTime;
    public float appearDistanceMultiplier;
    public GameObject player;
    public GameObject figure;
    public RaycastInfo[] raycasts;

    private float waitingTimer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        OnHallucinationStart += SpawnFigure;
        OnHallucinationEnd += HideFigure;
        HideFigure();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(isActive)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            onScreen = screenPoint.z > -0.25f && screenPoint.x > -0.25f && screenPoint.x < 1.25f && screenPoint.y > -0.25f && screenPoint.y < 1.25f;

            if (!onScreen)
            {

                if (canTeleport)
                {
                    if (raycasts.Length >= 1)
                    {
                        raycasts[0].dir = player.transform.forward * -1;
                    }

                    if (raycasts.Length >= 2)
                    {
                        raycasts[1].dir = player.transform.right * 1;
                    }

                    if (raycasts.Length >= 3)
                    {
                        raycasts[2].dir = player.transform.right * -1;
                    }

                    for (int i = 0; i < raycasts.Length; i++)
                    {
                        raycasts[i].start = player.transform.position;

                        if (Physics.Raycast(raycasts[i].start, raycasts[i].dir, out raycasts[i].raycastsHit, raycasts[i].rayDistance, raycasts[i].rayCastLayer))
                        {
                            Debug.DrawRay(raycasts[i].start, raycasts[i].dir * raycasts[i].raycastsHit.distance, Color.yellow);

                            //string layerHitted = LayerMask.LayerToName(raycasts[i].raycastsHit.transform.gameObject.layer);

                            raycasts[i].isTouching = true;
                        }
                        else
                        {
                            raycasts[i].isTouching = false;
                            Debug.DrawRay(raycasts[i].start, raycasts[i].dir * raycasts[i].rayDistance, Color.white);
                        }
                    }

                    for (int i = 0; i < raycasts.Length; i++)
                    {
                        if (!raycasts[i].isTouching)
                        {
                            Vector3 finalPos = transform.position;

                            switch (i)
                            {
                                case 0:
                                    finalPos = player.transform.position + player.transform.forward * -1 * appearDistanceMultiplier;
                                    break;
                                case 1:
                                    finalPos = player.transform.position + player.transform.right * 1 * appearDistanceMultiplier;
                                    break;
                                case 2:
                                    finalPos = player.transform.position + player.transform.forward * -1 * appearDistanceMultiplier;
                                    break;
                                default:
                                    break;
                            }

                            transform.position = finalPos;
                            transform.LookAt(player.transform);
                            transform.position += new Vector3(0, -1.0f, 0);

                            i = raycasts.Length;
                        }

                    }

                    canTeleport = false;
                }
                else
                {
                    waitingTimer += Time.deltaTime;

                    if (waitingTimer >= waitingTime)
                    {
                        canTeleport = true;
                        waitingTimer = 0;
                    }
                }
            }
        }
    }

    private void SpawnFigure()
    {
        figure.SetActive(true);
    }

    private void HideFigure()
    {
        figure.SetActive(false);
    }

    private void OnDestroy()
    {
        OnHallucinationStart -= SpawnFigure;
        OnHallucinationEnd -= HideFigure;
    }

}
