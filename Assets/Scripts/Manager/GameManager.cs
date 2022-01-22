using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        for (int index = 0; index < 5; index++)
        {
            SpawnManager.Instance.SpawnEnemy(SpawnManager.EnemyType.Enemy_Slime, 0, 0);
            SpawnManager.Instance.SpawnEnemy(SpawnManager.EnemyType.Enemy_Shell, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
