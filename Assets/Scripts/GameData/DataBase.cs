using System.Collections.Generic;

/// <summary>
/// To Use it
/// First, initalize it than you can get data by using GetData Method
/// </summary>
/// <typeparam name="TKey">primary key</typeparam>
/// <typeparam name="TValue">data type</typeparam>

public abstract class DataBase<TKey, TValue>
{
    protected Dictionary<TKey, TValue> data = new Dictionary<TKey, TValue>();
    public void Initalize()
    {
        LoadData();
    }
    protected abstract void LoadData();
    public virtual TValue GetData(TKey key)
    {
        if (data.ContainsKey(key))
        {
            return data[key];
        }
        return default;
    }
}
