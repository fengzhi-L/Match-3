using System.Collections.Generic;
using QFramework;

public interface IMissionModel : IModel
{
    List<MissionTarget> Targets { get; }
    void SetTargets(List<MissionTarget> targets);
    void UpdateTarget(FruitType type, int delta);
}
