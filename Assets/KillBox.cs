using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    #region Events

    public delegate void Action();

    public static event Action OnDeath;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnDeath?.Invoke();
        }
    }
    #endregion
}