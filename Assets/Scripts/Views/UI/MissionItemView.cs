using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemView : MonoBehaviour
{
    public Image icon;

    public TextMeshProUGUI targetText;

    private FruitType _type;
    [SerializeField] private FruitSpriteConfig spriteConfig;

    private void Start()
    {
        Match3.Interface.RegisterEvent<FruitCrushEvent>(e => { UpdateTarget(e.FruitType); })
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    public void SetData(FruitType type, int target)
    {
        _type = type;
        icon.sprite = spriteConfig.GetSprite(type);
        targetText.text = $"{target}";
    }

    private void UpdateTarget(FruitType targetType)
    {
        if (_type == targetType)
        {
            if (int.TryParse(targetText.text, out var val))
            {
                targetText.text = (val - 1).ToString();
                if(val == 0) Destroy(gameObject);
            }
        }
    }
}
