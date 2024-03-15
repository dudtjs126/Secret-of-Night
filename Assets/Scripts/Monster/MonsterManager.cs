using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public DataManager dataManager;

    // Start is called before the first frame update
    void Start()
    {
        dataManager = DataManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public MonsterInfo GetMonsterInfoByKey(string name)
    {
        return dataManager.monsterDatabase.GetMonsterInfoByKey(name);
    }

    //���� ����
    //���� ����
    //���� ����
}
