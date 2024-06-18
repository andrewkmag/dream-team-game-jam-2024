using System;
using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifespan = 5f;
    private void Update()
    {
        transform.Translate(Vector3.forward *Time.deltaTime*speed);
        if (isActiveAndEnabled)
        {
            StartCoroutine(disableShot());
        }
    }

    IEnumerator disableShot()
    {
        yield return new WaitForSeconds(lifespan);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
