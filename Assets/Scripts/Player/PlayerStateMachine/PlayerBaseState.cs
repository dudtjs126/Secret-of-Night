using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected int skillKey = -1;
    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
    }
    public virtual void Enter()
    {
        AddPlayerActionCallbacks();
    }

    public virtual void Exit()
    {
        RemovePlayerActionCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void Update()
    {
        GroundedCheck();
        Move();
    }
    protected virtual void AddPlayerActionCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        stateMachine.Player.Input.PlayerActions.Run.performed += OnRunStarted;
        stateMachine.Player.Input.PlayerActions.Run.canceled += OnRunCanceled;

        stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;

        stateMachine.Player.Input.PlayerActions.Dodge.started += OnDodgeStarted;

        stateMachine.Player.Input.PlayerActions.Attack.started += OnAttackStarted;
        stateMachine.Player.Input.PlayerActions.Attack.canceled += OnAttackCanceled;
        stateMachine.Player.Input.PlayerActions.Skill1.started += OnSkill1Started;
        stateMachine.Player.Input.PlayerActions.Skill2.started += OnSkill2Started;
    }


    protected virtual void RemovePlayerActionCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        stateMachine.Player.Input.PlayerActions.Run.started -= OnRunStarted;
        stateMachine.Player.Input.PlayerActions.Run.canceled -= OnRunCanceled;

        stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;

        stateMachine.Player.Input.PlayerActions.Dodge.started -= OnDodgeStarted;

        stateMachine.Player.Input.PlayerActions.Attack.started -= OnAttackStarted;
        stateMachine.Player.Input.PlayerActions.Attack.canceled -= OnAttackCanceled;
        stateMachine.Player.Input.PlayerActions.Skill1.started -= OnSkill1Started;
        stateMachine.Player.Input.PlayerActions.Skill2.started -= OnSkill2Started;
    }
    #region addevent
    private void OnSkill1Started(InputAction.CallbackContext obj)
    {
        skillKey = 1;
        stateMachine.ChangeState(stateMachine.SkillState);
    }
    private void OnSkill2Started(InputAction.CallbackContext obj)
    {
        skillKey = 2;
        stateMachine.ChangeState(stateMachine.SkillState);
    }
    protected virtual void OnJumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (stateMachine.Player.IsGrounded && stateMachine.Player.IsJumping == false)
        {
            stateMachine.Player.IsJumping = true;
            stateMachine.ChangeState(stateMachine.JumpState);
        }
    }

    protected virtual void OnRunStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        stateMachine.Player.IsRunning = true;
    }

    private void OnDodgeStarted(InputAction.CallbackContext obj)
    {
        if (stateMachine.Player.IsDodgeing == false && stateMachine.Player.IsJumping == false)
        {
            stateMachine.Player.IsDodgeing = true;
            stateMachine.ChangeState(stateMachine.DodgeState);
        }
    }
    protected virtual void OnAttackStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        stateMachine.Player.IsAttacking = true;
    }
    protected virtual void OnAttackCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        stateMachine.Player.IsAttacking = false;
    }
    protected virtual void OnRunCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        stateMachine.Player.IsRunning = false;
    }
    protected virtual void OnMovementCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }
    #endregion
    //Read InputActions
    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        // 움직임이 없는 경우에는 지속적으로 중력값을 받는다
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);

        //윰직임이 있는 경우에는
        if (CanMove())
        {
            Vector3 moveDirection = MoveDirection_Ver_Key();
            if (stateMachine.Player.IsGrounded)
            {
                float moveSpeed = GetMovementSpeed();
                stateMachine.Player.Controller.Move(((moveDirection * moveSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);
            }
            // test
            if (stateMachine.MovementInput.x != 0)
            {
                float temp = stateMachine.MovementInput.y == 0 ? 1 : stateMachine.MovementInput.y;
                stateMachine.cameraScript.SetAngleH(45 * stateMachine.MovementInput.x * temp);
            }
            else
            {
                stateMachine.cameraScript.ResetAngleH();
            }
            Rotate(moveDirection);
        }
    }
    private bool CanMove()
    {
        return stateMachine.MovementInput != Vector2.zero && !stateMachine.Player.IsDodgeing && !stateMachine.Player.IsJumping;
    }
    // 카메라 방향으로 이동하는 방식
    private Vector3 MoveDirection_Ver_Camera()
    {
        Vector3 moveDirection = stateMachine.MainCameraTransform.right * stateMachine.MovementInput.x;
        moveDirection += stateMachine.MainCameraTransform.forward * stateMachine.MovementInput.y;
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;
        return moveDirection;
    }
    // 키 입력 방향으로 이동하는 방식
    private Vector3 MoveDirection_Ver_Key() // test
    {
        Vector3 moveDirection = new Vector3(stateMachine.MovementInput.x, 0, stateMachine.MovementInput.y);
        return moveDirection.normalized;
    }
    private void Rotate(Vector3 lookDirection)
    {
        lookDirection = new Vector3(lookDirection.x, lookDirection.y, lookDirection.z); // test
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        stateMachine.Player.transform.rotation = Quaternion.Slerp(stateMachine.Player.transform.rotation,
            lookRotation, Time.deltaTime * stateMachine.Player.rotationDamping);

    }
    private void GroundedCheck()
    {
        Vector3 spherePosition = stateMachine.Player.transform.position;
        stateMachine.Player.IsGrounded = Physics.CheckSphere(spherePosition, 0.09f, stateMachine.Player.GroundLayerMask,
                QueryTriggerInteraction.Ignore);
    }
    protected float GetMovementSpeed()
    {
        return stateMachine.Player.PlayerData.MoveSpeed * stateMachine.Player.MovementSpeedModifier;
    }
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
