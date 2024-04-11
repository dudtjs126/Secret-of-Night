using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldMonsters : MonoBehaviour, IDamageable
{


    [field: Header("Reference")]
    [field: SerializeField] public MonsterInfo myInfo;
    public float HP = 0;

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public ForceReceiver forceReceiver { get; private set; }
    public CharacterController controller { get; private set; }
    public BoxCollider attackCollider;

    public MonsterAnimation monsterAnimation;
    private MonsterStateMachine stateMachine;
    public ItemDataBase itemDataBase;
    public MonsterSpawner monsterSpawner;

    public Vector3 originalPosition;
    public Vector3 circlePosition;
    public float circleRadius;

    public event Action<float> OnDamage;
    public event Action<GameObject> OnAttack;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        forceReceiver = GetComponent<ForceReceiver>();
        controller = GetComponent<CharacterController>();
        monsterAnimation = GetComponent<MonsterAnimation>();
        attackCollider = GetComponent<BoxCollider>();

        monsterSpawner = gameObject.AddComponent<MonsterSpawner>();
    }

    public void Init(MonsterInfo monsterInfo)
    {

        myInfo = monsterInfo;

        HP = myInfo.HP;

        itemDataBase = GameManager.Instance.dataManager.itemDataBase;
        //monsterSpawner = GameManager.Instance.monsterManager.monsterSpawner;

        stateMachine = new MonsterStateMachine(this);
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine?.HandleInput();
        stateMachine?.Update();
    }

    private void FixedUpdate()
    {
        stateMachine?.PhysicsUpdate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, myInfo.TargetRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, myInfo.AtkRange);
    }

    public void SetPosition(Vector3 position)//처음 스폰 포지션 설정
    {
        originalPosition = position;
    }

    public Vector3 GetNewMovePoint()//랜덤한 무브지점
    {
        Vector3 movePoint = monsterSpawner.GetRandomPointInCircle(circlePosition, circleRadius);

        return movePoint;
    }

    //base에 있는 takedamage구독
    public void TakeDamage(float Damage)
    {
        OnDamage?.Invoke(Damage);
    }

    public void OnTriggerEnter(Collider other)
    {
        //[todo]플레이어 콜라이더만
        OnAttack?.Invoke(other.gameObject);
    }

    public void dropItem(Item _item)
    {
        Vector3 throwPosition = transform.position + transform.forward * Random.Range(0.5f, 0.5f);

        Instantiate(_item.Prefab, throwPosition, Quaternion.identity);
    }

    public void DropData()
    {
        //int count;

        //float itemWeight = 1;
        //float _weigth = 0;

        //for (int j = 0; j < myInfo.Weigth.Length; j++)
        //{
        //    if (i == j)
        //    {
        //        _weigth = j;

        //    }
        //}

        for (int i = 0; i < myInfo.DropItem.Length; i++)//랜덤으로 해야할것같음
        {
            dropItem(itemDataBase.GetData(myInfo.DropItem[i]));

        }
    }

}
