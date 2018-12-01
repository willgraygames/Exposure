using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string itemName;                     //The name of the item
    public int fireValue;                       //The value this item adds to the fire if burned
    public int pitvalue;                        //The value this item adds to the pit if sacrificed
    public bool burnable;                       //Whether or not this object is burned in the fire

    void Start ()
    {
        gameObject.name = itemName;
    }
}
