using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera follows target
    [SerializeField] Transform target;
    [SerializeField] Vector3 pivot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + pivot;
    }
}
