using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Transform targetPosition;
    public List<GameObject> items = new List<GameObject>();
    public ItemType acceptedTypeForContainer;
}
