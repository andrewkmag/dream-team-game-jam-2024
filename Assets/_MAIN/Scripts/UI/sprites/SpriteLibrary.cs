using UnityEngine;

public class SpriteLibrary : GenericLibrary<Sprite>
{
    [SerializeField] private SpriteLibScriptableObject spriteLib;


    protected override void getGenericScriptableObject()
    {
        genericLib = spriteLib;
    }
}