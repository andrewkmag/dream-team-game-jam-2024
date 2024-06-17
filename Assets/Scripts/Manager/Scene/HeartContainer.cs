using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private Image EmptyImg;
    [SerializeField] private Image FullImg;

    #endregion

    #region Properties

    public int ID
    {
        get => id;
        set => id = value;
    }

    #endregion

    #region UnityMethods

    private void Reset()
    {
        var images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            if (FullImg == null && EmptyImg != null)
            {
                FullImg = image;
            }

            if (EmptyImg == null)
            {
                EmptyImg = image;
            }
        }
    }

    #endregion

    #region Methods

    public void Damaged()
    {
        EmptyImg.enabled = true;
        FullImg.enabled = false;
    }

    public void Healed()
    {
        FullImg.enabled = true;
        EmptyImg.enabled = false;
    }

    #endregion
}