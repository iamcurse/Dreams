using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public string itemData;
}