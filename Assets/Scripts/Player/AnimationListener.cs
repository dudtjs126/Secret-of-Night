using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    public Transform skillPos;
    public ParticleSystem[] SlashEffects;
    public float attackRange = 1.5f;
    public LayerMask enemyLayer;

    public Dictionary<string, ParticleSystem> playerSkillEffectDic = new Dictionary<string, ParticleSystem>();
    private PlayerController _playerController;
    public void Start()
    {
        foreach (var skill in GameManager.Instance.playerManager.playerSkillList.Values)
        {
            playerSkillEffectDic.Add(skill.playerSkillData.Name, skill.skillEffect.GetComponent<ParticleSystem>());
        }
        _playerController = GetComponentInParent<PlayerController>();
    }
    public void SlashEffect(int index)
    {
        SlashEffects[index].Play();
    }
    public void SlashAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer, QueryTriggerInteraction.Ignore);

        foreach (var hitCollider in hitEnemies)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if (angle < 90f)
            {
                Debug.Log("SlashAttack: " + hitCollider.name);
                hitCollider.GetComponent<IDamageable>()?.TakeDamage(_playerController.PlayerData.Damage);
            }
        }
    }
    public void ShowPlayerSkillEffect(string name)
    {
        if (name == "StingAttack")
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Skills/StingAttackEffect"), skillPos).GetComponent<ParticleSystem>().Play();
        }
    }
    public void ShowPlayerSkill(string name)
    {
        ParticleSystem go = Instantiate(playerSkillEffectDic[name], skillPos);
        go.transform.parent = null;
        go.Play();
        StartCoroutine(DeacitveEffect(go, name));
    }
    public void PlaySound(string name)
    {
        SoundList sound = (SoundList)System.Enum.Parse(typeof(SoundList), name);
        GameManager.Instance.soundManager.PlayEffectSound(GameManager.Instance.soundManager.GetSoundClip(sound), transform.position, 1.0f);
    }
    IEnumerator DeacitveEffect(ParticleSystem go, string name, float delaytime = 2.0f)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(go.gameObject);
    }
    public void SkillDamage(string name)
    {
        float skillDamage = GameManager.Instance.playerManager.GetSkillDamage(name);
        float skillRange = GameManager.Instance.playerManager.GetSkillRange(name);
        float skillAngle = GameManager.Instance.playerManager.GetSkillAngle(name);
        // 추후 엑셀데이터로 스킬 범위와 스킬 각도를 추가함으로써 최적화 할 수 있겠다.
        if (name == "JumpSkill")
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, skillRange, enemyLayer, QueryTriggerInteraction.Ignore);
            Debug.DrawLine(transform.position, transform.position + transform.forward * skillRange, Color.red, 2.0f);
            foreach (var hitCollider in hitEnemies)
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToTarget);
                if (angle < skillAngle)
                {
                    hitCollider.GetComponent<IDamageable>()?.TakeDamage(skillDamage);
                }
            }
        }
        else if (name == "StoneSlash")
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, skillRange, enemyLayer, QueryTriggerInteraction.Ignore);

            foreach (var hitCollider in hitEnemies)
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToTarget);

                if (angle < skillAngle)
                {
                    Debug.Log("JumpAttack: " + hitCollider.name);
                    hitCollider.GetComponent<IDamageable>()?.TakeDamage(skillDamage);
                }
            }
        }
        else if (name == "AssassinAttack")
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, out RaycastHit hitInfo, skillRange, ~(enemyLayer | gameObject.layer)))
            {
                attackRange = hitInfo.distance - 0.2f;
            }

            Collider[] hitEnemies = Physics.OverlapBox(transform.position + transform.rotation * Vector3.forward * (skillRange / 2), new Vector3(1f, 1.0f, attackRange), transform.rotation, enemyLayer, QueryTriggerInteraction.Ignore);

            foreach (var hitCollider in hitEnemies)
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToTarget);

                if (angle < skillAngle)
                {
                    Debug.Log("AssassinAttack: " + hitCollider.name);
                    hitCollider.GetComponent<IDamageable>()?.TakeDamage(skillDamage);
                }
            }
        }
        else if (name == "StingAttack")
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, skillRange, enemyLayer, QueryTriggerInteraction.Ignore);

            foreach (var hitCollider in hitEnemies)
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToTarget);

                if (angle < skillAngle)
                {
                    Debug.Log("StingAttack: " + hitCollider.name);
                    hitCollider.GetComponent<IDamageable>()?.TakeDamage(skillDamage);
                }
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Semi-transparent red

        // Draw a cube where the OverlapBox is going to be, with the same size and orientation
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.rotation * Vector3.forward * 2.5f, transform.rotation, new Vector3(1f, 1.0f, 5.0f));
        Gizmos.DrawCube(Vector3.zero, Vector3.one); // The cube is drawn at the origin of the Gizmos matrix
    }
    public void MoveDash()
    {
        float moveDistance = 5.0f;

        if (Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, out RaycastHit hitInfo, moveDistance, ~(enemyLayer | gameObject.layer)))
        {
            moveDistance = hitInfo.distance - 0.3f;
        }

        _playerController.gameObject.transform.position += transform.forward * moveDistance;
    }

    void OnDrawGizmosSelected()
    {
        // Draw the attack range arc for visual debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 forward = transform.forward * attackRange;
        Vector3 right = Quaternion.Euler(0, 90, 0) * forward;
        Vector3 left = Quaternion.Euler(0, -90, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + forward);
        Gizmos.DrawLine(transform.position, transform.position + right);
        Gizmos.DrawLine(transform.position, transform.position + left);
    }
}
