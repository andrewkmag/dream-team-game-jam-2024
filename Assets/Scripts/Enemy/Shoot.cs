using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    private void Update()
    {
        transform.Translate(Vector3.forward *Time.deltaTime*speed);
    }
}
