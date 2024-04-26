using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup dimedCanvasGroup; // 대화창 Dimed CanvasGroup
    [SerializeField] private TextMeshProUGUI nameText; // 대화창 이름 Text
    [SerializeField] private TextMeshProUGUI dialogueText; // 대화창 대사 Text
    public delegate void HideDialogueUIEventHandler();
    public static event HideDialogueUIEventHandler OnDialogeExited;

    public List<Dialogue> dialogues = new List<Dialogue>(); // 대화 리스트
    public int dialogueIndex = 0; // 대화 인덱스

    public static DialogueHandler I;
    private void Awake()
    {
        I = this;
    }
    private void Update()
    {
        // F 누르면 CheckDialogue 호출
        if (IsDialogueActive() && Input.GetKeyDown(KeyCode.F))
        {
            CheckDialogue();
        }
    }

    // 대화 확인
    public void CheckDialogue()
    {
        dialogueIndex++; // 대화 인덱스 증가

        // dialogues가 남아있다면
        if (dialogueIndex < dialogues.Count)
        {
            ShowDialogue(); // 대화창 표시
        }
        else
        {
            HideDialogueUI(); // 대화창 UI 숨기기
            QuestManager.I.currentQuest.Queststatus = QuestStatus.Progress;

            Debug.Log("대화 끝, 퀘스트 목표 표시, 바로 퀘스트 완료인지 확인");

            // 퀘스트 목표 표시
            QuestManager.I.ShowQuestDescription();

            // 바로 퀘스트 완료인지 확인
            QuestManager.I.CheckDirectQuestClear();
        }
    }

    // 대화 목록 초기화
    public void InitDialogues(List<Dialogue> dialogues)
    {
        this.dialogues = dialogues; // 대화 리스트 할당
        dialogueIndex = 0; // 대화 인덱스 초기화
        QuestManager.I.currentQuest.Queststatus = QuestStatus.TalkActive;

        if (!QuestManager.I.currentQuest.isNoScript)
        {
            ShowDialogue(); // 대화창 표시
            GameManager.Instance.inputManager.DisablePlayerAction();
        }
    }
    public void InitDialogue(List<Dialogue> dialogues)
    {
        this.dialogues = dialogues; // 대화 리스트 할당
        dialogueIndex = 0; // 대화 인덱스 초기화

        if (!QuestManager.I.currentQuest.isNoScript)
        {
            ShowDialogue(); // 대화창 표시
            GameManager.Instance.inputManager.DisablePlayerAction();
        }
    }

    // 대화창 표시
    private void ShowDialogue()
    {
        var dialogue = dialogues[dialogueIndex]; // 현재 대화

        ShowDialogueUI(dialogue.TalkerKo, dialogue.Scripts); // 대화창 UI 표시
    }

    // 대화창 UI 표시
    private void ShowDialogueUI(string name, string dialogue)
    {
        nameText.text = name; // 이름 표시
        dialogueText.text = dialogue; // 대사 표시

        ActiveDialogue(); // 대화창 활성화
    }

    // 대화창 활성화
    private void ActiveDialogue()
    {
        if (!dimedCanvasGroup.blocksRaycasts)
        {
            dimedCanvasGroup.alpha = 1; // Dimed 활성화
            dimedCanvasGroup.blocksRaycasts = true; // Raycast 활성화
            dimedCanvasGroup.interactable = true; // 상호작용 활성화
        }
    }

    // 대화창 UI 숨기기
    public void HideDialogueUI()
    {
        DeactiveDialogue(); // 대화창 비활성화
        OnDialogeExited?.Invoke();
        nameText.text = ""; // 이름 초기화
        dialogueText.text = ""; // 대사 초기화
        GameManager.Instance.inputManager.EnablePlayerAction();
    }

    // 대화창 비활성화
    private void DeactiveDialogue()
    {
        if (dimedCanvasGroup.blocksRaycasts)
        {
            dimedCanvasGroup.alpha = 0; // Dimed 비활성화
            dimedCanvasGroup.blocksRaycasts = false; // Raycast 비활성화
            dimedCanvasGroup.interactable = false; // 상호작용 비활성화
        }
    }

    // 대화창 UI가 활성화 상태인지 확인
    private bool IsDialogueActive()
    {
        return dimedCanvasGroup.blocksRaycasts;
    }
}

[Serializable]
public class Dialogue
{
    public int DialogueID; // 대화 번호
    public string Talker;
    public string TalkerKo; // 이름
    public string Scripts; // 대사
    public Dialogue(DialogueData dialogue)
    {
        DialogueID = dialogue.DialogueID;
        Talker = dialogue.Talker;
        TalkerKo = dialogue.TalkerKo;
        Scripts = dialogue.Scripts;
    }
    public Dialogue() { }
}