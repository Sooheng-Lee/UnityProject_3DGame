using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Unity Components
    private CharacterController m_Controller;
    private PlayerInput m_Input;

    // Character Move Components
    private Vector3 moveVec;
    private float moveVelocityY = 0f;
    [SerializeField] private float moveSpeed = 5.7f;

    void Awake()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        moveVelocityY += Physics.gravity.y * Time.deltaTime;
        moveVec = new Vector3(m_Input.h_Axis, 0f, m_Input.v_Axis).normalized * moveSpeed;
        moveVec.y = moveVelocityY;
        m_Controller.Move(moveVec * Time.deltaTime);
        if (m_Controller.isGrounded)
            moveVelocityY = 0f;
    }
}
