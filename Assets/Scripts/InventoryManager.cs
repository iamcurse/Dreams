using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Manager;
    public Inventory inventory;
    
    public Transform inventoryUI;
    public GameObject inventoryItemTemplate;
    
    private void Awake(){
        if (Manager == null)
        {
            Manager = this;
        }
    }
    
    private void FixedUpdate() {
        ListItems();
    }

    public void Add(Item item)
    {
        var vowel = item.itemName.Substring(0, 1);
        if (vowel == "A" || vowel == "E" || vowel == "I" || vowel == "O" || vowel == "U"|| vowel == "e" || vowel == "i" || vowel == "o" || vowel == "u" || vowel == "u") {
            //_dialogueTrigger.TriggerDialogue("You Found an " + item.itemName);
        } else {
            //_dialogueTrigger.TriggerDialogue("You Found a " + item.itemName);
        }
        inventory.items.Add(item);
    }
    
    public bool CheckItem(Item item)
    {
        return inventory.items.Contains(item);
    }
    
    public void Remove(Item item) {
        inventory.items.Remove(item);
    }

    private void ListItems()
    {
        foreach (Transform item in inventoryUI)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in inventory.items)
        {
            try
            {
                var itemGameObject = Instantiate(inventoryItemTemplate, inventoryUI);
                var itemName = itemGameObject.transform.Find("ItemName").GetComponent<TMP_Text>();
                var itemIcon = itemGameObject.transform.Find("ItemIcon").GetComponent<Image>();

                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
    }
}
