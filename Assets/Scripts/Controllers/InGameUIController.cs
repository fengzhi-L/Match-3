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
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private Image[] stars;
    private LevelData _levelData;
    private int _starCount;

    private void Start()
    {
        _levelData = this.GetModel<ILevelModel>().currentLevelData;
        targetScoreText.text = _levelData.targetScore.ToString();
        totalScoreText.text = "0";
        levelText.text = _levelData.levelName;
        remainingText.text = _levelData.movesLimit.ToString();
        
        // 初始只显示0星
        for (var i = 1; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }

        this.RegisterEvent<FruitMoveSuccessEvent>(e =>
        {
            if (int.TryParse(remainingText.text, out var moveleft))
            {
                remainingText.text = (moveleft - 1).ToString();

                if (moveleft - 1 != 0) return;

                if (this.GetModel<IUserModel>().currentScore.Value >= _levelData.targetScore)
                {
                    OnGameWin(this.GetModel<IUserModel>().currentScore.Value);
                }
                else
                {
                    OnGameLose();
                }
            }
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<GetScoreEvent>(e => { UpdateScore(this.GetModel<IUserModel>().currentScore.Value); })
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    public void UpdateScore(int score)
    {
        totalScoreText.text = score.ToString();

        var visibleStar = 0;

        if (score >= _levelData.score1Star && score < _levelData.score2Star)
        {
            visibleStar = 1;
        }
        else if(score >= _levelData.score2Star && score < _levelData.score3Star)
        {
            visibleStar = 2;
        }
        else if (score >= _levelData.score3Star)
        {
            visibleStar = 3;
        }

        for (var i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i == visibleStar;
        }

        _starCount = visibleStar;
    }

    public void OnGameWin(int score)
    {
        this.SendCommand(new GameWinCommand(score, _starCount));
    }

    public void OnGameLose()
    {
        this.SendCommand(new GameLoseCommand());
    }

    public void OnExitClicked()
    {
        this.SendCommand<PlayButtonClickSoundCommand>();
        gameUIPanel.SetActive(false);
        this.SendCommand(new LoadSceneAsyncCommand("Main"));
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
