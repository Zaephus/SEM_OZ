
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.U2D.Animation;

public class PlayerController : MonoBehaviour {

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

    // private AIDestinationSetter pathSetter;
    private AIPath pathData;
    
    private void Start() {
        // pathSetter = transform.GetComponent<AIDestinationSetter>();
        pathData = transform.GetComponent<AIPath>();
    }

    private void Update() {

        if(Input.GetMouseButtonDown(0)) {
            reactionSprites.SetCategoryAndLabel("Face", "Normal");
            reactionSprites.ResolveSpriteToSpriteRenderer();
            pathData.EndOfPathReached -= DestinationReached;

            Vector3 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.GetComponent<NPCBehaviour>() != null) {
                    targetedNPC = hit.collider.GetComponent<NPCBehaviour>();
                    if(targetedNPC.canGiveItem || targetedNPC.awaitingItem) {
                        pathData.EndOfPathReached += DestinationReached;
                    }
                    else {
                        targetedNPC = null;
                    }
                }
                else {
                    targetedNPC = null;
                }
            }

            target.position = clickedPos;
            pathData.SearchPath();

        }
        
    }

    private void DestinationReached() {
        Debug.Log("reached");
        if(targetedNPC != null) {
            if(targetedNPC.hasGivingItem && heldItemType == ItemType.None) {
                reactionSprites.SetCategoryAndLabel("Face", "Exclamation");
                reactionSprites.ResolveSpriteToSpriteRenderer();
                heldItemType = targetedNPC.currentItem;
                heldItemObject = targetedNPC.currentItemObject;
                heldItemObject.transform.parent = itemTransform;
                heldItemObject.transform.position = itemTransform.position;
                targetedNPC.LoseGivingItem();
            }
            else if(targetedNPC.awaitingItem && heldItemType == targetedNPC.currentItem) {
                reactionSprites.SetCategoryAndLabel("Face", "Finish" );
                reactionSprites.ResolveSpriteToSpriteRenderer();
            }
        }
        pathData.EndOfPathReached -= DestinationReached;
    }
    
}