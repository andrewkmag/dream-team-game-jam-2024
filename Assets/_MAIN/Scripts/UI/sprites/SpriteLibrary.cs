using UnityEngine;

public class SpriteLibrary : GenericLibrary<Sprite>
{
    #region Fields

    [SerializeField] private SpriteLibScriptableObject spriteLib;

    #endregion

    #region Methods

    protected override void GetGenericScriptableObject()
    {
        genericLib = spriteLib;
    }

    #endregion
}