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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        loadingPanel.SetActive(false);

        this.RegisterEvent<LoadSceneAsyncEvent>(e => { LoadSceneAsync(e.SceneName); });
    }

    private void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
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
            // 0.9f是预设的最大进度值
            var progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            UpdateProgress(progress);

            if (progress >= 1f)
            {
                _asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        HideLoading();
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
