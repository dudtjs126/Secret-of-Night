using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    public Animator animator;

    private static readonly int _Idle = Animator.StringToHash("Idle");
    private static readonly int _Run = Animator.StringToHash("Run");
    private static readonly int _Attack = Animator.StringToHash("Attack");
    private static readonly int _Die = Animator.StringToHash("Die");
    private static readonly int _Walk = Animator.StringToHash("Walk");
    private static readonly int _Damage = Animator.StringToHash("Damage");
    private static readonly int _LongAttack = Animator.StringToHash("LongAttack");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartIdleAnimation()
    {
        animator.SetBool(_Idle, true);
    }

    public void StopIdleAnimation()
    {
        animator.SetBool(_Idle, false);
    }

    public void StartRunAnimation()
    {
        animator.SetBool(_Run, true);
    }

    public void StopRunAnimation()
    {
        animator.SetBool(_Run, false);
    }

    public void StartAttackAnimation()
    {
        animator.SetBool(_Attack, true);
    }

    public void StopAttackAnimation()
    {
        animator.SetBool(_Attack, false);
    }

    public void StartDieAnimation()
    {
        animator.SetTrigger(_Die);
    }

    public void StartWalkAnimation()
    {
        animator.SetBool(_Walk, true);
    }

    public void StopWalkAnimation()
    {
        animator.SetBool(_Walk, false);
    }

    public void StartDamageAnimation()
    {
        animator.SetTrigger(_Damage);
    }

    public void StartLongAttackAnimation()
    {
        animator.SetBool(_LongAttack, true);
    }

    public void StopLongAttackAnimation()
    {
        animator.SetBool(_LongAttack, false);
    }

}
