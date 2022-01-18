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
    
    public int UseMP(int value)
    {
        int currentMP = m_Info.MP;
        currentMP -= value;
        if (currentMP >= 0)
            m_Info.MP = currentMP;
        return currentMP;
    }

    private void GetRecoveryCapsule(ref RecoveryCapsule.CapsuleType type, ref int value)
    {
        int count = 0;
        switch(type)
        {
            case RecoveryCapsule.CapsuleType.HP:
                while(value > 0 && m_Info.HP < m_Info.HPMax)
                {
                    m_Info.HP += value;
                    value--;
                    count++;
                }
                if (count != 0)
                    EffectManager.Instance.PlayRecoveryEffect(transform, 0);
                break;

            case RecoveryCapsule.CapsuleType.MP:
                while (value > 0 && m_Info.MP < m_Info.MPMax)
                {
                    m_Info.MP += value;
                    value--;
                    count++;
                }
                if (count != 0)
                    EffectManager.Instance.PlayRecoveryEffect(transform, 1);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // HP, MP Capsule�� �̿��Ͽ� ȸ���� ���
        if(other.tag=="RecoveryCapsule")
        {
            Debug.Log("RecoveryCapsule");
            RecoveryCapsule capsule = other.GetComponent<RecoveryCapsule>();
            if(capsule!=null)
            {
                int currentVal = capsule.m_Value;
                GetRecoveryCapsule(ref capsule.m_Type, ref capsule.m_Value);
                if (currentVal != capsule.m_Value)
                    Destroy(other.gameObject);
            }
        }
    }
}
