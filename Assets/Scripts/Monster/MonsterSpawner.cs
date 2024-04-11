using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInfo
{
    public Vector3 center;
    public float radius;
    public List<MonsterInfo> monsterID = new List<MonsterInfo>();

    public CircleInfo(Vector3 centerPosition, float circleRadius)
    {
        center = centerPosition;
        radius = circleRadius;
    }

    //각 원에 스폰될 몬스터를 ID로 분류하여 넣어주기
    public void AddMonsterInfo(MonsterInfo ID)
    {
        monsterID.Add(ID);
    }
}

public class MonsterSpawner : MonoBehaviour
{
    MonsterManager monsterManager;
    MonsterInfo monsterInfo;
    GameObject go;

    //MonsterSpot[] spotPoint;//가져온 monsterSpot들의 배열
    public List<CircleInfo> MonsterSpot = new List<CircleInfo>();//스팟리스트

    public int monsterNumber;
    int maxMonsterNumber = 10;//[todo]각 몬스터 개체수

    void Start()
    {
        //GetMonsterInfoByKey메서드를 가져오기 위해 몬스터매니저도 가져옴
        monsterManager = GameManager.Instance.monsterManager;

        //내 자식들의 monsterSpot을 가져온다.
        // spotPoint = GetComponentsInChildren<MonsterSpot>();

        AddInfo();

        SpawnAllMonster();
    }

    private void Update()
    {
        if (maxMonsterNumber < monsterNumber)
        {
            StartCoroutine(SpawnMonster());
        }
    }

    //스폰 및 무브 범위 정보 추가
    public void AddMonsterSpot(Vector3 center, float radius)
    {
        MonsterSpot.Add(new CircleInfo(center, radius));
    }

    private void SpawnAllMonster()
    {
        while (monsterNumber < maxMonsterNumber)
        {
            foreach (CircleInfo circle in MonsterSpot)
            {
                //원안의 몬스터들
                foreach (MonsterInfo monster in circle.monsterID)
                {
                    Vector3 spawnPoint = GetRandomPointInCircle(circle.center, circle.radius);
                    spawnPoint.y = circle.center.y; // y 값을 원래y값으로 설정
                    go = Instantiate(monster.prefab, spawnPoint, Quaternion.identity);
                    monsterNumber++;

                    //몬스터 정보 넣어줌
                    FieldMonsters fieldMonster = go.GetComponent<FieldMonsters>();
                    fieldMonster.SetPosition(spawnPoint);//포지션 전달
                    fieldMonster.Init(monster);

                    fieldMonster.circlePosition = circle.center;
                    fieldMonster.circleRadius = circle.radius;

                    Debug.Log(monsterNumber);
                    Debug.Log(spawnPoint);
                }
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        while (monsterNumber < maxMonsterNumber)//[todo]각 몬스터 개체수가 10일때까지
        {
            // 각 원의 스폰지점
            foreach (CircleInfo circle in MonsterSpot)
            {
                //원안의 몬스터들
                foreach (MonsterInfo monster in circle.monsterID)
                {
                    Vector3 spawnPoint = GetRandomPointInCircle(circle.center, circle.radius);
                    spawnPoint.y = circle.center.y; // y 값을 원래y값으로 설정
                    go = Instantiate(monster.prefab, spawnPoint, Quaternion.identity);
                    monsterNumber++;

                    //몬스터 정보 넣어줌
                    FieldMonsters fieldMonster = go.GetComponent<FieldMonsters>();
                    fieldMonster.SetPosition(spawnPoint);
                    fieldMonster.Init(monster);

                    fieldMonster.circlePosition = circle.center;
                    fieldMonster.circleRadius = circle.radius;

                    Debug.Log(monsterNumber);
                    Debug.Log(spawnPoint);
                }
            }
            yield return new WaitForSeconds(3f); // 3초 마다 1개씩
        }
    }

    public Vector3 GetRandomPointInCircle(Vector3 center, float radius)//랜덤한 스폰지점
    {
        //[todo]랜덤지점에 뭐가 없을때만 반환
        float angle = Random.Range(0f, Mathf.PI * 2f); // 랜덤한 각도 생성
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius; // 랜덤한 거리 생성
        float x = center.x + distance * Mathf.Cos(angle); // x 좌표 계산
        float z = center.z + distance * Mathf.Sin(angle); // z 좌표 계산
        return new Vector3(x, center.y, z); // 랜덤 지점 반환
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (CircleInfo circle in MonsterSpot)
        {
            Gizmos.DrawWireSphere(circle.center, circle.radius);
        }
    }

    void AddInfo()
    {
        //스팟추가
        //[todo]스팟은 맵에 추가
        AddMonsterSpot(new Vector3(3.2f, 3f, 69f), 30f);
        AddMonsterSpot(new Vector3(-58.9f, 3f, 121.8f), 30f);
        AddMonsterSpot(new Vector3(50.3f, 3f, 118.1f), 15f);
        AddMonsterSpot(new Vector3(84.6f, 5f, 180.3f), 15f);
        AddMonsterSpot(new Vector3(98.7f, 12f, 94.5f), 20f);
        AddMonsterSpot(new Vector3(116.6f, 12f, 170.8f), 35f);
        AddMonsterSpot(new Vector3(66.8f, 7f, 247.5f), 20f);
        AddMonsterSpot(new Vector3(31.7f, 7f, 292.1f), 30f);

        //각 스팟 몬스터 정보 추가
        MonsterSpot[0].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(2));
        //MonsterSpot[0].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(8));
        MonsterSpot[0].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(3));
        MonsterSpot[1].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(2));
        //MonsterSpot[1].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(8));
        MonsterSpot[1].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(3));
        MonsterSpot[2].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(1));
        MonsterSpot[2].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(2));
        MonsterSpot[2].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(4));
        MonsterSpot[3].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(1));
        MonsterSpot[3].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(3));
        MonsterSpot[3].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(4));
        MonsterSpot[4].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(7));
        MonsterSpot[5].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(7));
        MonsterSpot[6].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(5));
        MonsterSpot[7].AddMonsterInfo(monsterManager.GetMonsterInfoByKey(6));
    }
}
