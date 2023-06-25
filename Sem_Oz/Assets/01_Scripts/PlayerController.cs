
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.U2D.Animation;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private ItemType heldItemType = ItemType.None;
    private GameObject heldItemObject = null;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform itemTransform;

    [SerializeField]
    private SpriteResolver reactionSprites;

    private NPCBehaviour targetedNPC = null;
    private Container targetedContainer = null;

    private AIPath pathData;

    private void Start()
    {
        pathData = transform.GetComponent<AIPath>();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            reactionSprites.SetCategoryAndLabel("Face", "Normal");
            reactionSprites.ResolveSpriteToSpriteRenderer();

            pathData.EndOfPathReached -= DestinationReached;

            Vector3 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<NPCBehaviour>() != null)
                {
                    targetedNPC = hit.collider.GetComponent<NPCBehaviour>();
                    if (targetedNPC.canGiveItem || targetedNPC.awaitingItem)
                    {
                        pathData.EndOfPathReached += DestinationReached;
                    }
                    else
                    {
                        targetedNPC = null;
                    }
                }
                else
                {
                    targetedNPC = null;
                }

                if (hit.collider.GetComponent<Container>() != null)
                {
                    targetedContainer = hit.collider.GetComponent<Container>();
                    pathData.EndOfPathReached += DestinationReached;
                }
                else
                {
                    targetedContainer = null;
                }
            }

            target.position = clickedPos;
            pathData.SearchPath();

        }

        if (Input.GetKeyDown(KeyCode.X) && heldItemType != ItemType.None)
        {
            DropItem();
        }

    }

    private void DropItem()
    {
        Destroy(heldItemObject);
        heldItemType = ItemType.None;
    }

    private void DestinationReached()
    {
        if (targetedContainer != null)
        {
            if (heldItemObject != null && heldItemType == targetedContainer.acceptedTypeForContainer)
            {
                reactionSprites.SetCategoryAndLabel("Face", "Normal");
                reactionSprites.ResolveSpriteToSpriteRenderer();

                heldItemObject.transform.parent = targetedContainer.targetPosition;
                heldItemObject.transform.localPosition = Vector3.zero;

                targetedContainer.items.Add(heldItemObject);

                heldItemObject = null;
                heldItemType = ItemType.None;
            }
            else if (targetedContainer.items.Count > 0)
            {
                reactionSprites.SetCategoryAndLabel("Face", "Exclamation");
                reactionSprites.ResolveSpriteToSpriteRenderer();

                heldItemType = targetedContainer.items[targetedContainer.items.Count - 1].GetComponent<Item>().itemType;

                heldItemObject = targetedContainer.items[targetedContainer.items.Count - 1];
                heldItemObject.transform.parent = itemTransform;
                heldItemObject.transform.localPosition = Vector3.zero;

                targetedContainer.items.RemoveAt(targetedContainer.items.Count - 1);
            }
        }

        if (targetedNPC != null)
        {
            if (targetedNPC.hasGivingItem && heldItemType == ItemType.None)
            {
                reactionSprites.SetCategoryAndLabel("Face", "Exclamation");
                reactionSprites.ResolveSpriteToSpriteRenderer();

                heldItemType = targetedNPC.currentItem;

                heldItemObject = targetedNPC.currentItemObject;
                heldItemObject.transform.parent = itemTransform;
                heldItemObject.transform.localPosition = Vector3.zero;

                targetedNPC.LoseGivingItem();
            }
            else if (targetedNPC.awaitingItem && heldItemType == targetedNPC.currentItem)
            {
                reactionSprites.SetCategoryAndLabel("Face", "Finish");
                reactionSprites.ResolveSpriteToSpriteRenderer();

                targetedNPC.GetTakingItem(heldItemObject);

                heldItemType = ItemType.None;
            }
        }

        pathData.EndOfPathReached -= DestinationReached;
    }
}
