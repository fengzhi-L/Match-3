using System;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEngine;

public class EffectSpawner : MonoBehaviour, IController
{
    public GameObject fruitClearPrefab;
    public GameObject scoreEffectPrefab;

    private Queue<GameObject> _fruitClearPrefabPool = new();
    private Queue<TextMeshPro> _scoreEffectPool = new();
    private Transform _effectRoot;

    private void Start()
    {
        _effectRoot = transform;

        this.RegisterEvent<FruitCrushEvent>(e => { OnShowFruitClearEffect(e.Position); });
        this.RegisterEvent<GetScoreEvent>(e => { OnShowScoreEffect(e.Position, e.Score); });
    }

    public void OnShowFruitClearEffect(Vector3 position)
    {
        GameObject obj;
        if (_fruitClearPrefabPool.Count > 0)
        {
            obj = _fruitClearPrefabPool.Dequeue();
        }
        else
        {
            obj = Instantiate(fruitClearPrefab, _effectRoot, false);

            var bhv = obj.GetComponent<AnimationController>();
            bhv.animEventCallback = (str) =>
            {
                if (str == "finish")
                {
                    obj.SetActive(false);
                    _fruitClearPrefabPool.Enqueue(obj);
                }
            };
        }
        
        obj.SetActive(true);
        obj.transform.position = new Vector3(position.x, position.y, _effectRoot.transform.position.z);
    }

    public void OnShowScoreEffect(Vector3 position, int score)
    {
        TextMeshPro textMeshPro = null;
        if (_scoreEffectPool.Count > 0)
        {
            textMeshPro = _scoreEffectPool.Dequeue();
        }
        else
        {
            var obj = Instantiate(scoreEffectPrefab, _effectRoot, false);
            textMeshPro = obj.GetComponent<TextMeshPro>();
            var animController = obj.GetComponent<AnimationController>();
            animController.animEventCallback = (str) =>
            {
                if (str == "finish")
                {
                    obj.SetActive(false);
                    _scoreEffectPool.Enqueue(textMeshPro);
                }
            };
        }
        
        textMeshPro.gameObject.SetActive(true);
        textMeshPro.transform.position = new Vector3(position.x, position.y, _effectRoot.transform.position.z);
        textMeshPro.text = score.ToString();
    }
    
    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
