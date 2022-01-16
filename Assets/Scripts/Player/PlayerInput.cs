using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Character Movement Keys
    [HideInInspector] public float h_Axis;
    [HideInInspector] public float v_Axis;

    void Update()
    {
        MovementKey();
    }

    private void MovementKey()
    {
        h_Axis = Input.GetAxisRaw("Horizontal");
        v_Axis = Input.GetAxisRaw("Vertical");
    }
}
