using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button resume;
    [SerializeField] private Button exit;
    [SerializeField] private Toggle arc;

    [Header("Components")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        arc.isOn = PlayerInteractions.showArc;
        resume.onClick.AddListener(Resume);
        exit.onClick.AddListener(Exit);
        arc.onValueChanged.AddListener(setArc);
    }

    void Resume()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    void Exit()
    {
        Destroy(player);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    
    void setArc(bool val)
    {
        PlayerInteractions.showArc = val;
    }
}
