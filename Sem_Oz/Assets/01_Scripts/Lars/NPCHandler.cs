using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class NPCHandler : MonoBehaviour
{
    public GameObject prefabNPC;
    public List<Transform> startEndPositions = new List<Transform>();
    public List<Transform> waitLocations = new List<Transform>();

    [SerializeField]
    private float npcSpawnMinTime = 8.0f;
    [SerializeField]
    private float npcSpawnMaxTime = 15.0f;
    private float npcSpawnTimer = 15.0f;

    private void Start()
    {
        CreateNPC();
        npcSpawnTimer = Random.Range(npcSpawnMinTime, npcSpawnMaxTime);
    }

    private void FixedUpdate()
    {
        npcSpawnTimer -= Time.fixedDeltaTime;

        if (npcSpawnTimer <= 0)
        {
            npcSpawnTimer = Random.Range(npcSpawnMinTime, npcSpawnMaxTime);
            CreateNPC();
        }
    }

    private void CreateNPC()
    {
        int rnd = UnityEngine.Random.Range(0, 5);
        GameObject newNPC = Instantiate(prefabNPC, startEndPositions[rnd].position, Quaternion.identity);
        NPCBehaviour behaviour = newNPC.GetComponentInChildren<NPCBehaviour>();

        behaviour.taking = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
    }
}
