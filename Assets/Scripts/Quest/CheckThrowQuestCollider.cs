using System.Collections.Generic;
using UnityEngine;

public class CheckThrowQuestCollider : MonoBehaviour
{
    [SerializeField] private List<int> questID; // 퀘스트 ID

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            foreach (int id in questID)
            {
                QuestManager.I.CheckCurrentQuest(id); // 현재 퀘스트 확인
            }
        }
    }
}
