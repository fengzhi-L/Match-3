using System;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour , IController
{
    [SerializeField] private TextMeshProUGUI remainingText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI targetScoreText;
    [SerializeField] private Image star0Image;
    [SerializeField] private Image star1Image;
    [SerializeField] private Image star2Image;
    [SerializeField] private Image star3Image;

    private void Start()
    {
        targetScoreText.text = this.GetModel<ILevelModel>().currentLevelData.targetScore.ToString();
        totalScoreText.text = "0";
        levelText.text = this.GetModel<ILevelModel>().currentLevelData.levelName;
        remainingText.text = this.GetModel<ILevelModel>().currentLevelData.movesLimit.ToString();
        star1Image.gameObject.SetActive(false);
        star2Image.gameObject.SetActive(false);
        star3Image.gameObject.SetActive(false);

        this.RegisterEvent<FruitMoveSuccessEvent>(e =>
        {
            if (int.TryParse(remainingText.text, out var moveleft))
            {
                remainingText.text = (moveleft - 1).ToString();
            }
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    public void UpdateScore(int score)
    {
        totalScoreText.text = score.ToString();
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
