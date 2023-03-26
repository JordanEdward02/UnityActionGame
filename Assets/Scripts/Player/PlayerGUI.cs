using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Include a bar indicating the power as you hold down throw. Do this within the PlayerInteractions class and do function calls to enable and progress the bar.

public class PlayerGUI : MonoBehaviour
{
    [Header("TextFields")]
    [SerializeField] private TextMeshProUGUI toolTip;
    [SerializeField] private TextMeshProUGUI playerThoughts;
    [SerializeField] private TextMeshProUGUI tutorial;
    [Header("Images")]
    [SerializeField] private Image blackout;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image damageBlur;
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
                switch (tip.type)
                {
                    case TooltipType.TOOLTIP:
                        toolTip.SetText(tip.tooltip);
                        break;
                    case TooltipType.THOUGHT:
                        playerThoughts.SetText(tip.tooltip);
                        break;
                    case TooltipType.TUTORIAL:
                        tutorial.SetText(tip.tooltip);
                        break;
                }
            }
            else
            {
                toolTip.SetText("");
                playerThoughts.SetText("");
                tutorial.SetText("");
            }
        }
    }

    public void SetPowerBar(float val)
    {
        progressBar.fillAmount = val;
    }

    public IEnumerator FadeOut(int newScene)
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

    IEnumerator FadeIn()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            blackout.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    IEnumerator Die()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            gameObject.transform.Rotate(new Vector3(0,0,-1f));
            blackout.color = new Color(0, 0, 0, i);
            yield return null;
        }
        Scene currentScene = SceneManager.GetActiveScene();
        Destroy(gameObject);
        SceneManager.LoadScene(currentScene.name);
    }

    public void Damage()
    {
        if (damageBlur.enabled)
        {
            StartCoroutine(Die());
        }
        damageBlur.enabled = true;
        StartCoroutine(DamageFade());
    }

    IEnumerator DamageFade()
    {
        yield return new WaitForSeconds(3);
        damageBlur.enabled = false;
    }
}
