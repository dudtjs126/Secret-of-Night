using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float _timer = 0f;
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.IsJumping = true;
        AddJumpForce();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameter);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.ForceReceiver.Reset();
        _timer = 0f;
        stateMachine.Player.IsJumping = false;
        StopAnimation(stateMachine.Player.AnimationData.JumpParameter);
    }

    public override void Update()
    {
        base.Update();

        AnimatorStateInfo info = stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsTag("Jump"))
        {
            if (info.normalizedTime > 0.5f && stateMachine.Player.IsGrounded)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }

        //if (_timer <= stateMachine.Player.jumpDelayTime)
        //{
        //    _timer += Time.deltaTime;
        //    return;
        //}
        //if (stateMachine.Player.IsGrounded && stateMachine.Player.ForceReceiver.Movement.y < 0f)
        //{
        //    if (stateMachine.MovementInput == Vector2.zero)
        //    {
        //        stateMachine.ChangeState(stateMachine.IdleState);
        //    }
        //    else
        //    {
        //        stateMachine.ChangeState(stateMachine.WalkState);
        //    }

        //}
    }
    private void AddJumpForce()
    {
        stateMachine.Player.ForceReceiver.Jump(stateMachine.Player.jumpForce);
        //stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * GetMovementSpeed());
    }
}