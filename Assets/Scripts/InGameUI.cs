using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    // Player Status UI
    public Slider hpBar;
    public Slider mpBar;
    public Text playerName;
    public Text coin;

    // NPC Talk UI
    public GameObject NPC_Panel;
    public Text AttackText;
    public Text DefenseText;
    public Text HPText;
    public Text MPText;
    public Text AttackPriceText;
    public Text DefensePriceText;
    public Text HPPriceText;
    public Text MPPriceText;
    public Button AttackButton;
    public Button DefenseButton;
    public Button HpButton;
    public Button MpButton;
    

    public GameObject escMenu;

    // ETC UI
    public Image DeadScreen;
    public Text DeadText;
    public Text EnemyCount;
    private bool onMenuOpen = false;
    private bool onTalkWindowOpen = false;
    

    private void Start()
    {
        NPC_Panel.SetActive(false);
        escMenu.SetActive(false);
        DeadScreen.gameObject.SetActive(false);
        DeadText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (onTalkWindowOpen)
            {
                NPC_Panel.SetActive(false);
                onTalkWindowOpen = false;
            }
            else
            {
                onMenuOpen = !onMenuOpen;
                escMenu.SetActive(onMenuOpen);
                Time.timeScale = 0f;
            }
        }
    }
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

    public void ClickResumeButton()
    {
        onMenuOpen = false;
        escMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ClickRestartButton()
    {
        Time.timeScale = 1f;
        onMenuOpen = false;
        escMenu.SetActive(false);
        GameManager.Instance.AllClearSpawned();
        SceneManager.LoadScene(SceneManager.sceneCount, LoadSceneMode.Single);
    }

    public void ClickQuitButton()
    {
        GameManager.Instance.AllClearSpawned();
        Application.Quit();
    }

    public void OpenDeadScreen()
    {
        DeadScreen.gameObject.SetActive(true);
        DeadText.gameObject.SetActive(true);
        StartCoroutine(DeadRoutine());
    }

    private IEnumerator DeadRoutine()
    {
        float alpha = 0f;
        while(alpha < 0.5)
        {
            alpha += Time.deltaTime * 0.1f;
            DeadScreen.color = new Color(DeadScreen.color.r, DeadScreen.color.g, DeadScreen.color.b, alpha);
            DeadText.color = new Color(DeadText.color.r, DeadText.color.g, DeadText.color.b, alpha * 5);
            yield return null;
        }
    }

    public void OpenNPCTalkWindow()
    {
        NPC_Panel.SetActive(true);
        onTalkWindowOpen = true;
        
        AttackText.text = "Attack " + GameManager.AttackValue.ToString();
        DefenseText.text = "Defense " + GameManager.DefenseValue.ToString();
        HPText.text = "HP " + GameManager.HpValue.ToString();
        MPText.text = "MP " + GameManager.MpValue.ToString();

        AttackPriceText.text = GameManager.AttackPrice.ToString();
        DefensePriceText.text = GameManager.DefensePrice.ToString();
        HPPriceText.text = GameManager.HpPrice.ToString();
        MPPriceText.text = GameManager.MpPrice.ToString();   
    }

    public void CloseNPCTalkWindow()
    {
        NPC_Panel.SetActive(false);
        onTalkWindowOpen = false;
    }
}
