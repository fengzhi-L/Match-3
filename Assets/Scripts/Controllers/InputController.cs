using System;
using QFramework;
using UnityEngine;

public class InputController : MonoBehaviour, IController
{
    [SerializeField] private Camera mainCamera;
    private FruitModel _fruitModel;
    private Vector3 _startPosition;

    private void Start()
    {
        _fruitModel = this.GetModel<IFruitModel>() as FruitModel;
    }

    void Update()
    {
        if (IsInputDown())
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<FruitItem>(out var fruit))
                {
                    Debug.Log($"点击了水果{fruit.rowIndex}, {fruit.columnIndex}");
                    _startPosition = GetInputPosition();
                    this.SendCommand(new FruitSelectedCommand(fruit));
                }
            }
        }
        
        if(_fruitModel.currentSelectedFruit == null) return;

        if (IsInputMove())
        {
            var currentPos = GetInputPosition();
            var delta = currentPos - _startPosition;
            
            if(delta.magnitude <= 20f) return;
            Debug.Log("移动水果");
            this.SendCommand(new FruitMoveCommand(delta));
            this.SendCommand(new FruitUnSelectedCommand(_fruitModel.currentSelectedFruit));
        }
    }
    
    // 检测是否有输入按下
    private bool IsInputDown()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonDown(0);
#elif UNITY_IOS || UNITY_ANDROID
        // 移动端：触摸开始
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
    }
    
    private Vector3 GetInputPosition()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // PC端：鼠标位置
        return Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID
        // 移动端：第一个触摸点位置
        return Input.GetTouch(0).position;
#endif
    }
    
    private bool IsInputMove()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButton(0);
#elif UNITY_IOS || UNITY_ANDROID
        // 移动端：触摸开始
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
#endif
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
