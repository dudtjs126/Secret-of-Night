public class GameStartScene : BaseScene
{
    public override void Initizlize()
    {
        base.Initizlize();
        SceneType = Scene.GameStart;

        GameManager.Instance.soundManager.PlayBGM(GameManager.Instance.soundManager.GetSoundClip(SoundList.gameStartBGM));
    }
    public override void Clear()
    {

    }
}
