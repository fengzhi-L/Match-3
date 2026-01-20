using System;
using QFramework;
using TMPro;
using UnityEngine;
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
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
