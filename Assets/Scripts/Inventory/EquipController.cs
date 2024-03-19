using UnityEngine;

public class EquipController : MonoBehaviour
{
    public GameObject curEquip;
    public Transform equipParent;

    private PlayerGameData _playerData;
    private float _defultDamage;

    private void Start()
    {
        _playerData = GameManager.Instance.playerManager.playerData;
        EquipDefaultWeapon();
    }

    // �⺻ ������ ������ �����ͼ� ����
    private void EquipDefaultWeapon()
    {
        Item defaultWeapon = GameManager.Instance.dataManager.itemDataBase.GetData(8);

        NewEquip(defaultWeapon);
        EquipWeapon(defaultWeapon.ItemID, defaultWeapon.Damage);

    }

    // ĳ���� �տ� ���� ����
    public void NewEquip(Item _item)
    {
        curEquip = Instantiate(_item.Prefab, equipParent);
        curEquip.GetComponent<Collider>().enabled = false; // �տ� ������ ����� �ν� x

    }
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip.GetComponent<Collider>().enabled = true;
            curEquip = null;
        }
    }

    // ���� ������ ���ݷ� ��ȭ
    public void EquipWeapon(int _id, float _damage)
    {
        _defultDamage = _playerData.Damage;
        _playerData.Damage = _defultDamage + _damage;
    }

    public void UnEquipWeapon()
    {
        _playerData.Damage = _defultDamage;
    }
}
