
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
    public static Inventory instance;
    private UseItem useItem;
    private CameraHandler _camera;

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
        _camera = FindAnyObjectByType<CameraHandler>();
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
    public void AddItem(Item item)
    {
        if (item.Type == "using")
        {
            ItemSlot slotStack = GetItemStack(item);
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
            emptySlot.item = item;
            emptySlot.count = 1;
            UpdateUI();
            return;
        }
        //�� ���� ������
    }

    // ������ ���� �߰�
    private ItemSlot GetItemStack(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].count < item.MaxAmount)
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
    public void SelectItem(int index)
    {
        if (slots[index].item == null)
        {
            return;
        }
        _selectedItem = slots[index];
        _selectedItemIndex = index;
        selectedItemName.text = _selectedItem.item.ItemName;
        selectedItemDescription.text = _selectedItem.item.Description;
        //selectedItemStat.text = selectedItem.Price.ToString();      

        // ����: ��� ��ư�� �׻� Ȱ��ȭ, ���� ��ư�� ���� ������ ��쿡�� Ȱ��ȭ�ǵ��� ����
        useButton.SetActive(_selectedItem.item.Type == "using");
        equipButton.SetActive(_selectedItem.item.Type == "Equip" && !_uiSlots[index].equipped);
        unEquipButton.SetActive(_selectedItem.item.Type == "Equip" && _uiSlots[index].equipped);
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
            for (int i = 0; i < _selectedItem.item.Type.Length; i++)
            {
                switch (_selectedItem.item.ItemID)
                {
                    case 1:
                        useItem.SmallHpPotion(_selectedItem.item.Price); break;

                }
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
        }

        _uiSlots[_selectedItemIndex].equipped = true;
        curEquipIndex = _selectedItemIndex;
        EquipManager.instance.NewEquip(_selectedItem.item);
        UpdateUI();

        SelectItem(_selectedItemIndex);
    }
    void UnEquip(int index)
    {
        _uiSlots[index].equipped = false;
        EquipManager.instance.UnEquip();
        UpdateUI();

        if (_selectedItemIndex == index)
        {
            SelectItem(index);
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
