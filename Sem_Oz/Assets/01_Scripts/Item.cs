
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

    }

    public Color SetType(ItemType _item)
    {
        switch (_item)
        {
            case ItemType.None:
                break;
            case ItemType.Milk:
                sr.color = Color.red;
                break;
            case ItemType.Plank:
                sr.color = Color.blue;
                break;
            case ItemType.Solar_Panel:
                sr.color = Color.green;
                break;
            case ItemType.Book:
                sr.color = Color.cyan;
                break;
            case ItemType.Tomato_Plant:
                sr.color = Color.magenta;
                break;
            default:
                break;
        }

        return sr.color;
    }
}