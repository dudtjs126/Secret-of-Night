using UnityEngine;

public class PlayerSkill1State : PlayerBaseState
{
    public PlayerSkill1State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = 0f;
        stateMachine.IsAttacking = true;
        int SkillID = GameManager.Instance.dataManager.playerStatDataBase.GetData(stateMachine.Player.PlayerData.CharacterID).Skills[1];
        string path = GameManager.Instance.dataManager.playerSkillDataBase.GetData(SkillID).PrefabPath;

        GameObject go = Resources.Load<GameObject>(path);
        Object.Instantiate(go, stateMachine.Player.transform.position + Vector3.up, Quaternion.identity);

        StartAnimation(stateMachine.Player.AnimationData.Skill1);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.Skill1);
    }
    public override void Update()
    {
        base.Update();
        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Skill");

        if (normalizedTime > 1f)
        {
            stateMachine.IsAttacking = false;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}