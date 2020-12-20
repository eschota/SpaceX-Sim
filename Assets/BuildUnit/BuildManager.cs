using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellWidth = 1f;
    [SerializeField] private float cellHeight = 1f;
    [SerializeField] private BuildCell buildCellPrefab;
    [SerializeField] private Transform buildCellRoot;
    [SerializeField] private LayerMask buildLayerMask;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BuildingUnit[] buildPrefabs;
    [SerializeField] private Transform buildingsRoot;
    [SerializeField] private GameObject deleteUnitPopup;

    private BuildCell[,] _buildCells;
    private BuildingUnit _buildingUnit;
    private List<BuildCell> _selectedCells = new List<BuildCell>();
    private BuildCell _hoverCell;
    private BuildingUnit _buildForDelete;

    private void Start()
    {
        BuildCell();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_buildingUnit != null)
                OnBuildClick();
            else if (_hoverCell != null && _hoverCell.Building != null)
                OnUnitClick();
        }

        CheckSelection();
        UpdateBuildPosition();
    }

    public void StartBuild(int type)
    {
        _buildingUnit = Instantiate(buildPrefabs[type], buildingsRoot, true);
        _buildingUnit.gameObject.SetActive(false);
    }

    private void CheckSelection()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, buildLayerMask))
        {
            var cell = hit.transform.GetComponent<BuildCell>();

            if (_hoverCell == cell)
                return;

            _hoverCell = cell;
            OnHoverCell();
        }
        else
        {
            if (_hoverCell != null)
                _hoverCell = null;
            if (_selectedCells.Count > 0)
                ClearCells();
        }
    }

    private void OnHoverCell()
    {
        if (_selectedCells.Count > 0)
            ClearCells();

        if (!_hoverCell.IsSelected)
            _hoverCell.SetSelection(true);

        if (_buildingUnit == null)
        {
            _selectedCells.Add(_hoverCell);
        }
        else
        {
            if (_buildingUnit.Size == 1)
            {
                _selectedCells.Add(_hoverCell);
            }
            else if (_buildingUnit.Size == 2)
            {
                var cells = new BuildCell[4];

                if (_hoverCell.x == width - 1 && _hoverCell.y == 0)
                {
                    cells[0] = _buildCells[_hoverCell.x - 1, _hoverCell.y + 1];
                    cells[1] = _buildCells[_hoverCell.x, _hoverCell.y + 1];
                    cells[2] = _buildCells[_hoverCell.x - 1, _hoverCell.y];
                    cells[3] = _buildCells[_hoverCell.x, _hoverCell.y];
                }
                else if (_hoverCell.x == width - 1)
                {
                    cells[0] = _buildCells[_hoverCell.x - 1, _hoverCell.y];
                    cells[1] = _buildCells[_hoverCell.x, _hoverCell.y];
                    cells[2] = _buildCells[_hoverCell.x - 1, _hoverCell.y - 1];
                    cells[3] = _buildCells[_hoverCell.x, _hoverCell.y - 1];
                }
                else if (_hoverCell.y == 0)
                {
                    cells[0] = _buildCells[_hoverCell.x, _hoverCell.y + 1];
                    cells[1] = _buildCells[_hoverCell.x + 1, _hoverCell.y + 1];
                    cells[2] = _buildCells[_hoverCell.x, _hoverCell.y];
                    cells[3] = _buildCells[_hoverCell.x + 1, _hoverCell.y];
                }
                else
                {
                    cells[0] = _buildCells[_hoverCell.x, _hoverCell.y];
                    cells[1] = _buildCells[_hoverCell.x + 1, _hoverCell.y];
                    cells[2] = _buildCells[_hoverCell.x, _hoverCell.y - 1];
                    cells[3] = _buildCells[_hoverCell.x + 1, _hoverCell.y - 1];
                }

                foreach (var ceil in cells)
                    ceil.SetSelection(true);

                _selectedCells.AddRange(cells);
            }
            else if (_buildingUnit.Size == 3)
            {
                var cells = new BuildCell[9];
                var xOffset = 0;
                var yOffset = 0;

                if (_hoverCell.x >= width - 2)
                {
                    xOffset = width - _hoverCell.x;
                    xOffset = 3 - xOffset;
                }

                if (_hoverCell.y < 2)
                {
                    yOffset = _hoverCell.y;
                    yOffset = 2 - yOffset;
                }

                cells[0] = _buildCells[_hoverCell.x - xOffset, _hoverCell.y + yOffset];
                cells[1] = _buildCells[_hoverCell.x + 1 - xOffset, _hoverCell.y + yOffset];
                cells[2] = _buildCells[_hoverCell.x + 2 - xOffset, _hoverCell.y + yOffset];

                cells[3] = _buildCells[_hoverCell.x - xOffset, _hoverCell.y - 1 + yOffset];
                cells[4] = _buildCells[_hoverCell.x + 1 - xOffset, _hoverCell.y - 1 + yOffset];
                cells[5] = _buildCells[_hoverCell.x + 2 - xOffset, _hoverCell.y - 1 + yOffset];

                cells[6] = _buildCells[_hoverCell.x - xOffset, _hoverCell.y - 2 + yOffset];
                cells[7] = _buildCells[_hoverCell.x + 1 - xOffset, _hoverCell.y - 2 + yOffset];
                cells[8] = _buildCells[_hoverCell.x + 2 - xOffset, _hoverCell.y - 2 + yOffset];

                foreach (var ceil in cells)
                    ceil.SetSelection(true);

                _selectedCells.AddRange(cells);
            }
        }
    }

    private void ClearCells()
    {
        foreach (var selectedCell in _selectedCells)
            selectedCell.SetSelection(false);
        _selectedCells.Clear();
    }

    private void BuildCell()
    {
        _buildCells = new BuildCell[width, height];

        var left = -width / cellWidth / 2f + .5f;
        var top = height / cellHeight / 2f - .5f;

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var buildCell = SpawnBuildCell(new Vector3(
                    left + i * cellWidth,
                    0f,
                    top - j * cellHeight
                ));
                _buildCells[j, i] = buildCell;
                buildCell.x = j;
                buildCell.y = i;
            }
        }
    }

    private void OnBuildClick()
    {
        if (_selectedCells.Count == 0)
            return;

        foreach (var selectedCell in _selectedCells)
        {
            if (selectedCell.Building != null)
                return;
        }

        foreach (var selectedCell in _selectedCells)
            selectedCell.Building = _buildingUnit.gameObject;
        _buildingUnit.Cells = _selectedCells.ToArray();

        _buildingUnit = null;
    }

    private BuildCell SpawnBuildCell(Vector3 position)
    {
        var prefab = Instantiate(buildCellPrefab, position, Quaternion.identity);
        prefab.gameObject.SetActive(true);
        prefab.transform.parent = buildCellRoot;
        return prefab;
    }

    private void UpdateBuildPosition()
    {
        if (_buildingUnit == null)
            return;

        if (_hoverCell != null)
        {
            if (!_buildingUnit.gameObject.activeSelf)
                _buildingUnit.gameObject.SetActive(true);

            var position = _selectedCells[0].transform.position;
            _buildingUnit.transform.position = position;
            if (_buildingUnit.Size == 1)
            {
                _buildingUnit.transform.localPosition =
                    new Vector3(_buildingUnit.transform.localPosition.x, 0f, _buildingUnit.transform.localPosition.z);
            }
            else if (_buildingUnit.Size == 2)
            {
                _buildingUnit.transform.localPosition =
                    new Vector3(_buildingUnit.transform.localPosition.x - .5f, 0f,
                        _buildingUnit.transform.localPosition.z - .5f);
            }
            else if (_buildingUnit.Size == 3)
            {
                _buildingUnit.transform.localPosition =
                    new Vector3(_buildingUnit.transform.localPosition.x - 1f, 0f,
                        _buildingUnit.transform.localPosition.z - 1f);
            }
        }
        else
        {
            _buildingUnit.gameObject.SetActive(false);
        }
    }

    private void OnUnitClick()
    {
        _buildForDelete = _hoverCell.Building.GetComponent<BuildingUnit>();
        deleteUnitPopup.SetActive(true);
    }

    public void DeleteUnitAccept()
    {
        deleteUnitPopup.SetActive(false);

        foreach (var cell in _buildForDelete.Cells)
            cell.Building = null;
        DestroyImmediate(_buildForDelete.gameObject);

        _buildForDelete = null;
    }

    public void DeleteUnitCancel()
    {
        deleteUnitPopup.SetActive(false);
        _buildForDelete = null;
    }
}