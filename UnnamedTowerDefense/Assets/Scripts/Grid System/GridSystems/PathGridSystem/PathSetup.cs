using UnityEngine;

namespace Grid_System.GridSystems.PathGridSystem
{
    [CreateAssetMenu(fileName = "Unnamed Path Setup", menuName = "Grid/Path/Path Setup", order = 1)]
    public class PathSetup : ScriptableObject
    {
        [SerializeField] [TextArea(15, 20)] private string data;

        public void FromGrid(PathGrid template)
        {
            data = string.Empty;

            foreach (PathCell cell in template.Cells)
            {
                if (!cell.IsPlaced) continue;

                Vector2Int pos = cell.GridPosition;
                data += $"{pos.x},{pos.y} {cell.Properties.ToString()}\n";
            }

            data = data[..^1];
        }
        
        public void SetToGrid(ref PathGrid grid)
        {
            grid.Initialize();
            

            string[] lines = data.Split('\n');
            foreach (string line in lines)
            {
                if (line.Length == 0) continue; // Skip blank lines
                
                Vector2Int pos = GetPos(line);
                grid.Place(pos);

                string propertiesString = line[(line.IndexOf(' ') + 1)..];
                PathCellProperties properties = PathCellProperties.FromString(propertiesString);
                
                grid[pos].Properties = properties;
            }
            

            Vector2Int GetPos(string line) => new Vector2Int(GetX(line), GetY(line));

            int GetX(string line)
            {
                var xResult = string.Empty;
                
                foreach (char item in line)
                {
                    if (int.TryParse(item.ToString(), out _)) xResult += item;
                    else break;
                }

                return int.Parse(xResult);
            }

            int GetY(string line)
            {
                var yResult = string.Empty;

                line = line[(line.IndexOf(',') + 1)..];
                foreach (char item in line)
                {
                    if (int.TryParse(item.ToString(), out _)) yResult += item;
                    else break;
                }

                return int.Parse(yResult);
            }
        }
    }
}