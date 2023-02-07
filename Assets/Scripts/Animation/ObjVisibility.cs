using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjVisibility : MonoBehaviour
{
    [SerializeField] private bool visibility;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.SetActive(visibility); // false to hide, true to show        
    }
}
