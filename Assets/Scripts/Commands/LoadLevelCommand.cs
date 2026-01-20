using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class LoadLevelCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var targets = new List<MissionTarget>();

        for (var i = 0; i < levelData.targetFruitCounts.Length; i++)
        {
            targets.Add(new MissionTarget()
            {
                FruitType = levelData.availableFruits[i],
                TargetCount = levelData.targetFruitCounts[i],
            });
        }
        
        this.GetModel<IMissionModel>().SetTargets(targets);
    }
}
