using System.Collections;
using System.Collections.Generic;

using Pathfinding;

using TMPro;

using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
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

    [HideInInspector] public bool taking;
    [HideInInspector] public ItemType currentItem;
    [HideInInspector] public GameObject currentItemObject;
    [HideInInspector] public Transform waitPosition;

    private NPCHandler handler;
    private AIDestinationSetter pathTarget;
    private AIPath pathingData;

    private void Awake()
    {
        handler = FindAnyObjectByType<NPCHandler>();
        pathTarget = transform.parent.GetComponent<AIDestinationSetter>();
        pathingData = transform.parent.GetComponent<AIPath>();
        speechBubble.gameObject.SetActive(false);
    }

    private void Start()
    {
        int rnd = Random.Range(0, 5);
        pathTarget.target = handler.waitLocations[rnd];

        CreateGivingItem();
    }

    private void Update()
    {
        if (hasGivingItem && pathingData.reachedDestination)
        {
            ShowGivingItem();
        }

        if (leaving && pathingData.reachedDestination)
        {
            Destroy(transform.parent.gameObject);
        }

        // Debug.Log(pathingData.reachedDestination);

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoseGivingItem();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GetTakingItemTest(ItemType.Milk, GameObject.Find("FauxItem"));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetTakingItemTest(ItemType.Plank, GameObject.Find("FauxItem"));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GetTakingItemTest(ItemType.Solar_Panel, GameObject.Find("FauxItem"));
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            GetTakingItemTest(ItemType.Book, GameObject.Find("FauxItem"));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GetTakingItemTest(ItemType.Tomato_Plant, GameObject.Find("FauxItem"));
        }

        if (!hasGivingItem)
        {
            if (taking && !awaitingItem)
            {
                CreateTakingItemRequest();
            }

            if (!taking && !leaving)
            {
                LeaveScene();
            }
        }

        if (hasTakingItem && !leaving)
        {
            LeaveScene();
        }
    }

    private void LeaveScene()
    {
        leaving = true;
        int rnd = Random.Range(0, 5);
        pathTarget.target = handler.startEndPositions[rnd];
    }

    private void CreateGivingItem()
    {
        int itemToHold = Random.Range(1, 6);
        currentItem = (ItemType)itemToHold;
        currentItemObject = Instantiate(prefabItem, itemSlot);
        speechItem.color = currentItemObject.GetComponent<Item>().SetType(currentItem);
        hasGivingItem = true;
    }

    private void ShowGivingItem()
    {
        speechBubble.gameObject.SetActive(true);
        canGiveItem = true;
        giveOrTakeText.text = "!";
    }

    public void LoseGivingItem()
    {
        hasGivingItem = false;
        speechBubble.gameObject.SetActive(false);
    }

    private void CreateTakingItemRequest()
    {
        awaitingItem = true;
        int itemToHold = RandomNoRepeat();
        currentItem = (ItemType)itemToHold;
        speechBubble.gameObject.SetActive(true);
        giveOrTakeText.text = "?";

        switch (currentItem)
        {
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

    private int RandomNoRepeat()
    {
        int itemToHold = Random.Range(1, 6);
        if (currentItem == (ItemType)itemToHold)
        {
            itemToHold = RandomNoRepeat();
        }

        return itemToHold;
    }

    public void GetTakingItem(GameObject _item)
    {
        if (_item.GetComponent<Item>().itemType == currentItem)
        {
            hasTakingItem = true;
            _item.transform.parent = itemSlot;
            _item.transform.localPosition = Vector3.zero;
            speechBubble.gameObject.SetActive(false);
        }
    }

    private void GetTakingItemTest(ItemType _itemType, GameObject _item)
    {
        if (_itemType == currentItem)
        {
            hasTakingItem = true;
            _item.transform.parent = itemSlot;
            _item.transform.localPosition = Vector3.zero;
            speechBubble.gameObject.SetActive(false);
        }
    }
}
