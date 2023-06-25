using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public ItemType acceptedTypeForContainer;
}
