using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool activated = false; // �κ��丮 Ȱ��ȭ �� �ٸ� �Է� ���ϰ�

    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _slotGrid;

    private Slot[] slots; //���Ե��� �迭�� �Ҵ�

    void Start()
    {
        slots = _slotGrid.GetComponentsInChildren<Slot>();
    }


    void Update()
    {
        OpenInventoryUI();
    }

    private void OpenInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activated = !activated;

            if (activated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        _inventoryUI.SetActive(true);
    }

    private void CloseInventory()
    {
        _inventoryUI?.SetActive(false);
    }

    //������ ȹ��, ���� �̸��� ������ �ִ� �������� ��쿡 +1
    public void PickupItem(ItemData _item, int _count = 1)
    {
        //���� �������� �ƴҰ�쿡�� (���� �������� ī��Ʈ x)
        if (ItemType.Equipment != _item.type)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.name == _item.name)
                    {
                        slots[i].SlotCount(_count);
                        return;
                    }
                }
            }
        }

        // ���� �̸��� ������ �ִ� �������� ���ٸ� ������ �߰� (�� ������ ���� ��)
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item);
                return;
            }
        }
    }
}
