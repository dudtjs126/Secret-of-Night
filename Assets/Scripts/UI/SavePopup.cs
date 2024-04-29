using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePopup : UIBase
{
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _deleteButton;
    private List<CharacterSlot> slots;

    public int selectedSlotNumber;
    public bool hasClickedSlot;
    private void Awake()
    {
        slots = new(GameManager.Instance.playerManager.maxSlotDataNumber);
        selectedSlotNumber = -1;
    }
    private void Start()
    {
        // 슬록을 5개 정도 생성한다
        for (int i = 0; i < GameManager.Instance.playerManager.maxSlotDataNumber; i++)
        {
            CreateCharacterSlot(i);
        }

        _confirmButton.onClick.AddListener(() =>
        {
            if (selectedSlotNumber == -1)
                return;
            GameManager.Instance.playerManager.playerData.SlotNumber = selectedSlotNumber;
            GameManager.Instance.playerManager.playerData.SaveData();
            if (GameManager.Instance.playerManager.playerDatas.ContainsKey(selectedSlotNumber))
            {
                GameManager.Instance.playerManager.playerDatas[selectedSlotNumber] = GameManager.Instance.playerManager.playerData;
            }
            else
            {
                GameManager.Instance.playerManager.playerDatas.Add(selectedSlotNumber, GameManager.Instance.playerManager.playerData);
            }

            slots[selectedSlotNumber].ReFreshData();
        });
        _cancelButton.onClick.AddListener(() => { Destroy(gameObject); });
        _deleteButton.onClick.AddListener(() =>
        {
            if (selectedSlotNumber == -1)
                return;
            GameManager.Instance.playerManager.playerDatas[selectedSlotNumber].DeleteData();
            GameManager.Instance.playerManager.playerDatas.Remove(selectedSlotNumber);
            slots[selectedSlotNumber].ReFreshData();
        });

    }
    void CreateCharacterSlot(int slotNumber)
    {
        slots.Add(GameManager.Instance.uiManager.MakeSubItem<CharacterSlot>(parent: _slotRoot));

        slots[slotNumber].SetSlotNumber(slotNumber);
        slots[slotNumber].Initialize();
    }
    public CharacterSlot GetSlot(int index)
    {
        return slots[index];
    }
}
