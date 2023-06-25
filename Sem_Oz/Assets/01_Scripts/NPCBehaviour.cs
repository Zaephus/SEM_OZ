using System.Collections;
using System.Collections.Generic;

using Pathfinding;

using TMPro;

using UnityEngine;
using UnityEngine.U2D.Animation;

public class NPCBehaviour : MonoBehaviour {

    public GameObject prefabItem;
    public List<GameObject> items = new List<GameObject>();

    public bool hasGivingItem = false;
    public bool canGiveItem = false;
    public bool awaitingItem = false;
    public bool hasTakingItem = false;
    public bool leaving = false;

    public Transform itemSlot;
    public Transform speechBubble;
    public SpriteRenderer speechItem;
    public TMP_Text giveOrTakeText;

    [HideInInspector]
    public bool taking;
    [HideInInspector]
    public ItemType currentItem;
    [HideInInspector]
    public GameObject currentItemObject;
    [HideInInspector]
    public Transform waitPosition;

    private NPCHandler handler;
    private AIDestinationSetter pathTarget;
    private AIPath pathingData;

    [SerializeField]
    private SpriteResolver reactionSprites;
    [SerializeField]
    private SpriteResolver npcSprites;
    private string[] npcNames = {
        "NPC_1", "NPC_2"
    };


    private void Awake() {
        handler = FindAnyObjectByType<NPCHandler>();

        pathTarget = transform.parent.GetComponent<AIDestinationSetter>();
        pathingData = transform.parent.GetComponent<AIPath>();

        speechBubble.gameObject.SetActive(false);
        npcSprites.SetCategoryAndLabel("NPC Sprites", npcNames[Random.Range(0, npcNames.Length)]);
        npcSprites.ResolveSpriteToSpriteRenderer();
    }

    private void Start() {
        int rnd = Random.Range(0, handler.waitLocations.Count);
        waitPosition = handler.waitLocations[rnd];
        handler.waitLocations.Remove(waitPosition);
        pathTarget.target = waitPosition;

        CreateGivingItem();
    }

    private void Update() {

        if(hasGivingItem && pathingData.reachedDestination) {
            ShowGivingItem();
        }

        if(leaving && pathingData.reachedDestination) {
            handler.waitLocations.Add(waitPosition);
            Destroy(transform.parent.gameObject);
        }

        if(!hasGivingItem) {
            if(taking && !awaitingItem) {
                CreateTakingItemRequest();
            }

            if(!taking && !leaving) {
                LeaveScene();
            }
        }

        if(hasTakingItem && !leaving) {
            LeaveScene();
        }

    }

    private void LeaveScene() {
        leaving = true;
        int rnd = Random.Range(0, 5);
        pathTarget.target = handler.startEndPositions[rnd];
    }

    private void CreateGivingItem() {
        int itemToHold = Random.Range(1, 6);
        currentItem = (ItemType)itemToHold;
        currentItemObject = Instantiate(prefabItem, itemSlot);
        speechItem.color = currentItemObject.GetComponent<Item>().SetType(currentItem);
        hasGivingItem = true;
    }

    private void ShowGivingItem() {
        speechBubble.gameObject.SetActive(true);
        canGiveItem = true;
        reactionSprites.SetCategoryAndLabel("Face", "Exclamation");
        reactionSprites.ResolveSpriteToSpriteRenderer();
    }

    public void LoseGivingItem() {
        hasGivingItem = false;
        speechBubble.gameObject.SetActive(false);
    }

    private void CreateTakingItemRequest() {

        awaitingItem = true;
        int itemToHold = RandomNoRepeat();
        currentItem = (ItemType)itemToHold;
        speechBubble.gameObject.SetActive(true);

        reactionSprites.SetCategoryAndLabel("Face", "Question");
        reactionSprites.ResolveSpriteToSpriteRenderer();

        switch (currentItem) {
            case ItemType.Milk:
                speechItem.color = Color.red;
                break;
            case ItemType.Plank:
                speechItem.color = Color.blue;
                break;
            case ItemType.Solar_Panel:
                speechItem.color = Color.green;
                break;
            case ItemType.Book:
                speechItem.color = Color.cyan;
                break;
            case ItemType.Tomato_Plant:
                speechItem.color = Color.magenta;
                break;
        }
    }

    private int RandomNoRepeat() {
        int itemToHold = Random.Range(1, 6);
        if (currentItem == (ItemType)itemToHold) {
            itemToHold = RandomNoRepeat();
        }

        return itemToHold;
    }

    public void GetTakingItem(GameObject _item) {
        if(_item.GetComponent<Item>().itemType == currentItem) {
            hasTakingItem = true;
            _item.transform.parent = itemSlot;
            _item.transform.localPosition = Vector3.zero;
            speechBubble.gameObject.SetActive(false);
        }
    }

}
