using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float _jumpTimeout = 0.5f;
    private float _jumpTimeoutDelta = 0f;
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AddJumpForce();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameter);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.ForceReceiver.Reset();
        stateMachine.Player.IsJumping = false;
        StopAnimation(stateMachine.Player.AnimationData.JumpParameter);
        _jumpTimeoutDelta = 0;
    }

    public override void Update()
    {
        base.Update();
        _jumpTimeoutDelta += Time.deltaTime;

        if (stateMachine.Player.IsGrounded && _jumpTimeoutDelta >= _jumpTimeout)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    private void AddJumpForce()
    {
        stateMachine.Player.ForceReceiver.Jump(stateMachine.Player.jumpForce);
        Vector3 moveDirection = stateMachine.MainCameraTransform.right * stateMachine.MovementInput.x;
        moveDirection += stateMachine.MainCameraTransform.forward * stateMachine.MovementInput.y;
        moveDirection.y = 0;
        stateMachine.Player.ForceReceiver.AddForce(moveDirection * stateMachine.Player.Controller.velocity.magnitude);
        Debug.Log(stateMachine.Player.Controller.velocity.magnitude);
    }
}