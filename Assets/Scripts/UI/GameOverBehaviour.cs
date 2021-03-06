using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverBehaviour : MonoBehaviour
{
    public Text deliveriesText;
    public Text scoreText;

    public AudioSource clickSound;
    public AudioSource gameOverSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateText(int deliveries, int score)
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            gameOverSound.Play();
        }

        deliveriesText.text = deliveries.ToString();
        scoreText.text = score.ToString();
    }

    public void ReplayClicked()
    {
        clickSound.Play();
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void MainMenuClicked()
    {
        clickSound.Play();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
