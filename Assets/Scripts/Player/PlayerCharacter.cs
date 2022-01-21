using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private float alpha = 1f;
    [SerializeField] AudioClip RecoveryClip;
    private void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        m_Controller = GetComponent<CharacterController>();
        m_Renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    void Start()
    {
        SetCharacterInfo("Knight", 10, 100, 15, 5);
    }

    private void Update()
    {
        CheckDeath();
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
                {
                    m_Audio.PlayOneShot(RecoveryClip, 0.2f);
                    EffectManager.Instance.PlayRecoveryEffect(transform, 0);
                }
                break;

            case RecoveryCapsule.CapsuleType.MP:
                while (value > 0 && m_Info.MP < m_Info.MPMax)
                {
                    m_Info.MP += value;
                    value--;
                    count++;
                }
                if (count != 0)
                {
                    m_Audio.PlayOneShot(RecoveryClip, 0.2f);
                    EffectManager.Instance.PlayRecoveryEffect(transform, 1);
                }
                break;
        }
    }

    protected override void CheckDeath()
    {
        base.CheckDeath();
        if(m_State==CharacterState.Death)
        {
            StartCoroutine(PlayDeadState());
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // HP, MP Capsule을 이용하여 회복한 경우
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
        // 스킬 사용 혹은 데미지를 입었을 경우 일정시간 무적판정
        else if(other.tag=="EnemyAttack" && m_State!=CharacterState.Damaged && m_State!=CharacterState.Skill)
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if(enemy!=null)
            {
                Debug.Log("Damaged");
                TakeDamage(enemy.transform, enemy.GetInfo().AttackMin, enemy.GetInfo().AttackMax);
            }
        }
    }

    public void DamageEnd()
    {
        m_State = CharacterState.Idle;
    }

    // 플레이어 사망에 대한 애니메이션이 없어서 바닥쪽으로 쓰러지는 과정으로 대체하였습니다.
    private IEnumerator PlayDeadState()
    {
        float angle = 0f;
        while(angle < 90f)
        {
            angle += 1f;
            transform.rotation = Quaternion.Euler(new Vector3(angle, transform.eulerAngles.y, 0f));
            yield return null;
        }
        Animator anim = GetComponent<Animator>();
        anim.enabled = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
