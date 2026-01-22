using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class EffectSpawner : MonoBehaviour, IController
{
    public GameObject fruitClearPrefab;
    public GameObject scoreEffectPrefab;
    
    private ObjectPool<GameObject> _fruitClearPool;
    private ObjectPool<TextMeshPro> _scoreTextPool;
    private Transform _effectRoot;

    private void Start()
    {
        _effectRoot = transform;

        _fruitClearPool = new ObjectPool<GameObject>(
            createFunc: CreateFruitClearEffect,
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100
        );

        _scoreTextPool = new ObjectPool<TextMeshPro>(
            createFunc: CreateScoreEffect,
            actionOnGet: tmp => tmp.gameObject.SetActive(true),
            actionOnRelease: tmp => tmp.gameObject.SetActive(false),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100
        );

        this.RegisterEvent<FruitCrushEvent>(e => { OnShowFruitClearEffect(e.Position); }).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<GetScoreEvent>(e => { OnShowScoreEffect(e.Position, e.Score); }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private GameObject CreateFruitClearEffect()
    {
        var obj = Instantiate(fruitClearPrefab, _effectRoot, false);
        var animController = obj.GetComponent<AnimationController>();
        animController.OnAnimationFinished += () => _fruitClearPool.Release(obj);
        obj.SetActive(false);
        return obj;
    }

    private TextMeshPro CreateScoreEffect()
    {
        var obj = Instantiate(scoreEffectPrefab, _effectRoot, false);
        var tmp = obj.GetComponent<TextMeshPro>();
        var animController = obj.GetComponent<AnimationController>();
        animController.OnAnimationFinished += () => _scoreTextPool.Release(tmp);
        obj.SetActive(false);
        return tmp;
    }

    private void OnShowFruitClearEffect(Vector3 position)
    {
        var obj = _fruitClearPool.Get();
        obj.transform.localPosition = position;
    }

    private void OnShowScoreEffect(Vector3 position, int score)
    {
        var tmp = _scoreTextPool.Get();
        tmp.transform.localPosition = position;
        tmp.text = score.ToString();
    }

    private void OnDestroy()
    {
        _fruitClearPool?.Dispose();
        _scoreTextPool?.Dispose();
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
