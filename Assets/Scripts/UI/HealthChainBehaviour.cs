using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthChainBehaviour : MonoBehaviour
{
    public List<Image> hearts;
    public Text chainText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateHealthChain(int health, int chainLength)
    {
        for (var i = 1; i <= hearts.Count; i++)
        {
            var heart = hearts[i - 1];

            heart.GetComponent<Image>().enabled = health >= i;
        }

        chainText.text = $"x {chainLength}";
    }
}
