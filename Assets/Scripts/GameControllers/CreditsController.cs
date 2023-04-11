using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Look at adding a timer to this to see how long it took the player to finish the game. Display this time at the end.
// Alternatively, look at the addition of achievements => Jailbreak, Dragonslayer, Shield Alergy

public class CreditsController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image blackout;
    [SerializeField] Button continueButton;

    void Awake()
    {
        GameObject player = GameObject.Find("Player(Clone)");
        Destroy(player);
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(FadeIn());
        continueButton.onClick.AddListener(Continue);
    }


    IEnumerator FadeIn()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            blackout.color = new Color(0, 0, 0, i);
            yield return null;
        }
        yield return null;
    }
    void Continue()
    {
        SceneManager.LoadScene(0);
    }
}
