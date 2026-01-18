using System;

[Serializable]
public class FruitCell
{
    public FruitType fruitType;
    public int rowIndex;
    public int colIndex;

    public FruitCell(FruitType type, int row, int col)
    {
        fruitType = type;
        rowIndex = row;
        colIndex = col;
    }

    public FruitCell(int row, int col)
    {
        rowIndex = row;
        colIndex = col;
    }
}