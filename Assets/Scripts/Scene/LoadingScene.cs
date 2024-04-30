public class LoadingScene : BaseScene
{
    public override void Initizlize()
    {
        base.Initizlize();
        GameManager.Instance.soundManager.Stop();
        GameManager.Instance.playerManager.playerData.SaveData();
    }
    public override void Clear()
    {

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}