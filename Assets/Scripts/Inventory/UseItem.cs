using UnityEngine;

public class UseItem : MonoBehaviour
{
    public PlayerGameData PlayerData { get; set; }

    public void Start()
    {

        PlayerData = GameManager.Instance.playerManager.playerData;

        Debug.Log(PlayerData.CurHP);
    }

    // �� ȸ��
    public void SmallHpPotion(float amount)
    {
        PlayerData.CurHP = Mathf.Min(PlayerData.CurHP + amount, PlayerData.MaxHP);
    }

    public void BigHpPotion(float amount)
    {
        PlayerData.CurHP = Mathf.Min(PlayerData.CurHP + amount, PlayerData.MaxHP);
    }

    // ���� ȸ��
    public void SmallMpPotion(float amount)
    {
        PlayerData.CurMP = Mathf.Min(PlayerData.CurMP + amount, PlayerData.MaxMP);
    }

    public void BigMpPotion(float amount)
    {
        PlayerData.CurMP = Mathf.Min(PlayerData.CurMP + amount, PlayerData.MaxMP);
    }

    // ���׹̳� ȸ��
    public void SmallSpPotion(float amount)
    {
        PlayerData.CurMP = Mathf.Min(PlayerData.CurSP + amount, PlayerData.MaxSP);
    }

    public void BigSpPotion(float amount)
    {
        PlayerData.CurMP = Mathf.Min(PlayerData.CurSP + amount, PlayerData.MaxSP);
    }
}
