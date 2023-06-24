using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    public GameObject prefabNPC;
    public List<Vector2> startendPositions = new List<Vector2>();
    public List<Vector2> waitLocations = new List<Vector2>();

    private float npcSpawnMaxTime = 15.0f;
    private float npcSpawnMinMaxTime = 8.0f;
    private float npcSpawnTimer = 15.0f;

    private void Start()
    {
        CreateNPC(true);
    }

    private void FixedUpdate()
    {
        npcSpawnTimer -= Time.fixedDeltaTime;

        if (npcSpawnTimer <= 0)
        {
            npcSpawnTimer = npcSpawnMaxTime;
            npcSpawnMaxTime -= 0.1f;
            npcSpawnMaxTime = Mathf.Max(npcSpawnMaxTime, npcSpawnMinMaxTime);
            CreateNPC(false);
        }
    }

    private void CreateNPC(bool _giving)
    {
        int rnd = UnityEngine.Random.Range(0, 5);
        GameObject newNPC = Instantiate(prefabNPC, startendPositions[rnd], Quaternion.identity);
        NPCBehaviour behaviour = newNPC.GetComponent<NPCBehaviour>();

        behaviour.giving = _giving ? _giving : Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        behaviour.taking = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

        //behaviour.
    }
}
