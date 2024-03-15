using UnityEngine;

[System.Serializable]
public class BossMonsterGameData
{
    public int BossID;
    public string Name;
    public float HP;
    public float Damage;
    public float Def;
    public float MoveSpeed;
    public float Range;
    
    public BossMonsterGameData(int monsterID)
    {
        Initialize(monsterID);
    }

    private void Initialize(int monsterID)
    {
        var dataManager = GameManager.Instance.dataManager;

        BossMonsterStatData monsterData = dataManager.bossMonsterStatDatabase.GetData(monsterID);
        if (monsterData != null)
        {
            BossID = monsterData.BossID;
            Name = monsterData.Name;
            HP = monsterData.HP;
            Damage = monsterData.Damage;
            Def = monsterData.Def;
            MoveSpeed = monsterData.MoveSpeed;
            Range = monsterData.Range;
        }
        else
        {
            // ���� �����͸� ã�� �� ���� ���, �α� �޽��� ���
            Debug.LogError("MonsterGameData: Initialize failed. No data for monsterID: " + monsterID);
        }
    }
}
