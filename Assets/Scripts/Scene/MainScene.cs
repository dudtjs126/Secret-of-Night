using UnityEngine;

public class MainScene : BaseScene
{
    [SerializeField] private AudioClip _bgm;

    public override void Initizlize()
    {
        base.Initizlize();

        SceneType = Scene.Main;

        GameManager.Instance.soundManager.PlayBGM(_bgm);
        GameManager.Instance.monsterManager.CreatMonsterSpot();
    }
    public override void Clear()
    {

    }
}
