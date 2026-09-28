using UnityEngine;

namespace Game.Grid
{
    public class WorldGrid : MonoBehaviour
    {
        [SerializeField]
        private int width = 50;

        [SerializeField]
        private int height = 50;

        private CellData[] cells;

        public int Width => width;
        public int Height => height;

        private void Awake()
        {
            InitializeGrid();
        }

        private void Start()
        {
            AddWaste(
                new Vector2Int(10, 15),
                WasteType.Metal,
                3
            );

            ref CellData cell = ref GetCell(10, 15);

            Debug.Log(cell.Waste.Type);
            Debug.Log(cell.Waste.Hardness);
            Debug.Log(cell.Waste.Exists);
        }

        private void InitializeGrid()
        {
            cells = new CellData[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = GetIndex(x, y);

                    cells[index] = new CellData
                    {
                        IsWalkable = true
                    };
                }
            }
        }

        private int GetIndex(int x, int y)
        {
            return y * width + x;
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 &&
                   x < width &&
                   y >= 0 &&
                   y < height;
        }

        public ref CellData GetCell(int x, int y)
        {
            if (!IsInside(x, y))
            {
                throw new System.ArgumentOutOfRangeException(
                    $"Cell ({x}, {y}) is outside the grid."
                );
            }

            return ref cells[GetIndex(x, y)];
        }

        public ref CellData GetCell(Vector2Int position)
        {
            return ref GetCell(position.x, position.y);
        }

        #region Modify CellData[]
        public bool AddWaste(Vector2Int position, WasteType type, byte hardness)
        {
            if (!IsInside(position.x, position.y))
                return false;

            ref CellData cell = ref GetCell(position);

            if (cell.Waste.Exists)
                return false;

            cell.Waste = new WasteData
            {
                Type = type,
                Hardness = hardness,
                ReservedByRobotId = -1
            };

            return true;
        }

        public bool RemoveWaste(Vector2Int position)
        {
            if (!IsInside(position.x, position.y))
                return false;

            ref CellData cell = ref GetCell(position);

            if (!cell.Waste.Exists)
                return false;

            cell.Waste = default;

            return true;
        }
        #endregion

        #region Gizmo
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;

            for (int x = 0; x <= width; x++)
            {
                Vector3 start = new Vector3(x, 0, 0);
                Vector3 end = new Vector3(x, height, 0);

                Gizmos.DrawLine(start, end);
            }

            for (int y = 0; y <= height; y++)
            {
                Vector3 start = new Vector3(0, y, 0);
                Vector3 end = new Vector3(width, y, 0);

                Gizmos.DrawLine(start, end);
            }
        }
        #endregion
    }
}