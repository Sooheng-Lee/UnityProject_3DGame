using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera follows target
    [SerializeField] Transform target;
    [SerializeField] Vector3 pivot;
    public bool posLock = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!posLock)
            transform.position = target.position + pivot;
    }

    public void ShakeEffect()
    {
        if (!posLock)
            StartCoroutine(EffectRoutine());
    }

    private IEnumerator EffectRoutine()
    {
        posLock = true;
        yield return new WaitForSecondsRealtime(0.01f);
        float randomPos = Random.Range(0.01f, 0.11f);
        transform.position = transform.position + new Vector3(randomPos, randomPos, 0);
        Time.timeScale = 0.8f;
        yield return new WaitForSecondsRealtime(0.02f);
        Time.timeScale = 1f;
        posLock = false;
    }
}
