using UnityEngine;

public class BuildingUnit : MonoBehaviour
{
    [SerializeField] private int size;
    [SerializeField] private BuildCell[] cells;

    public int Size => size;
    public BuildCell[] Cells
    {
        get => cells;
        set => cells = value;
    }
}