using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    public GameObject prefabNPC;
    public List<Transform> startendPositions = new List<Transform>();
    public List<Transform> waitLocations = new List<Transform>();

    private float npcSpawnMaxTime = 15.0f;
    private float npcSpawnMinMaxTime = 8.0f;
    private float npcSpawnTimer = 15.0f;

    private void Start()
    {
        CreateNPC();
    }

    private void FixedUpdate()
    {
        npcSpawnTimer -= Time.fixedDeltaTime;

        if (npcSpawnTimer <= 0)
        {
            npcSpawnTimer = npcSpawnMaxTime;
            npcSpawnMaxTime -= 0.1f;
            npcSpawnMaxTime = Mathf.Max(npcSpawnMaxTime, npcSpawnMinMaxTime);
            CreateNPC();
        }
    }

    private void CreateNPC()
    {
        int rnd = UnityEngine.Random.Range(0, 5);
        GameObject newNPC = Instantiate(prefabNPC, startendPositions[rnd].position, Quaternion.identity);
        NPCBehaviour behaviour = newNPC.GetComponentInChildren<NPCBehaviour>();

        behaviour.taking = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
    }
}
