
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Item : MonoBehaviour
{
    public List<Sprite> itemOptions = new List<Sprite>();
    private SpriteRenderer sr;
    [HideInInspector] public ItemType itemType;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public Sprite SetType(ItemType _item)
    {
        itemType = _item;

        switch (_item)
        {
            case ItemType.None:
                break;
            case ItemType.Milk:
                sr.sprite = itemOptions[0];
                break;
            case ItemType.Plank:
                sr.sprite = itemOptions[1];
                break;
            case ItemType.Solar_Panel:
                sr.sprite = itemOptions[2];
                break;
            case ItemType.Book:
                sr.sprite = itemOptions[3];
                break;
            case ItemType.Tomato_Plant:
                sr.sprite = itemOptions[4];
                break;
            default:
                break;
        }

        return sr.sprite;
    }
}