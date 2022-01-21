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
    public struct CharacterInfo
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
    protected SkinnedMeshRenderer[] m_Renderer;
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

    protected void TakeDamage(Transform damageLoc, int damageMin, int damageMax)
    {
        if (m_State == Character.CharacterState.Damaged)
            return;
        m_State = CharacterState.Damaged;
        Animator anim = gameObject.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("GetHit");
        }
        int totalDamage = Random.Range(damageMin, damageMax + 1) - Random.Range(m_Info.DefenseMin, m_Info.DefenseMax);
        totalDamage = (totalDamage > 0) ? totalDamage : 1;
        m_Info.HP -= totalDamage;
        m_Audio.PlayOneShot(HitClip);
        Vector3 reactVec = transform.position - damageLoc.position;
        reactVec.y = 0;
        StartCoroutine(OnDamaged(reactVec));
    }

    protected virtual void CheckDeath()
    {
        if(m_Info.HP <= 0)
        {
            m_State = CharacterState.Death;
            m_Controller.enabled = false;
        }
    }
    private IEnumerator OnDamaged(Vector3 Dir)
    {
        while(m_State==Character.CharacterState.Damaged)
        {
            m_Controller.Move(Dir * Time.deltaTime * 0.5f);
            yield return null;
        }
    }

    public CharacterInfo GetInfo()
    {
        return m_Info;
    }

    public void DamageEnd()
    {
        m_State = CharacterState.Idle;
    }
}
