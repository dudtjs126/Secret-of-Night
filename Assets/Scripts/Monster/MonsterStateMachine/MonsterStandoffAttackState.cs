using UnityEngine;

public class MonsterStandoffAttackState : MonsterBaseState
{
    private bool alreadyAppliedForce;

    public MonsterStandoffAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //[todo]원거리용 공격모션
        stateMachine.FieldMonsters.monsterAnimation.StartLongAttackAnimation();
        stateMachine.MovementSpeedModifier = 0;
    }

    public override void Exit()
    {
        stateMachine.FieldMonsters.monsterAnimation.StopLongAttackAnimation();
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Rotate(GetMovementDirection());
        Debug.Log("원거리공격");
        if (!IsInAttackRange() && IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
        if (DistanceFromPlayer() < 2f)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        ForceMove();

        float normalizedTime = 0.1f;/*GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");*/
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= 0 /*stateMachine.FieldMonsters.Data.ForceTransitionTime*/)
                TryApplyForce();
        }
        else
        {
            if (IsInChaseRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.PatrolState);
                return;
            }
        }
    }

    public void ShootingAttack()//원거리슈팅
    {

    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        alreadyAppliedForce = true;

    }
}
