using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public int ItemID;
    public string Name;
    public string Type;
    public float Price;
    public float Damage;
    public float Critical;
    public int Money;
    public string ItemName;
    public string Description;
    public string IconPath;
}

//json�� �ҷ����� ����
public class ItemInstance
{
    public Item item;
}


[System.Serializable]
public class ItemData
{
    public List<Item> items;
    public Dictionary<int, Item> itemDic = new Dictionary<int, Item>();

    public void Initialize()
    {
        foreach (Item item in items)
        {
            itemDic.Add(item.ItemID, item);
        }
    }

    public Item GetItemByKey(int _id)
    {
        if (itemDic.ContainsKey(_id)) // Ű�� �ִ��� Ȯ��
            return itemDic[_id];

        return null;
    }
}