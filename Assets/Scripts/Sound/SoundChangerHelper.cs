using UnityEngine;

[RequireComponent(typeof(SoundChanger))]
public class SoundChangerHelper : MonoBehaviour
{
    private SoundChanger _soundChanger;
    void Start()
    {
        _soundChanger = GetComponent<SoundChanger>();
        if (QuestManager.I.currentQuest.QuestID > 1032)
        {
            _soundChanger._enterBgm = SoundList.fieldBGMAfter;
        }
    }
}
