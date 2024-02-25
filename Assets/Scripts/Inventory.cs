using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Item/ Create New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> items;
}