using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class NPCHandler : MonoBehaviour {
    public GameObject prefabNPC;
    public List<Transform> startEndPositions = new List<Transform>();
    public List<Transform> waitLocations = new List<Transform>();

    public Dictionary<Transform, bool> waitLocOccupancy = new Dictionary<Transform, bool>();

    [SerializeField]
    private float npcSpawnMinTime = 8.0f;
    [SerializeField]
    private float npcSpawnMaxTime = 15.0f;
    private float npcSpawnTimer = 15.0f;

    [HideInInspector]
    public int npcAmount = 0;

    private void Start() {
        CreateNPC();
        npcSpawnTimer = Random.Range(npcSpawnMinTime, npcSpawnMaxTime);

        foreach(Transform t in waitLocations) {
            waitLocOccupancy.Add(t, false);
        }
    }

    private void FixedUpdate() {
        npcSpawnTimer -= Time.fixedDeltaTime;

        if(npcSpawnTimer <= 0 && waitLocations.Count > 0) {
            npcSpawnTimer = Random.Range(npcSpawnMinTime, npcSpawnMaxTime);
            CreateNPC();
        }
    }

    private void CreateNPC() {
        int rnd = UnityEngine.Random.Range(0, 5);
        GameObject newNPC = Instantiate(prefabNPC, startEndPositions[rnd].position, Quaternion.identity);
        NPCBehaviour behaviour = newNPC.GetComponentInChildren<NPCBehaviour>();

        behaviour.taking = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
    }

}
