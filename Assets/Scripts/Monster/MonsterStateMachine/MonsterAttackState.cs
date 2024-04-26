using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    private bool alreadyAppliedForce;
    private bool _attack = false;

    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = 0;

        if (DistanceFromPlayer() >= 2f && stateMachine.FieldMonsters.myInfo.ShortDistance)
        {
            stateMachine.ChangeState(stateMachine.StandoffAttackState);
            return;
        }
        stateMachine.FieldMonsters.monsterAnimation.StartAttackAnimation();

        stateMachine.FieldMonsters.OnAttack += NomalAttack;
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.FieldMonsters.monsterAnimation.StopAttackAnimation();

        stateMachine.FieldMonsters.OnAttack -= NomalAttack;
    }

    public override void Update()
    {
        base.Update();
        Rotate(GetMovementDirection());

        //선공몬스터가 아닐때
        if (stateMachine.FieldMonsters.myInfo.AtkStance == false)
        {
            if (IsInAttackRange())
            {
                return;
            }
            else if (IsInChaseRange() && !IsInAttackRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Debug.Log("근거리공격");

        ShortAttackCollider();

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

    private void ShortAttackCollider()
    {
        float progessiveTime = stateMachine.FieldMonsters.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
        if (progessiveTime < 0.5f)//애니메이션 플레이중
        {
            _attack = false;
            stateMachine.FieldMonsters.attackCollider.enabled = false;
        }

        if (!_attack && progessiveTime >= 0.5f)//애니메이션 끝났을때
        {
            _attack = true;
            stateMachine.FieldMonsters.attackCollider.enabled = true;
        }
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        alreadyAppliedForce = true;

        //stateMachine.FieldMonsters.ForceReceiver.Reset();

        //stateMachine.Enemy.ForceReceiver.AddForce(stateMachine.Enemy.transform.forward * stateMachine.Enemy.Data.Force);

    }

    //기본공격
    public void NomalAttack(GameObject other)
    {
        //other의 IDamageable
        other.TryGetComponent<IDamageable>(out IDamageable go);
        //데미지 넣어줌
        go.TakeDamage(stateMachine.FieldMonsters.myInfo.Damage);
    }
}
