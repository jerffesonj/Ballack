using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MenuController : MonoBehaviour
{

    [SerializeField] GameObject creditos;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip start;
    [SerializeField] AudioClip click;


    [SerializeField] TMP_Text score1;
    [SerializeField] TMP_Text score2;
    [SerializeField] TMP_Text score3;

    private void Start()
    {
        GetScore();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Jogo");
        audio.PlayOneShot(start);
    }
    public void EntrarCreditos()
    {
        creditos.SetActive(true);
        audio.PlayOneShot(click);
    }
    public void SairCreditos()
    {
        creditos.SetActive(false);
        audio.PlayOneShot(click);
    }
    public void Quit()
    {
        audio.PlayOneShot(click);
        Application.Quit();
    }

    public void GoInsta()
    {
        audio.PlayOneShot(start);

        Application.OpenURL("https://www.instagram.com/blackpaladins/");
    }

    public void GetScore()
    {
        if(PlayerPrefs.GetInt("Score1")!= 0)
        {
            score1.text = PlayerPrefs.GetInt("Score1").ToString();
        }
        else
        {
            score1.text = "0";
        }

        if (PlayerPrefs.GetInt("Score2") != 0)
        {
            score2.text = PlayerPrefs.GetInt("Score2").ToString();
        }
        else
        {
            score2.text = "0";
        }

        if (PlayerPrefs.GetInt("Score3") != 0)
        {
            score3.text = PlayerPrefs.GetInt("Score3").ToString();
        }
        else
        {
            score3.text = "0";
        }
    }
}
