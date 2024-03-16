using System;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _manager;
    [SerializeField] private Inventory inventory;
    private Transform _inventoryUI;
    [SerializeField] private GameObject inventoryItemTemplate;
    [SerializeField] private Inventory itemDatabase;
    
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

    private void OnEnable()
    {
        Lua.RegisterFunction("AddItem", this, SymbolExtensions.GetMethodInfo(() => AddItem(0)));
        Lua.RegisterFunction("CheckItem", this, SymbolExtensions.GetMethodInfo(() => CheckItem(0)));
        Lua.RegisterFunction("CheckItemAmount", this, SymbolExtensions.GetMethodInfo(() => CheckItemAmount(0)));
        Lua.RegisterFunction("RemoveItem", this, SymbolExtensions.GetMethodInfo(() => RemoveItem(0)));
        Lua.RegisterFunction("ClearInventory", this, SymbolExtensions.GetMethodInfo(() => ClearInventory()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("AddItem");
        Lua.UnregisterFunction("CheckItem");
        Lua.UnregisterFunction("CheckItemAmount");
        Lua.UnregisterFunction("RemoveItem");
        Lua.UnregisterFunction("ClearInventory");
    }

    private void FixedUpdate() => ListItems();

    public void AddItem(Item item) => inventory.items.Add(item);

    private void AddItem(double itemID)
    {
        var item = itemDatabase.items[(int)itemID];
        inventory.items.Add(item);
    }
    
    public bool CheckItem(Item item) => inventory.items.Contains(item);

    private bool CheckItem(double itemID)
    {
        var item = itemDatabase.items[(int)itemID];
        return inventory.items.Contains(item);
    }
    public int CheckItemAmount(Item item) => inventory.items.FindAll(x => x == item).Count;

    private double CheckItemAmount(double itemID)
    {
        var item = itemDatabase.items[(int)itemID];
        return inventory.items.FindAll(x => x == item).Count;
    }
    
    public void RemoveItem(Item item) => inventory.items.Remove(item);

    private void RemoveItem(double itemID) {
        var item = itemDatabase.items[(int)itemID];
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

    private void ClearInventory() => inventory.items.Clear();
}
