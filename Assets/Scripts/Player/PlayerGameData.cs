using System;
using System.IO;
using UnityEngine;

/// <summary>
/// PlayerGameData를 생성 한 다음 Initialize를 통해 데이터를 불러오거나 
/// 기본 데이터로 초기화 해둘 수 있다.
/// </summary>


[System.Serializable]
public class PlayerGameData
{

    private DataManager dataManager;
    public string JsonDataPath => $"{Application.persistentDataPath}/Datas/PlayerData_{SlotNumber}";

    [Header("PlayInfo")]
    public int SlotNumber;
    public string SaveTime;
    public string ChapterInfo;
    public Quest quest;
    public int questIndex;

    [field: Header("PlayerInfo")]
    public string CharacterType;
    public int CharacterID; // 캐릭터 ID -> 어떤 종류의 캐릭터인지 결정
    public string CharacterName; // 플레이어 이름    

    [Header("PlayerStat")]
    public int Level;
    public float MaxExp;
    public float CurExp;
    public float MaxHP;
    public float CurHP;
    public float MaxMP;
    public float CurMP;
    public float MaxSP;
    public float CurSP;
    public float DefaultDamage;
    public float DamageSpeed;
    public float CriDamage;
    public float Def;
    public float MoveSpeed;
    public int Gold;

    [Header("무기")]
    public float WeaponDamage;
    public float Damage => WeaponDamage + DefaultDamage;



    // 여기 없어야 되는데 일단 넣어 둡니다
    public event Action OnDie;
    public event Action OnHPChange;
    public event Action OnMPChange;
    public event Action OnSPChange;
    public event Action OnExpChange;
    public event Action OnLevelUp;
    public PlayerGameData()
    {
    }
    /// <summary>
    /// 현재는 어떤 종류의 캐릭터인지에 따라 데이터를 초기화 시켜준다
    /// </summary>
    /// <param name="CharacterID">캐릭터의 종류</param>
    public void Initialize()
    {
        Debug.Log(JsonDataPath);
        dataManager = GameManager.Instance.dataManager;
        // 파일이 있으면 파일Json데이터 불러오기
        if (File.Exists(JsonDataPath))
        {
            string json = File.ReadAllText(JsonDataPath);
            JsonUtility.FromJsonOverwrite(json, this);
            return;
        }
        // 파일이 없으면 기본 데이터 불러오기
        LoadDefaultData();
    }
    public void LoadDefaultData()
    {
        //defualtDataSetting
        if (dataManager != null)
        {
            //게임 정보 초기화
            CharacterID = 1; // 캐릭터 선택 창이 아직 존재 하지 않는다
            ChapterInfo = "튜토리얼";

            //캐릭터 정보 초기화
            CharacterName = "Unknown";

            //stat 초기화
            Level = 1;
            CurExp = 0;

            PlayerStatData statData = dataManager.playerStatDataBase.GetData(CharacterID);
            PlayerLevelData playerLevelData = dataManager.playerLevelDataBase.GetData(Level);

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
                MaxExp = playerLevelData.Exp;
                DefaultDamage = statData.Damage;
                DamageSpeed = statData.DamageSpeed;
                CriDamage = statData.CriDamage;
                Def = statData.Def;
                MoveSpeed = statData.MoveSpeed;
                Gold = statData.Gold;
                Debug.Log("데이터 최조 정보");
            }
        }
    }
    public void HPChange(float change)
    {
        CurHP += change;
        if (CurHP > MaxHP)
        {
            CurHP = MaxHP;
        }
        else if (CurHP < 0)
        {
            CurHP = 0;
            OnDie?.Invoke();
        }
        OnHPChange?.Invoke();
    }
    public void MPChange(float amount)
    {
        CurMP += amount;
        if (CurMP > MaxMP)
        {
            CurMP = MaxMP;
        }
        else if (CurMP < 0)
        {
            CurMP = 0;
        }
        OnMPChange?.Invoke();
    }
    public void SPChange(float amount)
    {
        CurSP += amount;
        if (CurSP > MaxSP)
        {
            CurSP = MaxSP;
        }
        else if (CurSP < 0)
        {
            CurSP = 0;
        }
        OnSPChange?.Invoke();
    }
    public void LevelUp()
    {
        Level++;

        PlayerLevelData playerLevelData = dataManager.playerLevelDataBase.GetData(Level);

        if (playerLevelData != null)
        {
            MaxExp = playerLevelData.Exp;
            MaxHP += playerLevelData.HP;
            CurHP = MaxHP;
            CurMP = MaxMP;
            DefaultDamage += playerLevelData.Damage;
            Def += playerLevelData.Def;
        }
        ResetStatus();
        OnLevelUp?.Invoke();

        if (CurExp >= MaxExp)
        {
            Debug.Log("over");
            ExpChange(0);
        }
    }

    public void ResetStatus()
    {
        CurHP = MaxHP;
        CurMP = MaxMP;
        CurSP = MaxMP;

        OnHPChange?.Invoke();
        OnMPChange?.Invoke();
        OnSPChange?.Invoke();
    }
    public void SaveData()
    {
        SaveTime = DateTime.Now.ToString();
        quest = QuestManager.I.currentQuest;
        questIndex = QuestManager.I.questIndex;
        Utility.SaveToJson(this, JsonDataPath);
    }
    public void LoadSavedData()
    {
        Utility.LoadJson<PlayerGameData>(JsonDataPath);
    }

    public void DeleteData()
    {
        Utility.DeleteJson(JsonDataPath);
    }

    public void ExpChange(float exp)
    {
        CurExp += exp;
        if (CurExp >= MaxExp)
        {
            CurExp -= MaxExp;
            LevelUp();
        }
        OnExpChange?.Invoke();
    }
}

