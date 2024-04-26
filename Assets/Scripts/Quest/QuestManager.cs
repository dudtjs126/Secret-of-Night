using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager I;
    private SpecialQuest quest03Director;
    public GameObject questPopup;
    public delegate void QusestClearedEventHandler();
    public static event QusestClearedEventHandler OnQuestCleared;

    private const int EXP_REWARD_ID = 100;

    [SerializeField] private DialogueHandler dialogueHandler;
    [SerializeField] private QuestGenerator questGenerator;

    [SerializeField] private TextMeshProUGUI questDescriptionText; // 퀘스트 설명 Text
    [SerializeField] private TextMeshProUGUI nextQuestGuideText; // 다음 퀘스트 수락 안내

    public List<Quest> quests; // 퀘스트 리스트
    public int questIndex = 0; // 퀘스트 인덱스
    public Quest currentQuest; // 현재 퀘스트    
    public Dialogue currentDialogue;

    public bool isKillMonsterClear;

    private void Awake()
    {
        I = this;
        quest03Director = GetComponent<SpecialQuest>();
    }

    private void Start()
    {
        quests = questGenerator.tempQuests; // 전체 퀘스트 리스트 설정 (임시)
        quests.Sort((x, y) => x.QuestID.CompareTo(y.QuestID)); // 퀘스트 ID 순으로 정렬

        if (GameManager.Instance.playerManager.playerData.quest == null)
        {
            questIndex = 0; // 퀘스트 인덱스 초기화
            SetCurrentQuest(); // 현재 퀘스트 설정
            InitDialogues(); // 대화 목록 초기화
        }
        else
        {
            questIndex = GameManager.Instance.playerManager.playerData.questIndex;
            currentQuest = GameManager.Instance.playerManager.playerData.quest;
            DialogueHandler.I.HideDialogueUI();
            ShowQuestDescription();
            InitDialogue();
        }




    }

    private void Update()
    {
        // H키 누르면 퀘스트 클리어 (테스트용)
        if (Input.GetKeyDown(KeyCode.H))
        {
            QuestClear(); // 퀘스트 클리어
        }

        //// H 누르면 다음 퀘스트 보이기 (테스트용)
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    InitDialogues(); // 대화 목록 초기화
        //}
    }

    // 현재 퀘스트 설정
    private void SetCurrentQuest()
    {
        if (questIndex >= quests.Count)
        {
            Debug.LogWarning("퀘스트가 더 이상 없습니다.");
            return;
        }

        currentQuest = quests[questIndex]; // 현재 퀘스트 설정
        if (currentQuest.QuestID == 1002)
            ShowQuestDescription();

    }

    // 대화 목록 초기화
    public void InitDialogues()
    {
        dialogueHandler.InitDialogues(currentQuest.dialogues); // 대화 목록 초기화
    }
    public void InitDialogue()
    {
        dialogueHandler.InitDialogue(currentQuest.dialogues); // 대화 목록 초기화
    }

    // 퀘스트 설명 표시
    public void ShowQuestDescription()
    {
        questDescriptionText.text = currentQuest.Description; // 퀘스트 설명 표시
        nextQuestGuideText.text = currentQuest.NextQuestGuide = "";
    }

    // 퀘스트 설명 숨기기
    public void HideQuestDescription()
    {
        questDescriptionText.text = ""; // 퀘스트 설명 숨기기
        nextQuestGuideText.text = currentQuest.NextQuestGuide;
    }

    // 퀘스트 성공
    public void QuestClear()
    {
        // 연출 같은 특수 퀘스트 성공 처리
        if (currentQuest.QuestID == 1005)
        {
            SpecialQuestClear(); // 특수 퀘스트 성공
        }
        else
        {
            NextQuest(); // 다음 퀘스트로            
            Debug.Log("퀘스트 클리어");
        }
        if (currentQuest.isNoScript)
        {
            ShowQuestDescription();
        }

        // 퀘스트 보상이 있으면 보상 처리
        ItemReward(
            currentQuest.RewardID2, currentQuest.RewardCount2,
            currentQuest.RewardID3, currentQuest.RewardCount3,
            currentQuest.RewardID4, currentQuest.RewardCount4,
            currentQuest.RewardID5, currentQuest.RewardCount5); // 보상 처리

        ExpReward(currentQuest.RewardID, currentQuest.RewardCount);


        OnQuestCleared?.Invoke();
        isKillMonsterClear = false;

    }

    private void ItemReward(
                           int rewardID2, int rewardCount2,
                           int rewardID3, int rewardCount3,
                           int rewardID4, int rewardCount4,
                           int rewardID5, int rewardCount5)
    {

        HandleReward(rewardID2, rewardCount2);
        HandleReward(rewardID3, rewardCount3);
        HandleReward(rewardID4, rewardCount4);
        HandleReward(rewardID5, rewardCount5);
    }

    private void ExpReward(int rewardID, float rewardCount)
    {
        if (rewardID == EXP_REWARD_ID)
        {
            // 경험치 보상 처리
            GameManager.Instance.playerManager.playerData.ExpChange(rewardCount);
            Debug.Log($"경험치 {rewardCount} 획득!");
        }
    }
    private void HandleReward(int rewardID, int rewardCount)
    {

        if (rewardID >= 1 && rewardID <= 29)
        {
            // 아이템 보상 처리
            Item item = GameManager.Instance.dataManager.itemDataBase.GetData(rewardID);
            if (item != null)
            {
                Inventory.instance.AddItem(item, rewardCount);
                Debug.Log($"{item.Name} {rewardCount}개 획득!");
            }
        }

    }
    // 특수 퀘스트 성공
    private void SpecialQuestClear()
    {
        quest03Director.StartQuestCompleteEffect();

        NextQuest(); // 다음 퀘스트로
    }

    // 다음 퀘스트로
    private void NextQuest()
    {
        questIndex++; // 퀘스트 인덱스 증가

        // 현재 퀘스트의 isContinue가 true라면
        if (currentQuest.isContinue)
        {
            SetCurrentQuest(); // 다음 퀘스트로 변경            
            InitDialogues(); // 대화 목록 초기화
            currentQuest.Queststatus = QuestStatus.Complete;
            Debug.Log("NextQuset");
        }
        else
        {
            SetCurrentQuest(); // 다음 퀘스트로 변경
            HideQuestDescription(); // 퀘스트 설명 숨기기
            currentQuest.Queststatus = QuestStatus.Wait;
            Debug.Log("NextQuset_s");
        }

    }

    // 특정 몬스터를 죽였을 때 or 특정 아이템을 획득했을 때
    public void CheckCount(int id)
    {
        // 첫 번째 몬스터의 ID와 개수를 확인
        if (currentQuest.QuestItemID == id)
        {
            currentQuest.GoalCount--; // 필요한 개수 감소
        }
        // 두 번째 몬스터의 ID와 개수를 확인
        else if (currentQuest.QuestItemID2 == id)
        {
            currentQuest.GoalCount2--; // 필요한 개수 감소
        }

        // 첫 번째 몬스터와 두 번째 몬스터 모두가 필요한 개수를 다 채웠는지 확인
        if (currentQuest.GoalCount <= 0 && currentQuest.GoalCount2 <= 0)
        {
            if (currentQuest.QuestType == 1)
            {
                QuestClear(); // 퀘스트 클리어
            }
            else if (currentQuest.QuestType == 4)
            {
                isKillMonsterClear = true;
                currentQuest.Queststatus = QuestStatus.Progress;
            }
        }
    }


    // NPC 상호작용, 아이템 사용, TriggerEnter 등에서 호출
    public void CheckCurrentQuest(int id)
    {
        // 현재 퀘스트의 questID와 id가 같다면
        if (currentQuest.QuestID == id)
            QuestClear(); // 퀘스트 클리어
        else if (currentQuest.QuestItemID == id)
            QuestClear();
    }

    public void AcceptQuest(int id)
    {
        if (currentQuest.QuestID == id)
        {
            if (currentQuest.Queststatus != QuestStatus.TalkActive)
            {
                InitDialogues();
            }
        }

    }

    // 바로 퀘스트 완료인지 확인
    public void CheckDirectQuestClear()
    {
        // 바로 퀘스트 클리어인 경우
        if (currentQuest.isDirectClear)
            QuestClear(); // 퀘스트 클리어
    }

    public void OnClickQuestPopup()
    {
        questPopup.SetActive(true);
    }

    public void OnExitQuestPopup()
    {
        questPopup.SetActive(false);
    }
}

