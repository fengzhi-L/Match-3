public class MissionTarget
{
    public FruitType FruitType;
    public int TargetCount;

    public bool isCompleted => TargetCount <= 0;
}
