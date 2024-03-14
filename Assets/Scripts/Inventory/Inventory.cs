using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Inventory : MonoBehaviour
{
    public static bool activated = false; // �κ��丮 Ȱ��ȭ �� �ٸ� �Է� ���ϰ�
    public static Inventory instance;

    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _slotGrid;
    [SerializeField] private Slot[] _slots; //���Ե��� �迭�� �Ҵ�

    // public Transform dropPosition; //��� ��ġ

    [Header("Selected Item")]
    private Item _selectedItem;
    private int _selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    //public TextMeshProUGUI selectedItemStat;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;


    // ������ �ѱ�� ���� ����
    private int _maxSlot = 12;
    private int _currentPage = 1;
    private int _totalPage = 3;

    public Button rightBtn;
    public Button leftBtn;


    void Start()
    {
        //��Ȱ��ȭ ���¿��� ������Ʈ�� �� ã��
        // slots = _slotGrid.GetComponentsInChildren<Slot>();

        instance = this;
        ClearSeletecItemWindow(); //������ ���� �����ִ� ������Ʈ ��Ȱ��ȭ
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
        var endIndex = Mathf.Min(startIndex + _maxSlot, _slots.Length);

        // ���� �������� �ִ� ���Ը� Ȱ��ȭ (1~12��)
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
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
    public void PickupItem(Item _item, int _count = 1)
    {
        //Debug.Log(_item);
        //���� �������� �ƴҰ�쿡�� (���� �������� ī��Ʈ x)
        if (_item.Type != "Equip")
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)
                {
                    //Debug.Log(_slots[i].item.ItemName);
                    if (_slots[i].item.ItemName == _item.ItemName)
                    {
                        _slots[i].SlotCount(_count);
                        return;
                    }
                }
            }
        }

        // ���� �̸��� ������ �ִ� �������� ���ٸ� ������ �߰� (�� ������ ���� ��)
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                Debug.Log(i);
                if (_slots[i].item.Name == null || _slots[i].item.Name == "")
                {
                    _slots[i].AddItem(_item, _count);
                    return;
                }

                if (string.IsNullOrEmpty(_slots[i].item.ItemName))
                {
                    // Debug.Log(_count);
                    _slots[i].AddItem(_item, _count);
                    return;
                }
            }
            else
            {
                _slots[i].AddItem(_item, _count);
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
        var endIndex = Mathf.Min(startIndex + _maxSlot, _slots.Length);

        Debug.Log(startIndex);
        Debug.Log(endIndex);
        // ���� �������� �ִ� ���Ը� Ȱ��ȭ (1~12��)
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
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
        var endIndex = Mathf.Min(startIndex + _maxSlot, _slots.Length);

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        rightBtn.gameObject.SetActive(true);
        leftBtn.gameObject.SetActive(_currentPage > 1);
    }

    // ����������
    public void NextOnClick()
    {
        NextPage();
    }

    // ����������
    public void PrevOnClick()
    {
        PrevPage();
    }

    // ������
    public void ExitOnClick()
    {
        _inventoryUI.SetActive(false);
    }

    //������ ����� ��
    private void ClearSeletecItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    //������ ���� ���ý� ������ ������ ���̰�
    public void UpdateSelectedItemInfo(Item selectedItem)
    {
        _selectedItem = selectedItem;
        selectedItemName.text = selectedItem.ItemName;
        selectedItemDescription.text = selectedItem.Description;
        //selectedItemStat.text = selectedItem.Price.ToString();      

        // ����: ��� ��ư�� �׻� Ȱ��ȭ, ���� ��ư�� ���� ������ ��쿡�� Ȱ��ȭ�ǵ��� ����
        useButton.SetActive(true);
        dropButton.SetActive(true);
    }

    //���� ������ ������ ����ϱ�
    public void UseButtonOnClick()
    {
        if (_selectedItem != null)
        {
            // ������ ã�� ������ ����
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item == _selectedItem)
                {
                    _slots[i].ClearSlot(); //���� ����
                    break;
                }
            }
            ClearSeletecItemWindow();
        }
    }
}
