using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] Image hpImage;
    [SerializeField] Image staminaImage;
    [SerializeField] Image hpSecondImage;
    [SerializeField] Image staminaSecondImage;

    [Header("Texts")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text scoreEndText;
    
    [Header("Panels")]
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOver;

    Hp playerHp;

    public GameObject StartPanel { get => startPanel; }
    public GameObject GameOver { get => gameOver; }

    void Start()
    {
        playerHp = GameController.Instance.Player.GetComponent<Hp>();
        hpImage.fillAmount = playerHp.CurrentHp / playerHp.MaxHp;
        staminaImage.fillAmount = playerHp.CurrentStamina / playerHp.MaxStamina;
    }

    void Update()
    {
        hpImage.fillAmount = playerHp.CurrentHp / playerHp.MaxHp;
        staminaImage.fillAmount = playerHp.CurrentStamina / playerHp.MaxStamina;
        scoreText.text = GameController.Instance.Score.ToString();
        scoreEndText.text = GameController.Instance.Score.ToString();
    }

    public void PauseGame()
    {
        GameController.Instance.PauseGame();

        if (GameController.Instance.Paused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
