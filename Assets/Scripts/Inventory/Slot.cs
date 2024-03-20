using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
    public int itemCount;
    public TextMeshProUGUI _itemCountText;
    private ItemSlot curSlot; //���� ���� 
    private Outline _outline; //���� ������ �׵θ�

    public int index;
    public bool equipped;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        _outline.enabled = equipped;
    }

    public void ItemImage(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void Set(ItemSlot _slot)
    {
        curSlot = _slot;
        itemImage.sprite = Resources.Load<Sprite>(_slot.item.IconPath);
        itemImage.preserveAspect = true; //���� �̹��� ������ �θ� �°� ����
        _itemCountText.text = _slot.count > 1 ? _slot.count.ToString() : string.Empty;

        if (_outline != null)
        {
            _outline.enabled = equipped;
        }

        ItemImage(1); //������ ǥ���ϱ� ���� ���� 1
    }


    public void ClearSlot()
    {
        curSlot = null;
        itemImage.sprite = null;
        _itemCountText.text = string.Empty;

        ItemImage(0);
    }

    // ���� Ŭ���� ������ ���� ���̰�
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.instance.SelectItem(index);
        }
    }
}
