public class MonsterIdleState : MonsterBaseState
{

    public MonsterIdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.MovementSpeedModifier = 0f;

        stateMachine.FieldMonsters.monsterAnimation.StartIdleAnimation();
        stateMachine.ChangeState(stateMachine.PatrolState);
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.FieldMonsters.monsterAnimation.StopIdleAnimation();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {

    }
}
