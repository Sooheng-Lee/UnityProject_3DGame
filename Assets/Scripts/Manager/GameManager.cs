using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum LocationType
    {
        Game
    }

    [SerializeField] public LocationType m_Type;
    [SerializeField] BoxCollider gate;
    [SerializeField] private GameObject NPC;
    private static GameManager m_Instance;
    private int monsterCount = 10;

    //Status Components
    private int MaxValue = 10;

    public int Attack = 0;
    public int Defense = 0;
    public int Hp = 0;
    public int Mp = 0;

    public static int AttackValue = 0;
    public static int DefenseValue = 0;
    public static int HpValue = 0;
    public static int MpValue = 0;
    public static int AttackPrice = 500;
    public static int DefensePrice = 500;
    public static int HpPrice = 500;
    public static int MpPrice = 500;

    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<GameManager>();
            return m_Instance;
        }
    }
    public List<GameObject> currentSpawned;
    private void Start()
    {
        gate.enabled = false;
        NPC.SetActive(false);
        switch (m_Type)
        {
            case LocationType.Game:
                InGameUI.Instance.EnemyCount.text = "Enemy X " + monsterCount.ToString();
                for (int index = 0; index < monsterCount / 2; index++)
                {
                    SpawnManager.Instance.SpawnEnemy(SpawnManager.EnemyType.Enemy_Slime, 0, 0);
                    SpawnManager.Instance.SpawnEnemy(SpawnManager.EnemyType.Enemy_Shell, 1, 1);
                }
                break;
        }
    }

    public void IncreaseStatus(string statusName)
    {

        if (statusName == "HP")
        {
            if (PlayerCharacter.coin >= HpPrice)
            {
                PlayerCharacter.coin -= HpPrice;
                HpValue += 1;
                HpPrice += 500;
            }
            
        }
        else if (statusName == "MP")
        {
            if (PlayerCharacter.coin >= MpPrice)
            {
                PlayerCharacter.coin -= MpPrice;
                MpValue += 1;
                MpPrice += 500;
            }
            
        }
        else if (statusName == "Attack")
        {
            if (PlayerCharacter.coin >= AttackPrice)
            {
                PlayerCharacter.coin -= AttackPrice;
                AttackValue += 1;
                AttackPrice += 500;
            }
        }
        else if (statusName == "Defense")
        {
            if (PlayerCharacter.coin >= DefensePrice)
            {
                PlayerCharacter.coin -= DefensePrice;
                DefenseValue += 1;
                DefensePrice += 500;
            }
        }
        
        if (HpValue >= MaxValue)
        {
            InGameUI.Instance.HpButton.enabled = false;
        }
        
        if (MpValue >= MaxValue)
        {
            InGameUI.Instance.MpButton.enabled = false;
        }
        if (AttackValue >= MaxValue)
        {
            InGameUI.Instance.AttackButton.enabled = false;
        }
        if (DefenseValue >= MaxValue)
        {
            InGameUI.Instance.DefenseButton.enabled = false;
        }
        InGameUI.Instance.AttackText.text = "Attack " + AttackValue.ToString();
        InGameUI.Instance.DefenseText.text = "Defense " + DefenseValue.ToString();
        InGameUI.Instance.HPText.text = "HP " + HpValue.ToString();
        InGameUI.Instance.MPText.text = "MP " + MpValue.ToString();
        
        InGameUI.Instance.AttackPriceText.text = AttackPrice.ToString();
        InGameUI.Instance.DefensePriceText.text = DefensePrice.ToString();
        InGameUI.Instance.HPPriceText.text = HpPrice.ToString();
        InGameUI.Instance.MPPriceText.text = MpPrice.ToString();
        InGameUI.Instance.coin.text = PlayerCharacter.coin.ToString();
    }

    public void IsStageEnd()
    {
        monsterCount--;
        InGameUI.Instance.EnemyCount.text = "Enemy X " + monsterCount.ToString();
        if (monsterCount <= 0)
        {
            gate.enabled = true;
            InGameUI.Instance.EnemyCount.text = "NPC에게 말을 걸거나 호수로 이동하세요...";
            NPC.SetActive(true);
        }
    }

    public void AllClearSpawned()
    {
        foreach (GameObject spawned in currentSpawned)
        {
            Destroy(spawned);
        }
    }

    private void OnApplicationQuit()
    {
        AllClearSpawned();
    }
}
