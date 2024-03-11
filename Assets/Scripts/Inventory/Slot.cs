using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemData item;
    public Image itemImage;
    public int itemCount;

    [SerializeField] private TextMeshProUGUI _itemCountText;

    //������ �̹��� ���� ���� (�⺻ 0)
    private void ItemImage(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //������ ���Կ� �߰�
    public void AddItem(ItemData _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.icon;

        //���� �������� �ƴϸ� ī��Ʈ
        if (item.type != ItemType.Equipment)
        {
            _itemCountText.text = itemCount.ToString();
        }
        else
        {
            _itemCountText.text = "0";
        }

        ItemImage(1); //������ ���� 1
    }

    // ������ ���� ������Ʈ
    public void SlotCount(int _count)
    {
        itemCount += _count;
        _itemCountText.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    //�������� ����� �� �ʱ�ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        ItemImage(0);

        _itemCountText.text = "0";
    }
}
