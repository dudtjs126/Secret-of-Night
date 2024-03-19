
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

    [SerializeField] private Slot[] _uiSlots; //슬롯들을 배열로 할당
    public ItemSlot[] slots; // 아이템 정보

    public Transform dropPosition; // 아이템 드랍 위치

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

    // 페이지 넘기기 위한 변수
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

        ClearSeletecItemWindow(); //아이템 정보 보여주는 오브젝트 비활성화

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
                _camera.enabled = false; // 카메라 비활성

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

        // 현재 페이지에 있는 슬롯만 활성화 (1~12개)
        for (int i = 0; i < _uiSlots.Length; i++)
        {
            _uiSlots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        //마우스 커서 표시
        Cursor.lockState = CursorLockMode.None;
        _inventoryUI.SetActive(true);
    }

    private void CloseInventory()
    {
        _inventoryUI.SetActive(false);
    }

    // 다음 슬롯 창
    private void NextPage()
    {
        if (_currentPage == _totalPage)
        {
            return;
        }
        _currentPage++;
        Debug.Log(_currentPage + " 페이지");

        //(0~11, 12~23, 24~35번 슬롯)
        var startIndex = (_currentPage - 1) * _maxSlot;
        var endIndex = Mathf.Min(startIndex + _maxSlot, _uiSlots.Length);

        Debug.Log(startIndex);
        Debug.Log(endIndex);
        // 현재 페이지에 있는 슬롯만 활성화 (1~12개)
        for (int i = 0; i < _uiSlots.Length; i++)
        {
            _uiSlots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        leftBtn.gameObject.SetActive(true);
        rightBtn.gameObject.SetActive(_currentPage < _totalPage);
    }

    // 이전 슬롯 창
    private void PrevPage()
    {
        if (_currentPage == 1)
        {
            return;
        }
        _currentPage--;
        Debug.Log(_currentPage + " 페이지");

        var startIndex = (_currentPage - 1) * _maxSlot;
        var endIndex = Mathf.Min(startIndex + _maxSlot, _uiSlots.Length);

        for (int i = 0; i < _uiSlots.Length; i++)
        {
            _uiSlots[i].gameObject.SetActive(i >= startIndex && i < endIndex);
        }

        rightBtn.gameObject.SetActive(true);
        leftBtn.gameObject.SetActive(_currentPage > 1);
    }

    // 다음페이지
    public void OnNext()
    {
        NextPage();
    }

    // 이전페이지
    public void OnPrev()
    {
        PrevPage();
    }

    // 나가기
    public void OnExit()
    {
        _inventoryUI.SetActive(false);
    }

    //아이템 추가
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

        ItemSlot emptySlot = GetEmptySlot(); // 빈 슬롯

        if (emptySlot != null)
        {
            emptySlot.item = _item;
            emptySlot.count = 1;
            UpdateUI();
            return;
        }
        //꽉 차면 버리게
    }

    // 아이템 수량 추가
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

    //아이템 슬롯 선택시 아이템 설명이 보이게
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

        // 예시: 사용 버튼은 항상 활성화, 장착 버튼은 장착 가능한 경우에만 활성화되도록 설정
        useButton.SetActive(_selectedItem.item.Type == "using");
        equipButton.SetActive(_selectedItem.item.Type == "Equip" && !_uiSlots[_index].equipped);
        unEquipButton.SetActive(_selectedItem.item.Type == "Equip" && _uiSlots[_index].equipped);
        dropButton.SetActive(true);
    }

    //슬롯이 비워질 때
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

    void UpdateUI() //UI 업데이트
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                _uiSlots[i].Set(slots[i]);
            else
                _uiSlots[i].ClearSlot();
        }
    }

    //현재 선택한 아이템 사용하기
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
                    if (!GameManager.Instance.itemManager.speedItemInUse) // 중복 사용 방지
                    {
                        GameManager.Instance.itemManager.SpeedPotion(_selectedItem.item.Price);

                    }
                    else
                    {
                        Debug.Log("이미 아이템 사용중");
                        return; // 중복 사용이므로 이후 코드 실행하지 않음
                    }
                    break;

            }
        }

        RemoveSelectedItem();
    }

    // 아이템 장착
    public void OnEquipBtton()
    {
        if (_uiSlots[curEquipIndex].equipped) //_selectedItemIndex로 하면 장비가 이중으로 껴짐
        {
            UnEquip(curEquipIndex);
            _equipController.UnEquipWeapon(); //기본 공격력으로 돌아가게
        }

        _uiSlots[_selectedItemIndex].equipped = true;
        curEquipIndex = _selectedItemIndex;
        _equipController.NewEquip(_selectedItem.item);
        _equipController.EquipWeapon(_selectedItem.item.ItemID, _selectedItem.item.Damage); //공격력 증가
        UpdateUI();

        SelectItem(_selectedItemIndex);
    }
    void UnEquip(int _index)
    {
        _uiSlots[_index].equipped = false;
        _equipController.UnEquip();
        _equipController.UnEquipWeapon(); //기본 공격력으로 돌아가게
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

        //장착아이템은 수량이 0으로 표시되기 때문에
        if (_selectedItem.count <= 0)
        {
            if (_uiSlots[_selectedItemIndex].equipped) //현재 장착하고 있는 아이템 인덱스에서 확인해야함 (_selectItemIndex)
            {
                UnEquip(_selectedItemIndex);
            }

            _selectedItem.item = null;
            ClearSeletecItemWindow();
        }


        UpdateUI();
    }

}
