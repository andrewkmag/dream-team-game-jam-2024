using System;
using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifespan = 5f;

    private const string PLAYER_TAG="Player";
    #region Events

    public delegate void HitAction();

    public static event HitAction OnPlayerHit;

    #endregion

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
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
        if (other.CompareTag(PLAYER_TAG))
        {
            OnPlayerHit?.Invoke();
        }
        gameObject.SetActive(false);
    }
}