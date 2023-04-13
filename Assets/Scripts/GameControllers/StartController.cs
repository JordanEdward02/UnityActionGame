using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button start;
    [SerializeField] Button exit;

    [Header("Scene")]
    [SerializeField] int startLevelIndex;

    [Header("Blackout")]
    [SerializeField] Image blackout;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        start.onClick.AddListener(StartGame);
        exit.onClick.AddListener(StopGame);
        PlayerInteractions.hasUsedObject = false;
        PlayerInteractions.hasUsedShield = false;
        PlayerInteractions.hasUsedMovement = false;
    }


    void StartGame()
    {
        CreditsController.startTime = Time.time;
        blackout.transform.SetSiblingIndex(4);
        StartCoroutine(FadeOut());
    }

    void StopGame()
    {
        Application.Quit();
    }

    public IEnumerator FadeOut()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            blackout.color = new Color(0, 0, 0, i);
            yield return null;
        }
        SceneManager.LoadScene(startLevelIndex);
    }
}
