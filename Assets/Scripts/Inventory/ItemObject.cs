using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public int Id; // �������� �̸��� ������ ���� (Ű)
    public Item item;

    private void Start()
    {
        item = ItemDataManager.Instance.itemData.GetItemByKey(Id);
    }
}
