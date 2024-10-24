using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MonsterSpot : MonoBehaviour
{
    public MonsterManager manager;
    public SpawnData spawnData;
    public MonsterInfo monsterInfo;

    List<FieldMonsters> monsterList = new List<FieldMonsters>();

    int maxMonsterCount = 10;

    public void Initialize(MonsterManager manager, SpawnData spawnData)
    {
        this.manager = manager;
        this.spawnData = spawnData;

        transform.position = spawnData.spotVector;
    }

    private void Start()
    {
        SpawnAllMonster();
        StartCoroutine(RespawnMonster());
    }

    public void SpawnAllMonster()
    {
        for (int i = 0; i < maxMonsterCount; i++)
        {
            SpawnMonster();
        }
    }

    IEnumerator RespawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);//30초 후 발동

            if (monsterList.Count < maxMonsterCount)
            {
                SpawnMonster();
                Debug.Log("재생성");
            }
            else//10마리 이상이면 코루틴 중지
            {
                StopCoroutine(RespawnMonster());
            }
        }
    }

    public void SpawnMonster()
    {
        int ID = spawnData.MonsterID[Random.Range(0, spawnData.MonsterID.Length)];//ID중에 하나 랜덤으로 뽑아옴

        monsterInfo = manager.monsterData.monsterDatabase.GetMonsterInfoByKey(ID);//몬스터 정보 불러오기
        Vector3 spawnPoint = GetRandomPointInCircle(spawnData.spotVector, spawnData.Radius);//랜덤 포지션 계산
        spawnPoint.y = spawnData.spotVector.y;//y값은 그대로
        if (monsterInfo.prefab == null)
            return;
        GameObject go = Instantiate(monsterInfo.prefab, spawnPoint, Quaternion.identity);//몬스터 생성

        FieldMonsters fieldMonster = go.GetComponent<FieldMonsters>();
        monsterList.Add(fieldMonster);

        fieldMonster.SetPosition(spawnPoint);//포지션 전달
        fieldMonster.Init(monsterInfo, this);

        fieldMonster.spawnSpot = transform.position;
        fieldMonster.spawnSpotRadius = spawnData.Radius;
    }

    public Vector3 GetRandomPointInCircle(Vector3 center, float radius)//랜덤한 스폰지점
    {
        int maxTries = 10;
        //[todo]랜덤지점에 뭐가 없을때만 반환
        for (int i = 0; i < maxTries; i++)
        {

            float angle = Random.Range(0f, Mathf.PI * 2f); // 랜덤한 각도 생성
            float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius; // 랜덤한 거리 생성
            float x = center.x + distance * Mathf.Cos(angle); // x 좌표 계산
            float z = center.z + distance * Mathf.Sin(angle); // z 좌표 계산
            Vector3 randomPoint = new Vector3(x, center.y, z); // 랜덤 지점 반환

            //return randomPoint;
            if (IsValidSpot(randomPoint))
            {
                return randomPoint;
            }
        }
        //다른 오브젝트가 있으면 일단 센터포지션 반환
        return center;
    }

    public void Remove(FieldMonsters fieldMonsters)
    {
        monsterList.Remove(fieldMonsters);
    }

    private bool IsValidSpot(Vector3 point)
    {
        RaycastHit hit;

        //레이로 쏜 곳에 다른 오브젝트가 있으면 false
        if (Physics.Raycast(point, Vector3.down, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("obstacle"))
            {
                return false;
            }
        }
        return false;
    }
}
