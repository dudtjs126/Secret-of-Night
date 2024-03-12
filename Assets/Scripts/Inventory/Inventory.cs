using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool activated = false; // �κ��丮 Ȱ��ȭ �� �ٸ� �Է� ���ϰ�

    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _slotGrid;
    [SerializeField] private Slot[] slots; //���Ե��� �迭�� �Ҵ�


    // ������ �ѱ�� ���� ����
    private int _maxSlot = 12;
    private int _currentPage = 1;
    private int _totalPage = 3;

    public Button rightBtn;
    public Button leftBtn;

    void Start()
    {
        // slots = _slotGrid.GetComponentsInChildren<Slot>();
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
        var startIndex = (_currentPage - 1) * _maxSlot;
        var endIndex = Mathf.Min(startIndex + _maxSlot, slots.Length);

        // ���� �������� �ִ� ���Ը� Ȱ��ȭ (1~12��)
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        //���콺 Ŀ�� ǥ��
        Cursor.lockState = CursorLockMode.None;
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


    // ���� ���� â
    private void NextPage()
    {
        if (_currentPage == _totalPage)
        {
            return;
        }
        _currentPage++;
        Debug.Log(_currentPage + " ������");

        //(0~11, 12~23, 24~35�� ����)
        var startIndex = (_currentPage - 1) * _maxSlot;
        var endIndex = Mathf.Min(startIndex + _maxSlot, slots.Length);

        Debug.Log(startIndex);
        Debug.Log(endIndex);
        // ���� �������� �ִ� ���Ը� Ȱ��ȭ (1~12��)
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        leftBtn.gameObject.SetActive(true);
        rightBtn.gameObject.SetActive(_currentPage < _totalPage);
    }

    // ���� ���� â
    private void PrevPage()
    {
        if (_currentPage == 1)
        {
            return;
        }
        _currentPage--;
        Debug.Log(_currentPage + " ������");

        var startIndex = (_currentPage - 1) * _maxSlot;
        var endIndex = Mathf.Min(startIndex + _maxSlot, slots.Length);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        rightBtn.gameObject.SetActive(true);
        leftBtn.gameObject.SetActive(_currentPage > 1);
    }


    public void NextOnClick()
    {
        NextPage();
    }

    public void PrevOnClick()
    {
        PrevPage();
    }
}
