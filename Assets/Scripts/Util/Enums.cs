public enum SkillType
{
    Active,
    Buff
}

public enum BossState
{
    Idle,
    Moving,
    Attacking,
    Dashing,
    Dying
}

public enum UIEvent
{
    Click,
}

public enum QuestStatus
{
    Wait,
    TalkActive,
    Progress,
    Complete,
}

public enum Scene
{
    GameStart,
    Main,
    Boss,
    Loading,
    None,

}
public enum SoundPlayType
{
    None = -1,
    BGM,
    EFFECT,
    UI,
}
