using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController instance;
    [SerializeField] CanvasScript canvas;
    [SerializeField] GameObject player;
    [SerializeField] AudioSource audioSource;

    int score;
    bool paused;
    bool gameOver;

    public static GameController Instance { get => instance; }
    public CanvasScript Canvas { get => canvas; }
    public bool Paused { get => paused; }
    public GameObject Player { get => player; }
    public AudioSource AudioSource { get => audioSource; }
    public int Score { get => score; set => score = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(RemoveHpPlayer), 5, 0.5f);
    }

    void RemoveHpPlayer()
    {
        player.GetComponent<Hp>().RemoveHpGame(1);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            canvas.PauseGame();
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }

    void SaveGame()
    {
        if (score > PlayerPrefs.GetInt("Score1"))
        {
            PlayerPrefs.SetInt("Score1", score);
        }
        else if (score > PlayerPrefs.GetInt("Score2"))
        {
            PlayerPrefs.SetInt("Score2", score);
        }
        else if (score > PlayerPrefs.GetInt("Score3"))
        {
            PlayerPrefs.SetInt("Score2", score);
        }
    }

    public void GoMenu()
    {
        SaveGame();
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
