using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericLibrary<T> : MonoBehaviour
{
    #region Fields

    [SerializeField] private GenericLibScriptableObject<T> GenericLib;
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
            if (GenericLib == null) return;
            var names = GenericLib.GetStringArr();
            var items = GenericLib.GetGenericArr();
            for (var index = FIRST_ARRAY; index < GenericLib.GetSize(); index++)
            {
                _GenericDic.Add(names[index], items[index]);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Methods
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