// Quest
[Serializable]
public class Quest
{
    public int QuestID; // 퀘스트 ID (1ABB - A: 챕터 번호, BB: 퀘스트 번호)
    public int QuestSubID;
    public int QuestType;
    public string QuestTitle;
    public string Description; // 퀘스트 설명
    public string NextQuestGuide;
    public string QuestGoal;
    public int GoalCount; // GoalCount 필요한 개수
    public int GoalCount2;
    public int GoalCount3;
    public int RewardID; // 보상 ID
    public int RewardID2;
    public int RewardID3;
    public int RewardID4;
    public int RewardID5;
    public string RewardItem;
    public string RewardItem2;
    public string RewardItem3;
    public string RewardItem4;
    public string RewardItem5;
    public float RewardCount; // 보상 개수
    public int RewardCount2;
    public int RewardCount3;
    public int RewardCount4;
    public int RewardCount5;
    public int QuestItemID; // QuestType 필요한 ID (고기 10개 가져오기, 스컹크 5마리 잡기 등)
    public int QuestItemID2;
    public int QuestItemID3;

    public QuestStatus Queststatus; // 퀘스트 진행상태
    public bool isContinue; // 이어서 퀘스트 진행 여부
    public bool isDirectClear; // 바로 퀘스트 클리어 여부 (대화가 끝나는 시점에 해당)
    public bool isNoScript;
    public List<Dialogue> dialogues; // 대화 리스트

    public Quest(QuestData quest)
    {
        QuestID = quest.QuestID;
        QuestSubID = quest.QuestSubID;
        QuestType = quest.QuestType;
        QuestTitle = quest.QuestTitle;
        Description = quest.Description;
        QuestGoal = quest.QuestGoal;
        QuestItemID = quest.QuestItemID;
        QuestItemID2 = quest.QuestItemID2;
        QuestItemID3 = quest.QuestItemID3;
        GoalCount = quest.GoalCount;
        GoalCount2 = quest.GoalCount2;
        GoalCount3 = quest.GoalCount3;
        RewardID = quest.RewardID;
        RewardID2 = quest.RewardID2;
        RewardID3 = quest.RewardID3;
        RewardID4 = quest.RewardID4;
        RewardID5 = quest.RewardID5;
        RewardItem = quest.RewardItem;
        RewardItem2 = quest.RewardItem2;
        RewardItem3 = quest.RewardItem3;
        RewardItem4 = quest.RewardItem4;
        RewardItem5 = quest.RewardItem5;
        RewardCount = quest.RewardCount;
        RewardCount2 = quest.RewardCount2;
        RewardCount3 = quest.RewardCount3;
        RewardCount4 = quest.RewardCount4;
        RewardCount5 = quest.RewardCount5;
    }

    public Quest() { }
}
