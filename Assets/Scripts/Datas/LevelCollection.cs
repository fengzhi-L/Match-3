using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCollection", menuName = "Game/Level Collection")]
public class LevelCollection : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();
}
