using UnityEngine;

public class MonsterBaseState : IState, IDamageable

{
    protected MonsterStateMachine stateMachine;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

    }

    public virtual void Enter()
    {
        stateMachine.FieldMonsters.OnDamage += TakeDamage;
    }

    public virtual void Exit()
    {
        stateMachine.FieldMonsters.OnDamage -= TakeDamage;
    }

    public virtual void HandleInput()
    {

    }

    public virtual void Update()
    {
        //attackStance에 따라 추격할지 말지
        //임시
        //if (stateMachine.FieldMonsters.myInfo.AtkStance == false)//false가 0, true가 1
        //{
        //    //선공X
        //    //TakeDamage();
        //    if (IsInChaseRange())
        //    {
        //        stateMachine.ChangeState(stateMachine.ChasingState);
        //        return;
        //    }
        //}
        if (stateMachine.FieldMonsters.myInfo.AtkStance)
        {
            //선공0
            if (IsInChaseRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
            return;
        }

    }

    public virtual void PhysicsUpdate()
    {

    }

    //애니메이션
    protected void StartAnimation(int animationHash)
    {
        stateMachine.FieldMonsters.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.FieldMonsters.Animator.SetBool(animationHash, false);
    }

    protected void Move()//v
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);
        Move(movementDirection);
    }

    protected void ForceMove()//네브메쉬로 수정 고려
    {
        stateMachine.FieldMonsters.controller.Move(stateMachine.FieldMonsters.forceReceiver.Movement * Time.deltaTime);
    }


    private void Move(Vector3 direction)//v
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

            stateMachine.FieldMonsters.transform.rotation = Quaternion.Slerp(stateMachine.FieldMonsters.transform.rotation, targetRotation, stateMachine.rotationDamping * Time.deltaTime);
        }
    }

    private Vector3 GetMovementDirection()//v
    {
        //stateMachine.FieldMonsters.originalPosition;
        if (!IsInChaseRange())
        {
            return (stateMachine.FieldMonsters.originalPosition - stateMachine.FieldMonsters.transform.position).normalized;
        }
        return (stateMachine.Target.transform.position - stateMachine.FieldMonsters.transform.position).normalized;
    }

    private float GetMovementSpeed()//v
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;

        return movementSpeed;
    }

    protected bool IsInChaseRange()//v
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.FieldMonsters.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.FieldMonsters.myInfo.TargetRange * stateMachine.FieldMonsters.myInfo.TargetRange;
    }

    public void TakeDamage(float Damage)
    {
        stateMachine.FieldMonsters.monsterAnimation.StartDamageAnimation();

        float Def = stateMachine.FieldMonsters.myInfo.Daf;

        float damage = Damage - Def;
        Debug.Log(damage);
        if (damage > 0)
        {
            stateMachine.FieldMonsters.HP -= damage;
        }

        Debug.Log(stateMachine.FieldMonsters.HP);

        if (stateMachine.FieldMonsters.HP <= 0)
        {
            stateMachine.FieldMonsters.HP = 0;
            stateMachine.ChangeState(stateMachine.DyingState);

        }
        else
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
        //return true;
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
}