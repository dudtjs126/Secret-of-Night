using UnityEngine;
using UnityObject = UnityEngine.Object;

/// <summary>
/// Resources.Load를 매핑한다
/// 나중에 어셋번들로 변경하기 위함
/// 공용으로 사용되는 함수이며 따로 데이터를 저장하는 매니저가 아니므로 스태틱을 사용
/// </summary>
public class ResourceManager
{
    public static UnityObject Load(string path)
    {
        // 추후 에셋 로드로 변경
        return Resources.Load(path);
    }
    public static T Load<T>(string path) where T : UnityObject
    {
        // 추후 에셋 로드로 변경
        return Resources.Load<T>(path);
    }
    public static GameObject LoadAndInstantiate(string path)
    {
        UnityObject source = Load(path);
        if (source == null)
        {
            return null;
        }
        return GameObject.Instantiate(source) as GameObject;
    }
}
