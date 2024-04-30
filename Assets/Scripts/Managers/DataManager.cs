using UnityEngine;


public class DataManager : MonoBehaviour
{
    #region DataBases
    public PlayerLevelDataBase playerLevelDataBase;
    public PlayerSkillDataBase playerSkillDataBase;
    public PlayerStatDataBase playerStatDataBase;
    public BossMonsterStatDatabase bossMonsterStatDatabase;
    public ItemDataBase itemDataBase;
    public QuestDataBase questDataBase;
    public DialogueDataBase dialogueDataBase;
    public MonsterSpawnDatabase monsterSpawnDatabase;
    public SoundData soundData = null;
    #endregion

    public void Initialize()
    {
        playerLevelDataBase = Utility.LoadJson<PlayerLevelDataBase>("PlayerLevel_Data");
        playerLevelDataBase.Initalize();
        playerSkillDataBase = Utility.LoadJson<PlayerSkillDataBase>("PlayerSkill_Data");
        playerSkillDataBase.Initalize();
        playerStatDataBase = Utility.LoadJson<PlayerStatDataBase>("PlayerStat_Data");
        playerStatDataBase.Initalize();

        bossMonsterStatDatabase = Utility.LoadJson<BossMonsterStatDatabase>("BossMonsterStat_Data");
        bossMonsterStatDatabase.Initalize();

        itemDataBase = Utility.LoadJson<ItemDataBase>("Items_Data");
        itemDataBase.Initalize();

        questDataBase = Utility.LoadJson<QuestDataBase>("Quest_Data");
        questDataBase.Initalize();

        dialogueDataBase = Utility.LoadJson<DialogueDataBase>("QuestScript_Data");
        dialogueDataBase.Initalize();

        monsterSpawnDatabase = Utility.LoadJson<MonsterSpawnDatabase>("FieldMonsterSpwan_Data");
        monsterSpawnDatabase.Initalize();

        soundData = ScriptableObject.CreateInstance<SoundData>();
        soundData.LoadData();
    }
}

