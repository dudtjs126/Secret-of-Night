using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : UIBase
{
    // 나중에 통합해야됨
    private string _jsonDataPath;

    private bool[] savefile;


    [Header("GameButton")]
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _makePeopleButton;
    [SerializeField] private Button _gameEndButton;

    [Header("Option")]
    [SerializeField] private Button _optionButton;
    [SerializeField] private GameObject _optionPopup;
    public void Start()
    {
        _jsonDataPath = $"{Application.persistentDataPath}/Datas/PlayerData_";
        Initialize();
    }
    public override void Initialize()
    {
        base.Initialize();

        // 초기 화면에서 하는것이 좋을거 같다
        for (int slotNumber = 0; slotNumber < 5; slotNumber++)
        {
            if (File.Exists(_jsonDataPath + $"{slotNumber}"))
            {

                GameManager.Instance.playerManager.playerDatas.Add(slotNumber, new PlayerGameData());
                GameManager.Instance.playerManager.playerDatas[slotNumber].SlotNumber = slotNumber;
                GameManager.Instance.playerManager.playerDatas[slotNumber].Initialize();
                Debug.Log($"데이터 존재 + {slotNumber}");
            }
        }
        // 데이터가 있는경우 이어하기 및 불러오기
        if (GameManager.Instance.playerManager.playerDatas.Count != 0)
        {
            _loadButton.gameObject.SetActive(true);
            _loadButton.onClick.AddListener(OnLoadButtonClick);
            _continueButton.gameObject.SetActive(true);
            _continueButton.onClick.AddListener(OnContinueButtonClick);
        }
        else
        {
            // 데이터가 없는경우 게임 시작 Button
            _gameStartButton.gameObject.SetActive(true);
            _gameStartButton.onClick.AddListener(OnGameStartButtonClick);
        }

        _makePeopleButton.onClick.AddListener(OnMakePeopleButtonClick);
        _gameEndButton.onClick.AddListener(OnGameEndButtonClick);
        _optionButton.onClick.AddListener(OnOptionButtonClick);
    }

    private void OnMakePeopleButtonClick()
    {
        GameManager.Instance.uiManager.ShowPopupUI<MakePeoplePopup>();
    }

    private void OnOptionButtonClick()
    {
        _optionPopup.SetActive(true);
    }

    void OnGameStartButtonClick()
    {
        GameManager.Instance.playerManager.Initialize(1);
        GameManager.Instance.sceneManager.LoadSceneAsync(Scene.Main);
    }
    void OnLoadButtonClick()
    {
        GameManager.Instance.uiManager.ShowPopupUI<LoadPopup>("LoadPopup");
    }
    void OnContinueButtonClick()
    {
        int lastestData = FindLastestData();
        if (lastestData == -1)
        {
            Debug.Log("There is no Data");
            return;
        }

        GameManager.Instance.playerManager.Init(lastestData);
        GameManager.Instance.sceneManager.LoadSceneAsync(Scene.Main);
    }
    int FindLastestData()
    {
        int lastestData = -1;
        DateTime data = DateTime.MinValue;

        for (int i = 0; i < GameManager.Instance.playerManager.maxSlotDataNumber; i++)
        {
            if (GameManager.Instance.playerManager.playerDatas.ContainsKey(i))
            {
                if (DateTime.Parse(GameManager.Instance.playerManager.playerDatas[i].SaveTime) > data)
                {
                    data = DateTime.Parse(GameManager.Instance.playerManager.playerDatas[i].SaveTime);
                    lastestData = i;
                }
            }
        }

        return lastestData;
    }
    void OnGameEndButtonClick()
    {
        Application.Quit();
    }
}
