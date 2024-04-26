using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class SpecialQuest : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public float blackoutDuration = 3.0f; // 어두워지는 시간
    public float delayBeforeFade = 2.0f; // 대화가 끝난 후 페이드 시작 전 딜레이 시간
    public GameObject player;

    public GameObject blackScreen; // 화면을 가리는 오브젝트
    public GameObject timeLineObject;
    private int playerstopcount = 1;

    private void Start()
    {
        blackScreen.SetActive(false); // 시작할 때는 화면 가리기 비활성화
    }

    private void Update()
    {
        if (QuestManager.I.currentQuest.QuestID == 1038)
        {
            
            for (int i = 0; i < playerstopcount; i++)
            {
                timeLineObject.GetComponent<PlayableDirector>().enabled = true;
                GameManager.Instance.inputManager.DisablePlayerAction();
                playerstopcount--;
            }          
        }
        //if (QuestManager.I.currentQuest.QuestID == 1039)
        //{
        //    timeLineObject.SetActive(false);
        //}
    }

    // 퀘스트 완료 후 연출을 시작하기 위한 메서드
    public void StartQuestCompleteEffect()
    {
        StartCoroutine(PerformFade());
    }

    // 어두워지고 다시 밝아지는 효과를 처리하는 코루틴
    private IEnumerator PerformFade()
    {
        yield return new WaitForSeconds(delayBeforeFade); // 페이드 시작 전 딜레이

        // 화면 어두워지는 효과
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(blackoutDuration);

        // 화면 다시 밝아지는 효과
        blackScreen.SetActive(false);
        dayNightCycle.OnQuest03Complete();

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForEndOfFrame();
            player.transform.position = new Vector3(-2.3763936f, 0.8f, 126.779488f);
        }
    }
}
