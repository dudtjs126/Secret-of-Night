using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PlayerGameData _playerData;

    public bool speedItemInUse = false;

    public void Start()
    {
        _playerData = GameManager.Instance.playerManager.playerData;
    }

    // �� ȸ��
    public void SmallHpPotion(float _amount)
    {
        _playerData.CurHP = Mathf.Min(_playerData.CurHP + _amount, _playerData.MaxHP);
    }

    public void BigHpPotion(float _amount)
    {
        _playerData.CurHP = Mathf.Min(_playerData.CurHP + _amount, _playerData.MaxHP);
    }

    // ���� ȸ��
    public void SmallMpPotion(float _amount)
    {
        _playerData.CurMP = Mathf.Min(_playerData.CurMP + _amount, _playerData.MaxMP);
    }

    public void BigMpPotion(float _amount)
    {
        _playerData.CurMP = Mathf.Min(_playerData.CurMP + _amount, _playerData.MaxMP);
    }

    // ���׹̳� ȸ��
    public void SmallSpPotion(float _amount)
    {
        _playerData.CurSP = Mathf.Min(_playerData.CurSP + _amount, _playerData.MaxSP);
    }

    public void BigSpPotion(float _amount)
    {
        _playerData.CurSP = Mathf.Min(_playerData.CurSP + _amount, _playerData.MaxSP);
    }

    // �̵��ӵ� ����
    public void SpeedPotion(float _amount)
    {
        if (!speedItemInUse) //������ ��� ���� �ƴ� ��쿡�� ������ ���
        {
            _playerData.MoveSpeed += _amount;
            StartCoroutine(SpeedCoroutine(_amount));
            speedItemInUse = true;
        }
    }

    IEnumerator SpeedCoroutine(float _amount)
    {
        Debug.Log("���ǵ� �� : " + _playerData.MoveSpeed);
        yield return new WaitForSeconds(5f); //TODO 60�ʷ� ����
        _playerData.MoveSpeed -= _amount;
        Debug.Log("���� : " + _playerData.MoveSpeed);
        speedItemInUse = false;
    }
}
