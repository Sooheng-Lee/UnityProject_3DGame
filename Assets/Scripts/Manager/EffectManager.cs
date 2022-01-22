using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject HP_Recovery_Effect;
    public GameObject MP_Recovery_Effect;
    public GameObject HitEffect;
    private static EffectManager m_Instance = null;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    // 0 : HP, 1 : MP
    public void PlayRecoveryEffect(Transform parentTrans, int recoveryType)
    {
        GameObject effect = null;
        switch(recoveryType)
        {
            case 0:
                effect = Instantiate(HP_Recovery_Effect, parentTrans.position, Quaternion.identity);
                break;
            case 1:
                effect = Instantiate(MP_Recovery_Effect, parentTrans.position, Quaternion.identity);
                break;
            default:
                return;
        }
        effect.transform.parent = parentTrans;
        Destroy(effect, 0.9f);
    }

    public void PlayHitEffect(Transform parentTrans)
    {
        Vector3 pos = parentTrans.position + new Vector3(0f, 0.7f, 1f);
        GameObject effect = Instantiate(HitEffect, pos, Quaternion.identity);
        effect.transform.parent = parentTrans;
        Destroy(effect, 0.8f);
    }
}
