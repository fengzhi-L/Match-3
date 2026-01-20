using System.Collections;
using System.Collections.Generic;
using QFramework;

public class MissionModel : AbstractModel, IMissionModel
{
    public List<MissionTarget> Targets { get; set; }
    
    public void SetTargets(List<MissionTarget> targets)
    {
        Targets = targets;
    }

    public void UpdateTarget(FruitType type, int delta)
    {
        
    }
    
    protected override void OnInit()
    {
        
    }
}
