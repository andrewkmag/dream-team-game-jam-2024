using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericLibScriptableObject", menuName = "ScriptableObjects/GenericLib")]
public class GenericLibScriptableObject <T> : ScriptableObject
{
    #region Fields

    [SerializeField] private GenericLibElemClass<T>[] GenericLib;

    #endregion

    #region Methods

    public int GetSize()
    {
        return GenericLib.Length;
    }

    public string[] GetStringArr()
    {
        var spritenames = GenericLib
            .Select(spritenames => spritenames.name)
            .ToArray();
        return spritenames;
    }

    public T[] GetGenericArr()
    {
        var genericItem = GenericLib
            .Select(genericLibElemClass => genericLibElemClass.item)
            .ToArray();
        return genericItem;
    }

    #endregion
}

