using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolComponent : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rigidbody;
    public Transform m_Transform => _transform;
    public Rigidbody m_Rigidbody => _rigidbody;
}
