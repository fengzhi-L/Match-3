using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class MissionPanelController : MonoBehaviour , IController
{
    [SerializeField] private GameObject missionItemPrefab;

    [SerializeField] private Transform contentParent;

    private List<MissionItemView> _activeItems = new();

    private void Start()
    {
        OnMissionTargetsChanged();
    }

    private void OnMissionTargetsChanged()
    {
        var targets = this.GetModel<IMissionModel>().Targets;
        
        while (_activeItems.Count < targets.Count)
        {
            var item = Instantiate(missionItemPrefab, contentParent, false);
            var itemView = item.GetComponent<MissionItemView>();
            _activeItems.Add(itemView);
        }

        for (var i = targets.Count; i < _activeItems.Count; i++)
        {
            Destroy(_activeItems[i].gameObject);
        }
        _activeItems.RemoveRange(targets.Count, _activeItems.Count - targets.Count);

        for (var i = 0; i < targets.Count; i++)
        {
            var target = targets[i];
            _activeItems[i].SetData(target.FruitType, target.TargetCount);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
