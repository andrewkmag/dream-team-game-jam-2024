using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float speed = 0.4f;
    private void Update()
    {
        transform.Translate(transform.forward*Time.deltaTime*speed);
    }
}
