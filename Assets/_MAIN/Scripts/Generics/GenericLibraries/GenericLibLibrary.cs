using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GenericLibrary<T> : MonoBehaviour
{
    #region Fields

    protected GenericLibScriptableObject<T> genericLib;
    [SerializeField] private readonly Dictionary<string, T> _GenericDic = new Dictionary<string, T>();

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
                _GenericDic.Add(names[index], items[index]);
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
        if (_GenericDic.TryGetValue(key, out var value))
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
        if (_GenericDic.TryGetValue(key, out var value))
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