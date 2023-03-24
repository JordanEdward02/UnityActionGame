using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGUI : MonoBehaviour
{
    [Header("TextFields")]
    [SerializeField] private TextMeshProUGUI toolTip;
    [SerializeField] private TextMeshProUGUI playerThoughts;
    [Header("Images")]
    [SerializeField] private Image blackout;
    [SerializeField] public Image collectible;

    // Update is called once per frame
    void Update()
    {
        // Checks if what the user is looking at has a tooltip to show on the player's UI.
        // Then will add this to their canvas or blank them out if not applicable.
        RaycastHit hit;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hit))
        {
            if (hit.distance < 4 && hit.collider.gameObject.TryGetComponent(out TooltipHolder tip))
            {
                if (tip.type == TooltipType.TOOLTIP)
                    toolTip.SetText(tip.tooltip);
                else
                    playerThoughts.SetText(tip.tooltip);
            }
            else
            {
                toolTip.SetText("");
                playerThoughts.SetText("");
            }
        }
    }

    public IEnumerator FadeOut(string newScene)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            blackout.color = new Color(0, 0, 0, i);
            yield return null;
        }
        collectible.enabled = false;
        SceneManager.LoadScene(newScene);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            blackout.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

}
