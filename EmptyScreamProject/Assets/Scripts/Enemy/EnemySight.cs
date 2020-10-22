using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    public float fovAngle;
    public bool playerInSight;
    public bool playerHeard;
    public Vector3 lastPlayerPos;
    public LayerMask mask;

    public NavMeshAgent nav;
    public SphereCollider detectionCol;
    public EnemyController enemyController;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerController;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fovAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, direction.normalized, out hit, detectionCol.radius * 1.2f, mask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        playerInSight = true;
                        lastPlayerPos = hit.transform.position;
                    }
                }
            }

            if (playerController.isRunning)
            {
                if (CalculatePathLenght(playerController.transform.position)<=detectionCol.radius * 1.2f)
                {
                    playerHeard = true;
                    lastPlayerPos = playerController.transform.position;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInSight = false;
            playerHeard = false;
        }
    }

    float CalculatePathLenght(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
        {
            nav.CalculatePath(targetPos, path);
        }

        Vector3[] allWaypoints = new Vector3[path.corners.Length + 2];

        allWaypoints[0] = transform.position;
        allWaypoints[allWaypoints.Length - 1] = targetPos;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWaypoints[i + 1] = path.corners[i];
        }

        float pathLenght = 0;

        for (int i = 0; i < allWaypoints.Length-1; i++)
        {
            pathLenght += Vector3.Distance(allWaypoints[i], allWaypoints[i + 1]);
        }

        return pathLenght;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
