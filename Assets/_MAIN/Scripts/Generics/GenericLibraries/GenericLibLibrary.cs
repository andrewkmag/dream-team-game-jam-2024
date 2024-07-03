using System.Collections.Generic;
using UnityEngine;

public abstract class GenericLibrary<T> : MonoBehaviour
{
    #region Fields

    protected GenericLibScriptableObject<T> genericLib;
    private readonly Dictionary<string, T> _genericDic = new Dictionary<string, T>();

    #endregion

    #region Properties

    public static GenericLibrary<T> Instance { get; private set; }

    #endregion

    #region Constants

    private const int FIRST_ARRAY = 0;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            getGenericScriptableObject();
            if (genericLib == null) return;
            var names = genericLib.GetStringArr();
            var items = genericLib.GetGenericArr();
            
            for (var index = FIRST_ARRAY; index < genericLib.GetSize(); index++)
            {
                _genericDic.Add(names[index], items[index]);
            }

            getGenericScriptableObject();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Methods

    protected abstract void getGenericScriptableObject();

    public T GetGeneric(string categoty, string keyname)
    {
        var key = categoty + keyname;
        if (_genericDic.TryGetValue(key, out var value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("Key not found");
            return default;
        }
    }

    public T GetGeneric(string key)
    {
        if (_genericDic.TryGetValue(key, out var value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("Key not found");
            return default;
        }
    }
    #endregion
}