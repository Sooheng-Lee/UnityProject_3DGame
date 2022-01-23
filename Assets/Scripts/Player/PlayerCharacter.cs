using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private float alpha = 1f;
    static private int coin = 0;
    private int coinMax = 100000;
    [SerializeField] AudioClip RecoveryClip;
    [SerializeField] AudioClip AcquireClip;
    private void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        m_Controller = GetComponent<CharacterController>();
        m_Renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    void Start()
    {
        SetCharacterInfo("Knight", 50, 50, 15, 5);
        InGameUI.Instance.playerName.text = m_Info.name;
        InGameUI.Instance.hpBar.value = (float)m_Info.HP / m_Info.HPMax;
        InGameUI.Instance.mpBar.value = (float)m_Info.MP / m_Info.MPMax;
        InGameUI.Instance.coin.text = coin.ToString();
    }

    private void Update()
    {
    }

    public int UseMP(int value)
    {
        float notUsedMP = m_Info.MP;
        int currentMP = m_Info.MP;
        currentMP -= value;
        if (currentMP >= 0)
        {
            m_Info.MP = currentMP;
            StartCoroutine(MpDecreaseRoutine(notUsedMP));
        }
        return currentMP;
    }

    protected override void TakeDamage(Transform damageLoc, int damageMin, int damageMax)
    {
        float notDamagedHp = m_Info.HP;
        base.TakeDamage(damageLoc, damageMin, damageMax);
        StartCoroutine(HpDecreaseRoutine(notDamagedHp));
    }

    private IEnumerator HpDecreaseRoutine(float hp)
    {
        while (m_State==CharacterState.Damaged)
        {
            hp = Mathf.Lerp(hp, (float)m_Info.HP, 0.05f);
            InGameUI.Instance.hpBar.value = hp / m_Info.HPMax ;
            yield return null;
        }
        InGameUI.Instance.hpBar.value = (float)m_Info.HP / m_Info.HPMax;
    }

    private IEnumerator MpDecreaseRoutine(float mp)
    {
        while (m_State == CharacterState.Skill)
        {
            mp = Mathf.Lerp(mp, (float)m_Info.MP, 0.05f);
            InGameUI.Instance.mpBar.value = (float)mp / m_Info.MPMax;
            yield return null;
        }
        InGameUI.Instance.mpBar.value = (float)m_Info.MP / m_Info.MPMax;
    }

    private void GetRecoveryCapsule(ref RecoveryCapsule.CapsuleType type, ref int value)
    {
        int count = 0;
        switch(type)
        {
            case RecoveryCapsule.CapsuleType.HP:
                while(value > 0 && m_Info.HP < m_Info.HPMax)
                {
                    m_Info.HP ++;
                    value--;
                    count++;
                    InGameUI.Instance.hpBar.value = (float)m_Info.HP / m_Info.HPMax;
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
                    m_Info.MP ++;
                    value--;
                    count++;
                    InGameUI.Instance.mpBar.value = (float)m_Info.MP / m_Info.MPMax;
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
            InGameUI.Instance.OpenDeadScreen();
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
        else if(other.tag=="Coin")
        {
            Item itemCoin = other.GetComponent<Item>();
            bool isEarn = AcquireCoin(itemCoin.value);
            if(isEarn)
            {
                m_Audio.PlayOneShot(AcquireClip);
                Destroy(other.gameObject);
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

    private bool AcquireCoin(int value)
    {
        if(coin + value <= coinMax)
        {
            coin += value;
            InGameUI.Instance.coin.text = coin.ToString();
            return true;
        }
        return false;
    }


}
