using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public int Id; // �������� �̸��� ������ ���� (Ű)
    public Item item;

    private void Start()
    {
        item = GameManager.Instance.dataManager.itemData.GetData(Id);
    }
}
