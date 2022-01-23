using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform model;
    public GameObject effectModel;
    public int value;

    void Update()
    {
        model.Rotate(Vector3.forward, 80f * Time.deltaTime);
    }
}
