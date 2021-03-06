using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject creditsPanel;
    public AudioSource clickSound;
    public Text randomMessageText;
    public List<string> messages;

    // Start is called before the first frame update
    void Start()
    {
        playPanel.SetActive(false);
        creditsPanel.SetActive(false);

        var selectedMessage = messages[Random.Range(0, messages.Count)];
        randomMessageText.text = $"({selectedMessage})";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickPlay()
    {
        clickSound.Play();
        playPanel.SetActive(true);
    }

    public void ClickPlayBack()
    {
        clickSound.Play();
        playPanel.SetActive(false);
    }

    public void ClickGo()
    {
        clickSound.Play();
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ClickCredits()
    {
        clickSound.Play();
        creditsPanel.SetActive(true);
    }

    public void ClickCreditsBack()
    {
        clickSound.Play();
        creditsPanel.SetActive(false);
    }
}
