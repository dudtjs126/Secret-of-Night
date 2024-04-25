using System;
using System.Collections;
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
    public MonsterSpot monsterSpot;
    public HPBar hpBar;

    public Vector3 originalPosition;
    public Vector3 spawnSpot;
    public float spawnSpotRadius;

    public event Action<float> OnDamage;
    public event Action<GameObject> OnAttack;

    public Color damageColor = new Color(0.5f, 0f, 0f, 1f);
    public Color[] originalColor;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        forceReceiver = GetComponent<ForceReceiver>();
        controller = GetComponent<CharacterController>();
        monsterAnimation = GetComponent<MonsterAnimation>();
        attackCollider = GetComponent<BoxCollider>();
        hpBar = GetComponent<HPBar>();
        hpBar.DeActive();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void Init(MonsterInfo monsterInfo, MonsterSpot monsterSpot)
    {
        myInfo = monsterInfo;
        this.monsterSpot = monsterSpot;

        HP = myInfo.HP;

        itemDataBase = GameManager.Instance.dataManager.itemDataBase;

        stateMachine = new MonsterStateMachine(this);
        stateMachine.ChangeState(stateMachine.IdleState);

        //원래 컬러 저장
        originalColor = new Color[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalColor[i] = meshRenderers[i].material.color;
        }
    }

    public void Init(MonsterInfo monsterInfo)
    {
        myInfo = monsterInfo;

        HP = myInfo.HP;

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
        Vector3 movePoint = monsterSpot.GetRandomPointInCircle(spawnSpot, spawnSpotRadius);

        return movePoint;
    }

    public void OffCollider()
    {
        stateMachine.FieldMonsters.controller.enabled = false;
        stateMachine.FieldMonsters.attackCollider.enabled = false;
    }

    //base에 있는 takedamage구독
    private Coroutine fadeOutCoroutine;
    public void TakeDamage(float Damage)
    {
        OnDamage?.Invoke(Damage);

        DamageColorChange();

        if (!hpBar.isActiveAndEnabled)
        {
            ShowHP();
        }
        else
        {
            RestartFadeOut();
        }

        hpBar.SetHP(HP / myInfo.HP);
    }

    private void ShowHP()
    {
        hpBar.Active();
        hpBar.ResetAlpha();

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(FadeOutHPBar());
    }

    private void RestartFadeOut()
    {
        hpBar.ResetAlpha();

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(FadeOutHPBar());
    }

    IEnumerator FadeOutHPBar()
    {
        yield return new WaitForSeconds(5f);
        hpBar.FadeOut(2f);
        yield return new WaitForSeconds(2f);
        hpBar.DeActive();
    }

    public void dropItem(Item _item)
    {
        Vector3 throwPosition = transform.position + transform.up * Random.Range(0.5f, 0.5f);

        Instantiate(_item.Prefab, throwPosition, Quaternion.identity);
    }

    public void DropData()
    {
        float totalWeight = 0f;
        float randomWeight;
        float accumulatedWeight = 0f;
        int selectItem;

        //전체가중치 합 계산
        foreach (float weight in myInfo.Weigth)
        {
            totalWeight += weight;
        }
        randomWeight = Random.Range(0f, totalWeight);//랜덤으로 뽑은 가중치값

        for (int i = 0; i < myInfo.DropItem.Length; i++)
        {
            accumulatedWeight += myInfo.Weigth[i];//i번째까지 가중치의 합
            if (randomWeight <= accumulatedWeight)//랜덤가중치값보다 커지면
            {
                selectItem = myInfo.DropItem[i];
                dropItem(itemDataBase.GetData(selectItem));
                break;
            }
        }
    }

    public void DamageColorChange()
    {
        StartCoroutine(DamageColorCoroutine());
    }

    private IEnumerator DamageColorCoroutine()
    {
        // 피해를 입었을 때의 색상으로 변경
        DamageColor();

        yield return new WaitForSeconds(0.5f);

        // 원래의 색상으로 되돌리기
        ResetColor();
    }

    private void DamageColor()
    {
        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.material.SetColor("_BaseColor", damageColor);
        }
    }

    private void ResetColor()//원래 색상
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.SetColor("_BaseColor", originalColor[i]);
        }
    }
}
