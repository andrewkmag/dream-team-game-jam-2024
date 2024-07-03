using System;

[Serializable]
public class GenericLibElemClass <T>
{
    #region Fields
    
    public string name;
    public T item;

    #endregion

    #region Constructor

    public GenericLibElemClass(string name, T item)
    {
        this.name = name;
        this.item = item;
    }

    #endregion

}