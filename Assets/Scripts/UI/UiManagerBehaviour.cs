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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerBehaviour>();
        alertArea = transform.Find("Alerts Panel").gameObject.GetComponent<AlertAreaBehaviour>();
        healthChain = transform.Find("HealthChain Panel").gameObject.GetComponent<HealthChainBehaviour>();
        minimap = transform.Find("Minimap Panel").gameObject.GetComponent<MinimapBehaviour>();
        timeScore = transform.Find("TimeScore Panel").gameObject.GetComponent<TimeScoreBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        var player = gameManager.GetPlayer();

        alertArea.UpdateAlerts(gameManager.GetAlerts());
        healthChain.UpdateHealthChain(player.GetHealth(), player.GetChain().Count - 1);
        minimap.UpdateMinimap(gameManager.GetMinimapData());
        timeScore.UpdateText(gameManager.GetRemainingTime(), gameManager.GetScore());
    }
}
