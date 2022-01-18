using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public enum PlayerState
    {
        Idle,
        Attack,
        Skill,
        Damaged,
        Death
    }

    public PlayerState m_State;

    void Start()
    {
        SetCharacterInfo("Knight", 300, 100, 15, 5);
    }
    void Update()
    {
        
    }
}
