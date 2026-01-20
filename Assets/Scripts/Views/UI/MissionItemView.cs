using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemView : MonoBehaviour
{
    public Image icon;

    public TextMeshProUGUI targetText;

    private FruitType _type;
    [SerializeField] private FruitSpriteConfig spriteConfig;

    public void SetData(FruitType type, int target)
    {
        _type = type;
        icon.sprite = spriteConfig.GetSprite(type);
        targetText.text = $"{target}";
    }
}
