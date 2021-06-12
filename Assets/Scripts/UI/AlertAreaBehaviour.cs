using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertAreaBehaviour : MonoBehaviour
{
    public Text alertsText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateAlerts(List<string> alerts)
    {
        alertsText.text = string.Join("\n\n", alerts);
    }
}
