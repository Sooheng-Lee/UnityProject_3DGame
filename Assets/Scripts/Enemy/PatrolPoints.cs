using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    MeshRenderer renderer;
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;
    }

}
