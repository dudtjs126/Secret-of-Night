using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    public PlayerSkillState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.MovementSpeedModifier = 0f;
        StartAnimation(stateMachine.Player.AnimationData.SkillParameter);
        lockRotation = true;
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.DoSkill = false;
        StopAnimation(stateMachine.Player.AnimationData.SkillParameter);
        lockRotation = false;
    }
    public override void Update()
    {
        base.Update();
        AnimatorStateInfo stateInfo = stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("Skill"))
        {
            if (stateInfo.normalizedTime > 0.8f)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }
}
