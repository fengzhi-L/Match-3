using QFramework;
using UnityEngine;

public class InputController : MonoBehaviour, IController
{
    [SerializeField] private Camera mainCamera;
    private FruitModel _fruitModel;

    private void Start()
    {
        _fruitModel = GetComponent<IFruitModel>() as FruitModel;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<FruitItem>(out var fruit))
                {
                    _fruitModel.currentSelectedFruit = fruit;
                    this.SendCommand<FruitSelectedCommand>();
                }
            }
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
