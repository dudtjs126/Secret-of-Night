
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot
{
    public Item item;
    public int count;
}

public class Inventory : MonoBehaviour
{
    private bool activated;
    private bool _speedItemUse;
    public static Inventory instance;
    private CameraHandler _camera;
    private EquipController _equipController;

    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _slotGrid;

    [SerializeField] private Slot[] _uiSlots; //���Ե��� �迭�� �Ҵ�
    public ItemSlot[] slots; // ������ ����

    public Transform dropPosition; // ������ ��� ��ġ

    [Header("Selected Item")]
    private ItemSlot _selectedItem;
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

    private int curEquipIndex;

    public Button rightBtn;
    public Button leftBtn;


    private void Awake()
    {
        instance = this;
        _camera = FindObjectOfType<CameraHandler>();
        _equipController = GetComponent<EquipController>();
    }

    void Start()
    {
        _inventoryUI.SetActive(false);
        slots = new ItemSlot[_uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            _uiSlots[i].index = i;
            _uiSlots[i].ClearSlot();
        }

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
                _camera.enabled = false; // ī�޶� ��Ȱ��

            }
            else
            {
                CloseInventory();
                _camera.enabled = true;
            }
        }
    }

    private void OpenInventory()
    {
        var startIndex = (_currentPage - 1) * _maxSlot;
        var endIndex = Mathf.Min(startIndex + _maxSlot, _uiSlots.Length);

        // ���� �������� �ִ� ���Ը� Ȱ��ȭ (1~12��)
        for (int i = 0; i < _uiSlots.Length; i++)
        {
            _uiSlots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        //���콺 Ŀ�� ǥ��
        Cursor.lockState = CursorLockMode.None;
        _inventoryUI.SetActive(true);
    }

    private void CloseInventory()
    {
        _inventoryUI.SetActive(false);
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
        var endIndex = Mathf.Min(startIndex + _maxSlot, _uiSlots.Length);

        Debug.Log(startIndex);
        Debug.Log(endIndex);
        // ���� �������� �ִ� ���Ը� Ȱ��ȭ (1~12��)
        for (int i = 0; i < _uiSlots.Length; i++)
        {
            _uiSlots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
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
        var endIndex = Mathf.Min(startIndex + _maxSlot, _uiSlots.Length);

        for (int i = 0; i < _uiSlots.Length; i++)
        {
            _uiSlots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        rightBtn.gameObject.SetActive(true);
        leftBtn.gameObject.SetActive(_currentPage > 1);
    }

    // ����������
    public void OnNext()
    {
        NextPage();
    }

    // ����������
    public void OnPrev()
    {
        PrevPage();
    }

    // ������
    public void OnExit()
    {
        _inventoryUI.SetActive(false);
    }

    //������ �߰�
    public void AddItem(Item _item)
    {
        if (_item.Type == "using")
        {
            ItemSlot slotStack = GetItemStack(_item);
            if (slotStack != null)
            {
                slotStack.count++;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot(); // �� ����

        if (emptySlot != null)
        {
            emptySlot.item = _item;
            emptySlot.count = 1;
            UpdateUI();
            return;
        }
        //�� ���� ������
    }

    // ������ ���� �߰�
    private ItemSlot GetItemStack(Item _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item && slots[i].count < _item.MaxAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    //������ ���� ���ý� ������ ������ ���̰�
    public void SelectItem(int _index)
    {
        if (slots[_index].item == null)
        {
            return;
        }
        _selectedItem = slots[_index];
        _selectedItemIndex = _index;
        selectedItemName.text = _selectedItem.item.ItemName;
        selectedItemDescription.text = _selectedItem.item.Description;
        //selectedItemStat.text = selectedItem.Price.ToString();      

        // ����: ��� ��ư�� �׻� Ȱ��ȭ, ���� ��ư�� ���� ������ ��쿡�� Ȱ��ȭ�ǵ��� ����
        useButton.SetActive(_selectedItem.item.Type == "using");
        equipButton.SetActive(_selectedItem.item.Type == "Equip" && !_uiSlots[_index].equipped);
        unEquipButton.SetActive(_selectedItem.item.Type == "Equip" && _uiSlots[_index].equipped);
        dropButton.SetActive(true);
    }

    //������ ����� ��
    private void ClearSeletecItemWindow()
    {
        _selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    void UpdateUI() //UI ������Ʈ
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                _uiSlots[i].Set(slots[i]);
            else
                _uiSlots[i].ClearSlot();
        }
    }

    //���� ������ ������ ����ϱ�
    public void OnUseButton()
    {
        if (_selectedItem.item.Type == "using")
        {
            switch (_selectedItem.item.ItemID)
            {
                case 1:
                    GameManager.Instance.itemManager.SmallHpPotion(_selectedItem.item.Price); break;
                case 2:
                    GameManager.Instance.itemManager.BigHpPotion(_selectedItem.item.Price); break;
                case 3:
                    GameManager.Instance.itemManager.SmallMpPotion(_selectedItem.item.Price); break;
                case 4:
                    GameManager.Instance.itemManager.BigMpPotion(_selectedItem.item.Price); break;
                case 5:
                    GameManager.Instance.itemManager.SmallSpPotion(_selectedItem.item.Price); break;
                case 6:
                    GameManager.Instance.itemManager.BigSpPotion(_selectedItem.item.Price); break;
                case 7:
                    if (!GameManager.Instance.itemManager.speedItemInUse) // �ߺ� ��� ����
                    {
                        GameManager.Instance.itemManager.SpeedPotion(_selectedItem.item.Price);

                    }
                    else
                    {
                        Debug.Log("�̹� ������ �����");
                        return; // �ߺ� ����̹Ƿ� ���� �ڵ� �������� ����
                    }
                    break;

            }
        }

        RemoveSelectedItem();
    }

    // ������ ����
    public void OnEquipBtton()
    {
        if (_uiSlots[curEquipIndex].equipped) //_selectedItemIndex�� �ϸ� ��� �������� ����
        {
            UnEquip(curEquipIndex);
            _equipController.UnEquipWeapon(); //�⺻ ���ݷ����� ���ư���
        }

        _uiSlots[_selectedItemIndex].equipped = true;
        curEquipIndex = _selectedItemIndex;
        _equipController.NewEquip(_selectedItem.item);
        _equipController.EquipWeapon(_selectedItem.item.ItemID, _selectedItem.item.Damage); //���ݷ� ����
        UpdateUI();

        SelectItem(_selectedItemIndex);
    }
    void UnEquip(int _index)
    {
        _uiSlots[_index].equipped = false;
        _equipController.UnEquip();
        _equipController.UnEquipWeapon(); //�⺻ ���ݷ����� ���ư���
        UpdateUI();

        if (_selectedItemIndex == _index)
        {
            SelectItem(_index);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(_selectedItemIndex);
    }

    public void OnDropButton()
    {
        ThrowItem(_selectedItem.item);
        RemoveSelectedItem();
    }

    private void ThrowItem(Item _item)
    {
        Instantiate(_item.Prefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    private void RemoveSelectedItem()
    {
        _selectedItem.count--;

        //������������ ������ 0���� ǥ�õǱ� ������
        if (_selectedItem.count <= 0)
        {
            if (_uiSlots[_selectedItemIndex].equipped) //���� �����ϰ� �ִ� ������ �ε������� Ȯ���ؾ��� (_selectItemIndex)
            {
                UnEquip(_selectedItemIndex);
            }

            _selectedItem.item = null;
            ClearSeletecItemWindow();
        }


        UpdateUI();
    }

}
