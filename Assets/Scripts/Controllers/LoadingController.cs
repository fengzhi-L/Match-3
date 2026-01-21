using System;
using System.Collections;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingController : MonoBehaviour, IController
{
    [Header("UI元素")] 
    public GameObject loadingPanel;
    public TextMeshProUGUI progressText;
    public Image progressBar;
    public Image progressFill;
    public TextMeshProUGUI loadingText;

    [Header("加载设置")] 
    public float minLoadTime = 1f;
    public string[] loadingTips;

    private AsyncOperation _asyncOperation;
    private bool _isLoading = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        loadingPanel.SetActive(false);

        this.RegisterEvent<LoadSceneAsyncEvent>(e => { LoadSceneAsync(e.SceneName); });
    }

    private void LoadSceneAsync(string sceneName)
    {
        if(_isLoading) return;
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        _isLoading = true;

        ResetProgress();
        
        ShowLoading();

        if (loadingTips.Length > 0)
        {
            var randomIndex = Random.Range(0, loadingTips.Length);
            loadingText.text = loadingTips[randomIndex];
        }

        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        // 暂不允许场景激活
        _asyncOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(minLoadTime);

        while (!_asyncOperation.isDone)
        {
            // 将进度限制在0-1之间，其中0.9对应100%（因为Unity内部进度最大约0.9）
            var rawProgress = _asyncOperation.progress;
            var clampedProgress = rawProgress >= 0.9f ? 1f : rawProgress / 0.9f;
            
            var finalProgress = Mathf.Clamp01(clampedProgress);
            
            UpdateProgress(finalProgress);

            if (finalProgress >= 1f)
            {
                _asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        
        UpdateProgress(1f);

        HideLoading();
        _isLoading = false;
    }

    private void UpdateProgress(float progress)
    {
        if (progressText != null)
        {
            progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }

        if (progressFill != null)
        {
            progressFill.fillAmount = progress;
        }
    }

    private void ShowLoading()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
    }
    
    private void ResetProgress()
    {
        if (progressText != null)
        {
            progressText.text = "0%";
        }

        if (progressFill != null)
        {
            progressFill.fillAmount = 0f;
        }
    }

    private void HideLoading()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
