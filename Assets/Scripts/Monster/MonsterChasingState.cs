public class MonsterChasingState : MonsterIdleState
{
    public MonsterChasingState(MonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1;
        base.Enter();

        stateMachine.FieldMonsters.monsterAnimation.StartRunAnimation();
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.FieldMonsters.monsterAnimation.StopRunAnimation();
    }

    public override void Update()
    {
        base.Update();

        if (!IsInChaseRange())
        {
            //[todo]������ġ�� ���ư��� �ڵ�

            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
    }

    private bool IsInAttackRange()//v
    {
        // if (stateMachine.Target.IsDead) { return false; }

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.FieldMonsters.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.FieldMonsters.myInfo.Range * stateMachine.FieldMonsters.myInfo.Range;
    }


}
