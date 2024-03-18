using System.IO;
using UnityEngine;

[System.Serializable]
public struct PlayerStat
{
    public int Level;
    public int MaxExp;
    public int CurExp;
    public float MaxHP;
    public float CurHP;
    public float MaxMP;
    public float CurMP;
    public float MaxSP;
    public float CurSP;
    public float Damage;
    public float DamageSpeed;
    public float CriDamage;
    public float Def;
    public float MoveSpeed;
}
/// <summary>
/// PlayerGameData�� ���� �� ���� Initialize�� ���� �����͸� �ҷ����ų� 
/// �⺻ �����ͷ� �ʱ�ȭ �ص� �� �ִ�.
/// </summary>
[System.Serializable]
public class PlayerGameData
{
    private string _jsonDataPath;
    private DataManager dataManager;

    [Header("PlayerInfo")]
    public string CharacterType;
    public int ID; // �α��� �Ҷ� �ʿ��ϸ� ����� ID -> ���� �̻��
    public int CharacterID; // ĳ���� ID -> � ������ ĳ�������� ����

    [Header("PlayerStat")]
    public PlayerStat stat = new PlayerStat();
    public int Level;
    public int MaxExp;
    public int CurExp;
    public float MaxHP;
    public float CurHP;
    public float MaxMP;
    public float CurMP;
    public float MaxSP;
    public float CurSP;
    public float Damage;
    public float DamageSpeed;
    public float CriDamage;
    public float Def;
    public float MoveSpeed;

    public PlayerGameData()
    {
        _jsonDataPath = $"{Application.dataPath}/Datas/PlayerData";
    }
    /// <summary>
    /// ����� � ������ ĳ���������� ���� �����͸� �ʱ�ȭ �����ش�
    /// </summary>
    /// <param name="CharacterID">ĳ������ ����</param>
    public void Initialize(int CharacterID)
    {
        dataManager = GameManager.Instance.dataManager;
        this.CharacterID = CharacterID;
        // ������ ������ ����Json������ �ҷ�����
        if (File.Exists(_jsonDataPath))
        {
            string json = File.ReadAllText(_jsonDataPath);
            JsonUtility.FromJsonOverwrite(json, this);
            return;
        }
        // ������ ������ �⺻ ������ �ҷ�����
        LoadDefaultData();
    }
    public void LoadDefaultData()
    {
        //defualtDataSetting
        if (dataManager != null)
        {
            PlayerStatData statData = dataManager.playerStatDataBase.GetData(CharacterID);
            Level = 1;
            CurExp = 0;
            if (statData != null)
            {
                CharacterType = statData.CharacterType;
                CharacterID = statData.CharacterID;
                MaxHP = statData.HP;
                CurHP = MaxHP;
                MaxMP = statData.MP;
                CurMP = MaxMP;
                MaxSP = statData.SP;
                CurSP = MaxSP;
                Damage = statData.Damage;
                DamageSpeed = statData.DamageSpeed;
                CriDamage = statData.CriDamage;
                Def = statData.Def;
                MoveSpeed = statData.MoveSpeed;
            }
        }
    }
    public void HPChange(int change)
    {
        CurHP += change;
        if (CurHP > MaxHP)
        {
            CurHP = MaxHP;
        }
        else if (CurHP < 0)
        {
            //�÷��̾� ���
            CurHP = 0;
        }
    }
    public void LevelUp()
    {
        Level++;
        CurExp = 0;

        PlayerLevelData playerLevelData = dataManager.playerLevelDataBase.GetData(Level);

        if (playerLevelData != null)
        {
            MaxExp = playerLevelData.Exp;
            MaxHP += playerLevelData.HP;
            CurHP = MaxHP;
            CurMP = MaxMP;
            Damage += playerLevelData.Damage;
            Def += playerLevelData.Def;
        }
        SaveData();
    }

    public void SaveData()
    {
        Utility.SaveToJson(this, _jsonDataPath);
    }

    public void DeleteData()
    {
        Utility.DeleteJson(_jsonDataPath);
    }
}

