using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Slider hpBar;
    public Slider mpBar;
    public Text playerName;
    public Text coin;

    private static InGameUI m_Instance = null;
    public static InGameUI Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<InGameUI>();
            return m_Instance;
        }
    }
}
