using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryCapsule : MonoBehaviour
{
    public enum CapsuleType
    {
        HP,
        MP
    }

    // recovery value
    public int m_Value;
    // recovery type
    public CapsuleType m_Type;

    // up and down floating speed
    private bool IsDown = false;
    public float floatingSpeed;
    private float currSwitchTime = 0f;
    private float dirSwitchTime = 1.2f;

    void Update()
    {
        currSwitchTime += Time.deltaTime;
        if(currSwitchTime >= dirSwitchTime)
        {
            currSwitchTime = 0f;
            IsDown = !IsDown;
        }
        transform.position += new Vector3(0, IsDown?-1:1, 0) * floatingSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, 60f * Time.deltaTime);
    }
}
