using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public GameObject prefabItem;
    public List<GameObject> items = new List<GameObject>();
    public bool hasGivingItem = false;
    public bool awaitingItem = false;
    public bool hasTakingItem = false;
    public Transform itemSlot;
    public Transform speechBubble;
    public SpriteRenderer speechItem;
    public TMP_Text giveOrTakeText;

    [HideInInspector] public bool taking;
    [HideInInspector] public bool giving;
    [HideInInspector] public ItemType currentItem;
    [HideInInspector] public GameObject currentItemObject;

    private void Awake()
    {
        speechBubble.gameObject.SetActive(false);
    }

    private void Start()
    {
        //join the scene

        if (giving)
        {
            CreateGivingItem();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoseGivingItem();
        }

        if (!hasGivingItem)
        {
            if (taking && !awaitingItem)
            {
                CreateTakingItemRequest();
            }

            if (!taking)
            {
                //leave the scene
            }
        }
    }

    private void CreateGivingItem()
    {
        int itemToHold = Random.Range(1, 6);
        currentItem = (ItemType)itemToHold;
        speechBubble.gameObject.SetActive(true);
        giveOrTakeText.text = "!";
        currentItemObject = Instantiate(prefabItem, itemSlot);
        speechItem.color = currentItemObject.GetComponent<Item>().SetType(currentItem);
        hasGivingItem = true;
    }

    public void LoseGivingItem()
    {
        hasGivingItem = false;
        currentItemObject.transform.parent = null;
        speechBubble.gameObject.SetActive(false);
    }

    private void CreateTakingItemRequest()
    {
        awaitingItem = true;
        int itemToHold = Random.Range(1, 6);
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

    public void GetTakingItem()
    {
    }
}
