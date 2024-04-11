using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public MonsterData dataManager;

    public void Initialize()
    {
        dataManager = new MonsterData();
        dataManager.Initialize();
    }

    public MonsterInfo GetMonsterInfoByKey(int MonsterID)
    {
        return dataManager.monsterDatabase.GetMonsterInfoByKey(MonsterID);
    }
}
