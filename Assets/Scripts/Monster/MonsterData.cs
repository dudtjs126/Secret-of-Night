using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterInfo
{
    public int MonsterID;
    public string Name;
    public int Level;
    public int Exp;
    public bool ShortDistance;
    public bool AtkStance;
    public float HP;
    public float Damage;
    public float Daf;
    public float DmgSpeed;
    public float CriDamage;
    public float MoveSpeed;
    public float RunSpeed;
    public float AtkRange;
    public float TargetRange;
    public float RotationDamping;
    public int[] DropItem;
    public int[] DropItemCount;
    public float[] Weigth;

    public string PrefabPath;

    public GameObject prefab;
}
//건원님 DataManager에 합쳐보자.

[Serializable]
public class MonsterDataBase //->실제데이터
{
    public List<MonsterInfo> FieldMonster;
    public Dictionary<int, MonsterInfo> fieldMonDic = new();

    //몬스터리스트를 딕셔너리에 추가
    public void Initialize()
    {
        foreach (MonsterInfo fieldMonster in FieldMonster)
        {
            //fieldMonster.Init();
            fieldMonDic.Add(fieldMonster.MonsterID, fieldMonster);
            fieldMonster.prefab = Resources.Load<GameObject>(fieldMonster.PrefabPath);
        }
    }

    //MonsterID로 몬스터정보 반환
    public MonsterInfo GetMonsterInfoByKey(int MonsterID)
    {
        if (fieldMonDic.ContainsKey(MonsterID))
        {
            return fieldMonDic[MonsterID];
        }
        return null;
    }
}

public class MonsterData
{
    public MonsterDataBase monsterDatabase;

    public void Initialize()
    {
        TextAsset fieldMonData = Resources.Load<TextAsset>("Json/FieldMonster_Data");
        monsterDatabase = JsonUtility.FromJson<MonsterDataBase>(fieldMonData.text);
        monsterDatabase.Initialize();
    }
}
