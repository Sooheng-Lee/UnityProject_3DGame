using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum LocationType
    {
        Game
    }

    public struct AdditiveStruct
    {
        int HP;
        int MP;
        int Attack;
        int Defense;
    }

    [SerializeField] public LocationType m_Type;
    [SerializeField] BoxCollider gate;
    private static GameManager m_Instance;
    private int monsterCount = 10;
    
    
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

    public void IsStageEnd()
    {
        monsterCount--;
        InGameUI.Instance.EnemyCount.text = "Enemy X " + monsterCount.ToString();
        if (monsterCount <= 0)
        {
            gate.enabled = true;
            InGameUI.Instance.EnemyCount.text = "NPC에게 말을 걸거나 호수로 이동하세요...";
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
