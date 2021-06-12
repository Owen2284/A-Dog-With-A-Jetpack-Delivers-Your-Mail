using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapBehaviour : MonoBehaviour
{
    public Image playerIcon;

    private float panelWidth;
    private float panelHeight;

    private bool testDone = false;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = transform.GetComponent<RectTransform>();
        panelWidth = rt.sizeDelta.x * rt.localScale.x;
        panelHeight = rt.sizeDelta.y * rt.localScale.y;
    }

    public void UpdateMinimap(MinimapData data)
    {
        var playerIconRectTransform = playerIcon.GetComponent<RectTransform>();
        playerIconRectTransform.SetParent(this.gameObject.transform);
        Debug.Log(playerIconRectTransform.position);
    }

    public Vector2 ScalePosition(Vector2 position)
    {
        Debug.Log(position);
        Debug.Log(position * new Vector2(panelWidth / 2, panelHeight / 2));

        return position * new Vector2(panelWidth / 2, panelHeight / 2);
    }
}
