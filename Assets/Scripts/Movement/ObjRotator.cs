using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjRotator : MonoBehaviour
{
    [SerializeField] private Vector3 Vector3m_axis;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3m_axis * speed * Time.deltaTime);   
    }
}
