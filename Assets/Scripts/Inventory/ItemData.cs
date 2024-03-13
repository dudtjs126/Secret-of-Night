using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string Name;
    public string Type;
    public float Price;
    public float Damage;
    public float Critical;
    public int Money;
    public string ItemName;
    public string Description;
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
    public Dictionary<string, Item> itemDic = new Dictionary<string, Item>();

    public void Initialize()
    {
        foreach (Item item in items)
        {
            itemDic.Add(item.Name, item);
        }
    }

    public Item GetItemByKey(string name)
    {
        if (itemDic.ContainsKey(name)) // Ű�� �ִ��� Ȯ��
            return itemDic[name];

        return null;
    }
}
