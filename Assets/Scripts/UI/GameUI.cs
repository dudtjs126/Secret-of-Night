using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : UIBase
{
    //캐싱용
    private PlayerGameData playerData;

    [Header("GamePlayUI")]
    [SerializeField] private GameObject _gamePlayUI;

    [Header("PlayerStatus")]
    [SerializeField] private Image _playerHPImage;
    [SerializeField] private Image _playerMPImage;
    [SerializeField] private Image _playerExpImage;

    [Header("BossInfo")]
    [SerializeField] private GameObject _monsterInfo;
    [SerializeField] private Image _bossHPImage;
    [SerializeField] private TextMeshProUGUI _bossHPText;
    [SerializeField] private TextMeshProUGUI _bossName;
    private Phase1Boss _boss;

    [Header("GameEndUI")]
    [SerializeField] private GameObject _gameEndUI;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _gameOutButton;
    [SerializeField] private Button _gameEndButton;

    [Header("SkillSlot")]
    [SerializeField] private SkillSlotUI _skillSlotUI;
    private void Start()
    {
        GameManager.Instance.inputManager.UIActions.Option.started += ToggleUI;
        GameManager.Instance.playerManager.playerData.OnDie += OpenGameOverPopup;
        playerData = GameManager.Instance.playerManager.playerData;

        //BossSetting
        if (SceneManager.GetActiveScene().name == "BossMap")
        {
            _monsterInfo.SetActive(true);
            BossScene.OnBossSpawned.AddListener(SetBossUI);
        }

        //Status Change
        playerData.OnHPChange += UpdateHP;
        playerData.OnMPChange += UpdateMP;
        playerData.OnExpChange += UpdateExp;
        //GameEndUI
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _gameOutButton.onClick.AddListener(OnGameOutButtonClick);
        _gameEndButton.onClick.AddListener(OnGameEndButtonClick);
        _gameEndUI.SetActive(false);

        _skillSlotUI.Initialize();

        UpdateUI();
    }



    private void OpenGameOverPopup()
    {
        GameManager.Instance.uiManager.ShowPopupUI<GameOverPopup>();
        GameManager.Instance.playerManager.playerData.OnDie -= OpenGameOverPopup;
    }

    private void Update()
    {
        if (_boss == null)
            return;

        _bossHPImage.fillAmount = _boss.bossMonsterData.HP / _boss.maxHP;
        _bossHPText.text = $"{(int)_boss.bossMonsterData.HP} / {(int)_boss.maxHP}";
    }
    private void SetBossUI(Phase1Boss boss)
    {
        _boss = boss;
        _bossName.text = _boss.bossMonsterData.Name;
    }

    private void OnContinueButtonClick()
    {
        _gamePlayUI.SetActive(true);
        _gameEndUI.SetActive(false);
        GameManager.Instance.inputManager.EnablePlayerAction();
    }
    private void OnGameOutButtonClick()
    {
        GameManager.Instance.sceneManager.LoadSceneAsync(Scene.GameStart);
    }
    private void OnGameEndButtonClick()
    {
#if UNITY_EDITOR        
        GameManager.Instance.playerManager.playerData.SaveData();
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UpdateHP()
    {
        _playerHPImage.fillAmount = playerData.CurHP / playerData.MaxHP;
    }
    private void UpdateMP()
    {
        _playerMPImage.fillAmount = playerData.CurMP / playerData.MaxMP;
    }

    private void UpdateExp()
    {
        _playerExpImage.fillAmount = (float)playerData.CurExp / playerData.MaxExp;
        Debug.Log("EXP" + playerData.CurExp + " " + playerData.MaxExp);
    }
    void UpdateUI()
    {
        UpdateHP();
        UpdateMP();
        UpdateExp();
    }
    private void ToggleUI(InputAction.CallbackContext obj)
    {
        bool toggle = _gamePlayUI.activeInHierarchy;
        _gamePlayUI.SetActive(!toggle);
        _gameEndUI.SetActive(toggle);

        if (!toggle)
        {
            GameManager.Instance.inputManager.EnablePlayerAction();
        }
        else
        {
            GameManager.Instance.inputManager.DisablePlayerAction();
        }
    }
    #region BuffSystem
    [Header("Buff System")]
    public RectTransform prefab;
    public RectTransform parent;

    private List<RectTransform> _slots = new List<RectTransform>();

    private const int slotSize = 70;
    private const int slotSpacing = 10;
    private const int slotPerRow = 9;
    private const int rowSpacing = 10;

    public void ShowBuff()
    {
        RectTransform newSlot = Instantiate(prefab, parent);
        _slots.Add(newSlot);
        UpdateLayout();
    }
    private void UpdateLayout()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i] == null)
            {
                _slots.RemoveAt(i);
            }
        }
        for (int i = 0; i < _slots.Count; i++)
        {
            int row = i / slotPerRow;
            int column = i % slotPerRow;
            float x = column * (slotSize + slotSpacing);
            float y = row * (slotSize + rowSpacing);
            _slots[i].anchoredPosition = new Vector2(x, y);
            _slots[i].sizeDelta = new Vector2(slotSize, slotSize);
        }
    }
    #endregion

    private void OnDestroy()
    {
        playerData.OnHPChange -= UpdateHP;
        playerData.OnMPChange -= UpdateMP;
        playerData.OnExpChange -= UpdateExp;
        GameManager.Instance.inputManager.UIActions.Option.started -= ToggleUI;
        BossScene.OnBossSpawned.RemoveListener(SetBossUI);
    }
}
