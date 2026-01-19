using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class EffectSpawner : MonoBehaviour, IController
{
    public GameObject fruitClearPrefab;

    private Queue<GameObject> _fruitClearPrefabPool = new();
    private Transform _effectRoot;

    private void Start()
    {
        _effectRoot = transform;

        this.RegisterEvent<FruitCrushEvent>(e => { OnShowFruitClearEffect(e.Position); });
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
    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
