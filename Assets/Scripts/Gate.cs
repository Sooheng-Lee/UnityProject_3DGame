using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            switch (GameManager.Instance.m_Type)
            {
                case GameManager.LocationType.Game:
                    SceneManager.LoadScene(SceneManager.sceneCount);
                    break;
            }
            
        }
    }
}
