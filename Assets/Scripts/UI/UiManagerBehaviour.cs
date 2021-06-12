using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerBehaviour : MonoBehaviour
{
    private GameManagerBehaviour gameManager;
    private AlertAreaBehaviour alertArea;
    private HealthBarBehaviour healthBar;
    private MinimapBehaviour minimap;
    private TimeScoreBehaviour timeScore;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerBehaviour>();
        alertArea = transform.Find("Alert Panel").gameObject.GetComponent<AlertAreaBehaviour>();
        healthBar = transform.Find("Health Panel").gameObject.GetComponent<HealthBarBehaviour>();
        minimap = transform.Find("Minimap Panel").gameObject.GetComponent<MinimapBehaviour>();
        timeScore = transform.Find("TimeScore Panel").gameObject.GetComponent<TimeScoreBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        timeScore.UpdateText(gameManager.GetRemainingTime(), gameManager.GetScore());
    }
}
