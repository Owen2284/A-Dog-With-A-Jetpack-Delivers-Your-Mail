using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScoreBehaviour : MonoBehaviour
{
    public Text timeText;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateText(float time, int score)
    {
        timeText.text = Mathf.Floor(time).ToString();
        scoreText.text = $"Score: {score}";
    }

    public void DisplayGameOverText()
    {
        timeText.text = "GAME OVER";
        scoreText.text = "";
    }
}
