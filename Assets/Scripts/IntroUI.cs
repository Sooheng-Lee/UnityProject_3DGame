using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroUI : MonoBehaviour
{
    private enum IntroState
    {
        Wait,
        Guide,
        GameStart
    }
    private IntroState state;
    public Text waitText;

    private bool isGuideText=false;
    public GameObject Panel;
    public Text guideText;

    public RawImage background;
    public Text gameName;
    
    bool bDown = true;
    private float alpha = 1f;
    // Start is called before the first frame update
    void Start()
    {
        state = IntroState.Wait;
        guideText.enabled = false;
        Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case IntroState.Wait:
                WaitScreen();
                break;
            case IntroState.Guide:
                GuideScreen();
                break;
            case IntroState.GameStart:
                GameStartScreen();
                break;
        }
    }

    void WaitScreen()
    {
        if (bDown)
        {
            alpha -= Time.deltaTime;
            if(alpha <= 0f)
            {
                bDown = false;
                alpha = 0f;
            }
        }
        else
        {
            alpha += Time.deltaTime;
            if(alpha >= 1f)
            {
                bDown = true;
                alpha = 1f;
            }
        }
        waitText.color = new Color(1, 1, 1, alpha);
        if(Input.anyKeyDown)
        {
            state = IntroState.Guide;
            waitText.enabled = false;
            Panel.SetActive(true);
            Invoke("ShowGuideText", 1.5f);
        }
    }

    void GuideScreen()
    {
        if (bDown)
        {
            alpha -= Time.deltaTime;
            if (alpha <= 0f)
            {
                bDown = false;
                alpha = 0f;
            }
        }
        else
        {
            alpha += Time.deltaTime;
            if (alpha >= 1f)
            {
                bDown = true;
                alpha = 1f;
            }
        }
        guideText.color = new Color(1f, 1f, 1f, alpha);
        if(Input.anyKeyDown && isGuideText)
        {
            alpha = 1f;
            state = IntroState.GameStart;
            Panel.SetActive(false);
        }
    }

    private void ShowGuideText()
    {
        alpha = 1f;
        guideText.enabled = true;
        isGuideText = true;
    }

    private void GameStartScreen()
    {
        if (alpha > 0f)
            alpha -= Time.deltaTime;
        else
            SceneManager.LoadScene("Main");
        background.color = new Color(alpha, alpha, alpha);
        gameName.color = new Color(alpha, alpha, alpha);

    }

}
