using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image EmptyImg;
    [SerializeField] private Image FullImg;

    public int ID
    {
        get => id;
        set => id = value;
    }
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
}
