using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public string name; // �������� �̸��� ������ ���� (Ű)
    public Item item;

    private void Start()
    {
        item = ItemDataManager.Instance.itemData.GetItemByKey(name);
    }
}
