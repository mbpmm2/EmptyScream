using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreditsRoll : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject creditsContent;
    public Transform creditsFinalPosition;

    [Header("Speed Settings")]
    public KeyCode highspeedKey;
    public float creditsSpeed;
    public float creditsHighSpeed;

    private float finalSpeed;
    private float initialYPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialYPosition = creditsContent.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(highspeedKey))
        {
            finalSpeed = creditsHighSpeed;
        }
        else
        {
            finalSpeed = creditsSpeed;
        }

        creditsContent.transform.position += new Vector3(0, Time.deltaTime * finalSpeed, 0);

        if (creditsContent.transform.position.y >= creditsFinalPosition.position.y)
        {
            creditsContent.transform.position = new Vector3(creditsContent.transform.position.x, initialYPosition, creditsContent.transform.position.z);
        }
    }
}
