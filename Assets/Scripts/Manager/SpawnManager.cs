using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public enum EnemyType
    {
        Enemy_Slime,
        Enemy_Shell
    }

    //Enemy Spawn Component
    private int enemyCount = 0;
    public Enemy[] enemyCollection;
    public GameObject[] patrolPackage;
    public GameObject[] spawnPoints;

    public List<GameObject> currentSpawned;

    private static SpawnManager m_Instance = null;
    public static SpawnManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<SpawnManager>();
            return m_Instance;
        }
    }

    private void Start()
    {
        if(spawnPoints.Length > 0)
        {
            for(int index=0; index < spawnPoints.Length; index++)
            {
                MeshRenderer renderer = spawnPoints[index].GetComponent<MeshRenderer>();
                renderer.enabled = false;
            }
        }
    }

    public void SpawnEnemy(EnemyType type, int spawnNum, int patrolNum)
    {
        if(spawnNum < spawnPoints.Length && patrolNum < patrolPackage.Length)
        {
            GameObject enemy = null;
            Vector3 spawnPos = Random.insideUnitSphere * 5f + spawnPoints[spawnNum].transform.position;
            spawnPos.y = 7f;
            switch (type)
            {
                case EnemyType.Enemy_Slime:
                    enemy = Instantiate(enemyCollection[0].gameObject, spawnPos, Quaternion.identity);
                    break;
                case EnemyType.Enemy_Shell:
                    enemy = Instantiate(enemyCollection[1].gameObject, spawnPos, Quaternion.identity);
                    break;
            }
            currentSpawned.Add(enemy);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.patrolPackage = patrolPackage[patrolNum];

        }
    }

    public void AllClearSpawned()
    {
        foreach(GameObject spawned in currentSpawned)
        {
            Destroy(spawned);
        }
    }
}
