using UnityEngine;
using UnityEngine.Pool;

public class MapObjectManager : MonoBehaviour
{
    public static MapObjectManager instance;

    public int minTreePoolSize = 20;
    public int maxTreePoolSize = 50;
    public GameObject TestTree;

    Vector3 treePoolPos;


    public IObjectPool<GameObject> TreePool { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CreateTree();
    }

    // ���� ������Ʈ�� ������Ʈ Ǯ�� �����ϱ� ���� �ڵ�
    private void CreateTree()
    {
        TreePool = new ObjectPool<GameObject>(CreateTreePool, UseTreePool, ReturnTreePool, DestroyTreePool, true, minTreePoolSize, maxTreePoolSize);

        for (int i = 0; i < minTreePoolSize; i++)
        {
            NatureTree natureTree = CreateTreePool().GetComponent<NatureTree>();
            /*natureTree.TreePool.Release(natureTree.gameObject);*/
        }
    }

    // ���� ������Ʈ ����
    private GameObject CreateTreePool()
    // ����Ƽ������ �����Ǵ� GameObject �ڷ����� ����. ����,����,��ȯ ���� ������ ó����.
    {
        CameraHandler cameraHandler = GetComponent<CameraHandler>();

        if (cameraHandler != null)
        {
            treePoolPos = transform.position + new Vector3(cameraHandler.targetTransform.position.x + 3f, 0, cameraHandler.targetTransform.position.z + 3f);
        }
        else
        {
            Debug.Log("null�Ӵ�");
        }

        GameObject treePool = Instantiate(TestTree, treePoolPos, Quaternion.identity);
        /*treePool.GetComponent<NatureTree>().TreePool = TreePool;*/
        return treePool;

    }


    // ���� ������Ʈ ���̵��� ����
    private void UseTreePool(GameObject treePool)
    {
        treePool.SetActive(true);
    }


    // ���� ������Ʈ �Ⱥ��̰� ���� (������ ���ؼ�)
    private void ReturnTreePool(GameObject treePool)
    {
        treePool.SetActive(false);
    }


    // ���� ������Ʈ ����
    private void DestroyTreePool(GameObject treePool)
    {
        Destroy(treePool);
    }


}
