using UnityEngine;
using UnityEngine.SceneManagement;

public class MapObjectManager : MonoBehaviour
{
    public GameObject magicCircle2;

    /*public void BossScene()
    {
        if (//�÷��̾ ��ġ�� ����ϸ�)
        {
            Debug.Log("������");
            magicCircle2.SetActive(true);
            mountain05_4.GetComponent<MeshCollider>(false);
        }
    }*/


    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag == "BossCheckPoint")
        {
            Debug.Log("���ζ� ���");
            magicCircle2.SetActive(true);
        }*/

        if (other.gameObject.tag == "BossMap")
        {
            Debug.Log("������ �̵�");
            BossScene();
        }
    }



    public void BossScene()
    {
        SceneManager.LoadScene("LYS_BossMap");
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         Debug.Log("������ �̵�");
     }*/

}
