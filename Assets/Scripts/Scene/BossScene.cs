using UnityEngine;
using UnityEngine.Events;

public class BossScene : BaseScene
{
    public static UnityEvent<Phase1Boss> OnBossSpawned = new();

    public Phase1Boss _boss;
    [SerializeField] private Transform _bossSpot;
    public override void Initizlize()
    {
        base.Initizlize();
        SceneType = Scene.Boss;

        GameManager.Instance.soundManager.PlayBGM(GameManager.Instance.soundManager.GetSoundClip(SoundList.bossBGM));
        Invoke("SpawnBoss", 0.1f);
    }

    private void SpawnBoss()
    {
        //var boss = Instantiate(_boss, _bossSpot.position, Quaternion.identity);
        OnBossSpawned?.Invoke(_boss);
    }

    public override void Clear()
    {

    }
    protected override void OnDestroy()
    {
        GameManager.Instance.playerManager.playerData.SaveData();
    }
}
