using TMPro;
using UnityEngine;


public class PickupController : MonoBehaviour
{
    [SerializeField] private float _range;
    private bool _pickupActivated = false;
    private bool _playerDie = false; // 플레이어가 죽었을 때 아이템 줍지 못하게

    private Collider _collider; //�浹ü ����

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private TextMeshProUGUI _pickupText;

    private Shop _shop;

    private void Start()
    {
        GameManager.Instance.playerManager.playerData.OnDie += DontPickUp;
        _shop = FindObjectOfType<Shop>();
    }

    private void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (_playerDie == false && Input.GetKey(KeyCode.G))
        {
            PickUp();
        }
        else
        {
            return;
        }
    }

    // ������ ���̾��ũ �˻�
    private void CheckItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range, _layerMask);

        if (colliders.Length > 0)
        {
            _collider = colliders[0];
            ItemInfoAppear();
        }
        else
        {
            _collider = null;
            ItemInfoDisappear();
        }
    }


    private void ItemInfoAppear()
    {
        if (_playerDie == false)
        {
            _pickupActivated = true;
            _pickupText.gameObject.SetActive(true);
            _pickupText.text = _collider.transform.GetComponentInChildren<ItemObject>().item.ItemName + " 획득하기" + " [G]";
        }
        else
        {
            _pickupText.gameObject.SetActive(false);
        }

    }

    private void ItemInfoDisappear()
    {
        _pickupActivated = false;
        _pickupText.gameObject.SetActive(false);
    }

    public void PickUp()
    {
        if (_pickupActivated && _collider != null)
        {

            float distance = Vector3.Distance(transform.position, _collider.transform.position);

            if (distance <= _range)
            {
                if (_collider.transform.GetComponentInChildren<ItemObject>().item.ItemID == 33)
                {
                    GameManager.Instance.playerManager.playerData.Gold += _collider.transform.GetComponentInChildren<ItemObject>().item.Money;
                    Inventory.instance.CashUpdate();
                    _shop.CashUpdate();
                    Destroy(_collider.gameObject);
                }
                else
                {
                    Inventory.instance.AddItem(_collider.transform.GetComponentInChildren<ItemObject>().item, 1);
                    Destroy(_collider.gameObject);
                    ItemInfoDisappear();
                }

            }
        }
    }

    private void DontPickUp()
    {
        _playerDie = true;
    }
}