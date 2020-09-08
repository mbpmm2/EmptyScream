using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkin : MonoBehaviour
{
    public SkinnedMeshRenderer scarf;
    public SkinnedMeshRenderer shirt;
    public SkinnedMeshRenderer pant;
    public GameObject[] masks;

    public Material[] scarfMats;
    public Material[] shirtMats;
    public Material[] pantMats;
    // Start is called before the first frame update
    void Start()
    {
        bool scarfBool = (Random.value > 0.5f);
        bool shirtBool = (Random.value > 0.5f);
        bool maskBool = (Random.value > 0.5f);
        scarf.gameObject.SetActive(scarfBool);
        shirt.gameObject.SetActive(shirtBool);

        if (scarfBool == false)
        {
            masks[Random.Range(0, masks.Length)].SetActive(maskBool);
        }

        scarf.material = scarfMats[Random.Range(0, scarfMats.Length)];
        shirt.material = shirtMats[Random.Range(0, shirtMats.Length)];
        pant.material = pantMats[Random.Range(0, pantMats.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
