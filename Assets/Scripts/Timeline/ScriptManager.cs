using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    // 다양한 자막 키와 그에 해당하는 자막 텍스트를 저장하는 딕셔너리
    private Dictionary<string, string> subtitleDictionary = new Dictionary<string, string>();

    // 자막 키와 자막 텍스트를 추가하는 함수
    public void AddSubtitle(string key, string text)
    {
        if (!subtitleDictionary.ContainsKey(key))
        {
            subtitleDictionary.Add(key, text);
        }
        else
        {
            Debug.LogWarning("Subtitle key already exists: " + key);
        }
    }

    private void Start()
    {
        AddSubtitle("Sub1", "크크큭 고기!! 고기를 가져와!");
        AddSubtitle("Sub2", "나님에게 고기를 바쳐라. 어리석은 것들아! 내 흑염룡을 맛보고 싶은것이냐!");
        AddSubtitle("Sub3", "띠리띠리!? 왜 그러는거야!");
        AddSubtitle("Sub4", "왜라니 나에게 감히 왜라는 단어를 쓴거야?");
        AddSubtitle("Sub5", "크으으으..내안에서 또다른 내가 살아난다!!!!");
        AddSubtitle("Sub6", "비켜.");
        AddSubtitle("Sub7", "스승님?");
        AddSubtitle("Sub8", "크하핫 나에게 도전장을 내미는 놈은 살아돌아가지 못할 ㄱ..");
        AddSubtitle("Sub9", "뚜쉬뚜쉬의 한방에 띠리띠리는 기절했다.");
        AddSubtitle("Sub10", "오랜만에 용의 힘을 개방했더니 좀 피곤하군. ");
        AddSubtitle("Sub11", "어떻게 된 일이죠?");
        AddSubtitle("Sub12", "녀석을 잠재웠다. 내 할일은 끝났으니 뒷일을 책임지도록.");
        AddSubtitle("Sub13", "(스승님이 뭔가를 알고 계신걸까?)");

        AddSubtitle("Sub14", "이번에도 사라진 우리 주민을 찾아주어 고맙네!");
        AddSubtitle("Sub15", "아닙니다! 촌장님. 연달아 이런 일이 일어나서...");
        AddSubtitle("Sub16", "아니! 하이하이가 왜 이러고 있어?");
        AddSubtitle("Sub17", "...?");

        AddSubtitle("Sub18", "(거친 숨을 쉬며)하아하아 서리서리...");
        AddSubtitle("Sub19", "응? 이게 뭐지?");
    }

    // 자막 키에 해당하는 자막 텍스트를 반환하는 함수
    public string GetSubtitleText(string key)
    {
        if (subtitleDictionary.ContainsKey(key))
        {
            return subtitleDictionary[key];
        }
        else
        {
            Debug.LogWarning("Subtitle key not found: " + key);
            return string.Empty;
        }
    }
}