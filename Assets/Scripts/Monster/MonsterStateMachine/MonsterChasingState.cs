using UnityEngine;

public class MonsterChasingState : MonsterBaseState
{
    public MonsterChasingState(MonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = 1;

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

        Move();

        if (!IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.PatrolState);
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
        else
        {
            IsInMyPosition();
        }
    }

    private bool IsInAttackRange()//v
    {
        // if (stateMachine.Target.IsDead) { return false; }

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.FieldMonsters.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.FieldMonsters.myInfo.AtkRange * stateMachine.FieldMonsters.myInfo.AtkRange;
    }

    private void IsInMyPosition()
    {
        Move();
        Vector3 myOriginalPosition = stateMachine.FieldMonsters.originalPosition;
        Vector3 currentPosition = stateMachine.FieldMonsters.transform.position;

        float distance = (currentPosition - myOriginalPosition).sqrMagnitude;

        //원래 포지션으로 가면 -> Patrol State로 바꿈
        if (myOriginalPosition.x <= 0.5f)
        {
            stateMachine.ChangeState(stateMachine.PatrolState);
        }
    }
}
