using UnityEngine;

/// <summary>
/// data의 기본 클래스 입니다
/// 공통적인 데이터 보유, 이름을 보유중
/// 데이터의 갯수와 이름의 목록 리시=ㅡ트를 얻을 수 있다
/// </summary>
/// 
[CreateAssetMenu(fileName = "BaseData", menuName = "BaseData/BaseData", order = 0)]
public class BaseData : ScriptableObject
{
    public const string dataDirectory = "/Resources/Json/";
    public string[] names = null;

    public BaseData() { }

    public int GetDataCount()
    {
        if (this.names != null)
        {
            return this.names.Length;
        }
        return 0;
    }
    public string[] GetNameList(bool showID, string filterWord = "")
    {
        string[] retList = new string[0];
        if (this.names == null)
        {
            Debug.Log("Error");
            return retList;
        }
        retList = new string[this.names.Length];

        for (int i = 0; i < GetDataCount(); i++)
        {
            if (filterWord != "")
            {
                if (names[i].ToLower().Contains(filterWord.ToLower()) == false)
                {
                    continue;
                }
            }
            if (showID)
            {
                retList[i] = i.ToString() + " : " + names[i];
            }
            else
            {
                retList[i] = names[i];
            }
        }

        return retList;
    }

    public virtual int AddData(string newName)
    {
        return GetDataCount();
    }
    public virtual void RemoveData(int index)
    {
    }
    public virtual void CopyData(int index)
    {
    }
}
