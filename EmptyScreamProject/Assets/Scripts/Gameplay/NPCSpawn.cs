using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public KeyCode spawnKey;
    public GameObject npcToSpawn;
    public Transform spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(spawnKey))
        {
            SpawnNPC();
        }
    }

    public void SpawnNPC()
    {
        Instantiate(npcToSpawn, spawnPosition.position, spawnPosition.rotation);
    }

    public void PlayButtonSound()
    {
        AkSoundEngine.PostEvent("Terminal", gameObject);
    }
}
