using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterInfo
{
    public int MonsterID;
    public string Name;
    public int Level;
    public float Exp;
    public bool AtkStance;
    public float HP;
    public float Damage;
    public float Daf;
    public float DmgSpeed;
    public float CriDamage;
    public float MoveSpeed;
    public float RunSpeed;
    public float Range;
}
//���ϴ� ������ -> HP
//����cs ������ HP�� ���Ҽ� �ֵ��� ���� ����־���
//�ǿ��� DataManager�� ���ĺ���.

[Serializable]
public class MonsterDataBase //->����������
{
    public List<MonsterInfo> FieldMonster;
    public Dictionary<string, MonsterInfo> fieldMonDic = new();

    //���͸���Ʈ�� ��ųʸ��� �߰�
    public void Initialize()
    {
        foreach (MonsterInfo fieldMonster in FieldMonster)
        {
            fieldMonDic.Add(fieldMonster.Name, fieldMonster);
        }
    }

    //�̸����� �������� ��ȯ
    public MonsterInfo GetMonsterInfoByKey(string name)
    {
        if (fieldMonDic.ContainsKey(name))
        {
            return fieldMonDic[name];
        }
        return null;
    }
}

public class MonsterData : MonoBehaviour
{
    public static MonsterData Instance;

    public MonsterDataBase monsterDatabase;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        TextAsset fieldMonData = Resources.Load<TextAsset>("Json/FieldMonster_Data");
        monsterDatabase = JsonUtility.FromJson<MonsterDataBase>(fieldMonData.text);
        monsterDatabase.Initialize();
    }

    private void Start()
    {


    }
}
