
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed = 5.0f;

    [SerializeField]
    private float stoppingDistance = 0.1f;

    [SerializeField]
    private ItemType heldItemType = ItemType.None;

    private bool isMoving = false;
    
    private void Start() {}

    private void Update() {

        if(Input.GetMouseButtonDown(0)) {

            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.GetComponent<NPCController>() != null) {
                    NPCController npc = hit.collider.GetComponent<NPCController>();
                    if(npc.hasItem) {
                        StartCoroutine(MoveToPosition(new Vector3(target.x, target.y, transform.position.z), npc));
                    }
                }
            }

            StartCoroutine(MoveToPosition(new Vector3(target.x, target.y, transform.position.z)));
        }
        
    }

    private IEnumerator MoveToPosition(Vector3 _target, NPCController _npc = null) {

        if(stoppingDistance <= 0.0f) {
            stoppingDistance = Mathf.Epsilon;
        }
        
        isMoving = false;
        yield return new WaitForEndOfFrame();
        isMoving = true;

        while(isMoving && Vector3.Distance(_target, transform.position) > stoppingDistance) {
            transform.position = Vector3.MoveTowards(transform.position, _target, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if(_npc != null) {
            PickupItem();
        }

    }

    private void PickupItem() {

    }
    
}