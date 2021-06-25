using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerBehaviour : MonoBehaviour
{
    private GameManagerBehaviour gameManager;

    private AlertAreaBehaviour alertArea;
    private HealthChainBehaviour healthChain;
    private MinimapBehaviour minimap;
    private TimeScoreBehaviour timeScore;
    private GameOverBehaviour gameOver;
    private RectTransform pausedPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerBehaviour>();

        alertArea = transform.Find("Alerts Panel").gameObject.GetComponent<AlertAreaBehaviour>();
        healthChain = transform.Find("HealthChain Panel").gameObject.GetComponent<HealthChainBehaviour>();
        minimap = transform.Find("Minimap Panel").gameObject.GetComponent<MinimapBehaviour>();
        timeScore = transform.Find("TimeScore Panel").gameObject.GetComponent<TimeScoreBehaviour>();
        gameOver = transform.Find("GameOver Panel").gameObject.GetComponent<GameOverBehaviour>();
        pausedPanel = (RectTransform)transform.Find("Paused Panel");

        gameOver.gameObject.SetActive(false);
        pausedPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var player = gameManager.GetPlayer();
        var isGameOver = gameManager.IsGameOver();
        var isPaued = gameManager.IsPaused();

        // Update these parts of the UI in the same way no matter what
        alertArea.UpdateAlerts(gameManager.GetAlerts());
        healthChain.UpdateHealthChain(player.GetHealth(), player.GetChain().Count - 1);
        minimap.UpdateMinimap(gameManager.GetMinimapData());

        // Different behaviour based on game state
        if (!isGameOver)
        {
            timeScore.UpdateText(gameManager.GetRemainingTime(), gameManager.GetScore());
        }
        else
        {
            timeScore.DisplayGameOverText();
            gameOver.UpdateText(gameManager.GetDeliveryTotal(), gameManager.GetScore());
        }

        pausedPanel.gameObject.SetActive(isPaued);
    }
}
