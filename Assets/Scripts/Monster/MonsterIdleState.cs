using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MonsterIdleState : IState
{
    protected MonsterStateMachine stateMachine;

    public MonsterIdleState(MonsterStateMachine monsterStateMachine)
    {
        stateMachine = monsterStateMachine;

    }

    public virtual void Enter()
    {
        stateMachine.MovementSpeedModifier = 0;

        //[�߰�] �ִϸ��̼�
    }

    public virtual void Exit()
    {
        //[�߰�] �ִϸ��̼�
    }

    public virtual void HandleInput()
    {

    }

    public virtual void Update()
    {
        Move();
        if (IsInChaseRange())
        {
            //stateMachine�� ChasingState�߰�->�ǿ��Կ��� �����
            //stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }
    }

    public virtual void PhysicsUpdate()
    {

    }

    //�ִϸ��̼�

    //protected void StartAnimation(int animationHash)
    //{
    //    stateMachine.FieldMonsters.Animator.SetBool(animationHash, true);
    //}

    //protected void StopAnimation(int animationHash)
    //{
    //    stateMachine.FieldMonsters.Animator.SetBool(animationHash, false);
    //}

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);
        Move(movementDirection);
    }

    protected void ForceMove()//�׺�޽��� ���� ���
    {
        stateMachine.FieldMonsters.controller.Move(stateMachine.FieldMonsters.forceReceiver.Movement * Time.deltaTime);
    }


    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.FieldMonsters.controller.Move(((direction * movementSpeed) + stateMachine.FieldMonsters.forceReceiver.Movement) * Time.deltaTime);
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            stateMachine.FieldMonsters.transform.rotation = Quaternion.Slerp(stateMachine.FieldMonsters.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private Vector3 GetMovementDirection()
    {
        return (stateMachine.Target.transform.position - stateMachine.FieldMonsters.transform.position).normalized;
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;

        return movementSpeed;
    }

    //protected float GetNormalizedTime(Animator animator, string tag)
    //{
    //    AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

    //    if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
    //    {
    //        return nextInfo.normalizedTime;
    //    }
    //    else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
    //    {
    //        return currentInfo.normalizedTime;
    //    }
    //    else
    //    {
    //        return 0f;
    //    }
    //}

    //
    protected bool IsInChaseRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.FieldMonsters.transform.position).sqrMagnitude;

        //[����]PlayerChasingRange ã�Ƽ� �ֱ�.
        return playerDistanceSqr <= 0/*stateMachine.FieldMonsters.Data.PlayerChasingRange * stateMachine.FieldMonsters.Data.PlayerChasingRange*/;
    }


}
