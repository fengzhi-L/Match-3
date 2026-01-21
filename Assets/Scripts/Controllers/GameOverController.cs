using System;
using System.Collections;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour, IController
{
    [SerializeField] private GameObject screenParent;
    [SerializeField] private GameObject scoreParent;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image[] stars;
    private void Start()
    {
        screenParent.SetActive(false);

        foreach (var star in stars)
        {
            star.enabled = false;
        }

        this.RegisterEvent<GameWinEvent>(e => { ShowWin(e.Score, e.StarCount); });
        this.RegisterEvent<GameLoseEvent>(e => { ShowLose(); });
    }

    public void ShowLose()
    {
        screenParent.SetActive(true);
        scoreParent.SetActive(false);

        var anim = GetComponent<Animator>();

        if (anim)
        {
            anim.Play("GameOverShow");
        }
    }

    public void ShowWin(int score, int starCount)
    {
        screenParent.SetActive(true);
        loseText.enabled = false;

        scoreText.text = score.ToString();
        scoreText.enabled = false;
        
        var anim = GetComponent<Animator>();

        if (anim)
        {
            anim.Play("GameOverShow");
        }

        StartCoroutine(ShowWinCoroutine(starCount));
    }

    private IEnumerator ShowWinCoroutine(int starCount)
    {
        yield return new WaitForSeconds(0.5f);

        if (starCount < stars.Length)
        {
            for (var i = 0; i < starCount; i++)
            {
                stars[i].enabled = true;

                if (i > 0)
                {
                    stars[i - 1].enabled = false;
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        scoreText.enabled = true;
    }

    public void OnReplayClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnDoneClicked()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
