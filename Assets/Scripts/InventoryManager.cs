using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _manager;
    [SerializeField] private Inventory inventory;
    private Transform _inventoryUI;
    [SerializeField] private GameObject inventoryItemTemplate;
    
    private void Awake(){
        if (_manager == null)
        {
            _manager = this;
        }
    }

    private void Start()
    {
        _inventoryUI = this.gameObject.transform;
        ClearInventory();
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
    
    public int CheckAmount(Item item)
    {
        return inventory.items.FindAll(x => x == item).Count;
    }
    
    public void Remove(Item item) {
        inventory.items.Remove(item);
    }

    private void ListItems()
    {
        foreach (Transform item in _inventoryUI)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in inventory.items)
        {
            try
            {
                var itemGameObject = Instantiate(inventoryItemTemplate, _inventoryUI);
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
    public void ClearInventory()
    {
        inventory.items.Clear();
    }
}
