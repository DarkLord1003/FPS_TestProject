using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Casing : MonoBehaviour
{
    [Header("Force X")]
    [SerializeField] private float _minForceX;
    [SerializeField] private float _maxForceX;

    [Header("Force Y")]
    [SerializeField] private float _minForceY;
    [SerializeField] private float _maxForceY;

    [Header("Force Z")]
    [SerializeField] private float _minForceZ;
    [SerializeField] private float _maxForceZ;

    [Header("Rotation Force")]
    [SerializeField] private float _minRotationForce;
    [SerializeField] private float _maxRotationForce;

    [Header("Time Destroy")]
    [SerializeField] private float _destroyAfter;

    [Header("Spin Speed")]
    [SerializeField] private float _spinSpeed;

    [Header("Audio")]
    [SerializeField] private AudioSource _casingAudioSource;

    private void Awake()
    {
        GetComponent<Rigidbody>().AddRelativeTorque(
        Random.Range(_minRotationForce, _maxRotationForce),
        Random.Range(_minRotationForce, _maxRotationForce),
        Random.Range(_minRotationForce, _maxRotationForce));

        GetComponent<Rigidbody>().AddRelativeForce(
        Random.Range(_minForceX, _maxForceX),
        Random.Range(_minForceY, _maxForceY),
        Random.Range(_minForceZ, _maxForceZ));
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.right * _spinSpeed * Time.deltaTime);
        transform.Rotate(Vector3.down * _spinSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        _casingAudioSource.Play();
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(_destroyAfter);
        gameObject.SetActive(false);
    }
}
