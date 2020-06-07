
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollingTexture : MonoBehaviour
{
    public float speed;

    private float timeBackground;
    //MeshRenderer backgroundRenderer;
    public RectTransform currentImage;
    public Vector3 originalPosition;
    public Vector3 finalPosition;
    
    private void Start()
    {
        //backgroundRenderer = GetComponent<MeshRenderer>();
        //currentImage = GetComponent<Image>();
        //currentImage = GetComponent<RectTransform>();
        originalPosition = currentImage.localPosition;
        //finalPosition = originalPosition + new Vector2(originalPosition.position.x * 2, 0,0);
    }

    // Update is called once per frame
    void Update()
    {
        //timeBackground += Time.deltaTime;

        currentImage.localPosition += new Vector3(speed * Time.deltaTime, 0,0);
        if(currentImage.localPosition.x >= finalPosition.x)
        {
            currentImage.localPosition = originalPosition;
        }
       // currentImage.material.SetTextureOffset("_BaseColorMap", new Vector2(timeBackground * speed, 0));
        //currentImage.material.mainTextureOffset = new Vector2(timeBackground * speed, 0);
        //backgroundRenderer.material.mainTextureOffset = new Vector2(timeBackground * speed, 0);
    }
}
