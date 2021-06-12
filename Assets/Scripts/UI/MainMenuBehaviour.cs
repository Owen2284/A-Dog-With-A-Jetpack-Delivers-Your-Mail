using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject creditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        playPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickPlay()
    {
        playPanel.SetActive(true);
    }

    public void ClickPlayBack()
    {
        playPanel.SetActive(false);
    }

    public void ClickGo()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ClickCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void ClickCreditsBack()
    {
        creditsPanel.SetActive(false);
    }
}
