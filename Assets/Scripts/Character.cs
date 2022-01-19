using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Attack,
        Skill,
        Damaged,
        Death
    }
    protected struct CharacterInfo
    {
        public string name;
        public int HP;
        public int HPMax;
        public int MP;
        public int MPMax;
        public int AttackMin;
        public int AttackMax;
        public int DefenseMin;
        public int DefenseMax;
    }

    protected CharacterInfo m_Info;
    protected AudioSource m_Audio;
    protected CharacterController m_Controller;
    public CharacterState m_State;
    public AudioClip HitClip;
    protected void SetCharacterInfo(string name, int hp, int mp, int attackMin, int defenseMin)
    {
        m_Info.name = name;
        m_Info.HP = hp;
        m_Info.HPMax = hp;
        m_Info.MP = mp;
        m_Info.MPMax = mp;
        m_Info.AttackMin = attackMin;
        m_Info.AttackMax = attackMin + 10;
        m_Info.DefenseMin = defenseMin;
        m_Info.DefenseMax = defenseMin + 5;
    }
}
