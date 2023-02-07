using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    [SerializeField] private Vector3 Vector3_gravity;

    void Update()
    {
        Physics.gravity = Vector3_gravity;
    }
}
