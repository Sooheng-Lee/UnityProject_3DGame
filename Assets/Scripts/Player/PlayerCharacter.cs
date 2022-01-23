using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private float alpha = 1f;
    public static int coin = 0;
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
        SetCharacterInfo("Knight", 50 + GameManager.HpValue * 50, 50 + GameManager.MpValue * 50, 15 + GameManager.AttackValue * 2, 5 + GameManager.DefenseValue * 2);
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
            StartCoroutine(MpDecreaseRoutine());
        }
        return currentMP;
    }

    protected override void TakeDamage(Transform damageLoc, int damageMin, int damageMax)
    {
        base.TakeDamage(damageLoc, damageMin, damageMax);
        StartCoroutine(HpDecreaseRoutine());
    }

    private IEnumerator HpDecreaseRoutine()
    {
        while (m_State==CharacterState.Damaged)
        {
            InGameUI.Instance.hpBar.value = Mathf.Lerp(InGameUI.Instance.hpBar.value, (float)m_Info.HP/m_Info.HPMax, 0.5f);
            yield return null;
        }
        InGameUI.Instance.hpBar.value = (float)m_Info.HP / m_Info.HPMax;
    }

    private IEnumerator MpDecreaseRoutine()
    {
        while (m_State == CharacterState.Skill)
        {
            InGameUI.Instance.mpBar.value = Mathf.Lerp(InGameUI.Instance.mpBar.value, (float)m_Info.MP/m_Info.MPMax, 0.5f);
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
        // ��ų ��� Ȥ�� �������� �Ծ��� ��� �����ð� ��������
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

    // �÷��̾� ����� ���� �ִϸ��̼��� ��� �ٴ������� �������� �������� ��ü�Ͽ����ϴ�.
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
