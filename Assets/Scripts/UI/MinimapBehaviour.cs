using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapBehaviour : MonoBehaviour
{
    public RectTransform playerIcon;
    public RectTransform homeIcon;
    public RectTransform mailboxIcon;
    public RectTransform cameraBoundsRectangle;

    public float panelWidth = 260;
    public float panelHeight = 130;

    private List<RectTransform> mailboxIcons;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateMinimap(MinimapData data)
    {
        // First time only, create mailbox icons, and position home icon
        if (mailboxIcons == null)
        {
            mailboxIcons = new List<RectTransform>();

            foreach (var (mailboxPosition, maiboxColor) in data.MailboxLocations)
            {
                var newMailboxIcon = Instantiate(mailboxIcon, new Vector2(0, 0), Quaternion.identity, this.gameObject.transform);
                newMailboxIcon.anchoredPosition = ScalePosition(mailboxPosition);
                newMailboxIcon.GetComponent<Image>().color = maiboxColor;
                mailboxIcons.Add(newMailboxIcon);
            }

            Destroy(mailboxIcon.gameObject);

            homeIcon.anchoredPosition = ScalePosition(data.HomeLocation);
        }

        // Update player and camera bounds size
        playerIcon.anchoredPosition = ScalePosition(data.PlayerLocation);
        cameraBoundsRectangle.offsetMax = data.CameraOffsetMax;
        cameraBoundsRectangle.offsetMin = data.CameraOffsetMin;

        // Highlight mailboxes if player is carrying a delivery for it
        foreach (var mi in mailboxIcons)
        {
            var mailboxColor = mi.GetComponent<Image>().color;

            if (data.MaiboxesToHighlight.Contains(mailboxColor))
            {
                mi.sizeDelta = new Vector2(15, 15);
            }
            else
            {
                mi.sizeDelta = new Vector2(5, 5);
            }
        }
    }

    public Vector2 ScalePosition(Vector2 position)
    {
        return new Vector2(panelWidth * position.x, panelHeight * position.y);
    }
}
