using UnityEditor;
using UnityEngine;

namespace Grid_System.GridSystems.PathGridSystem
{
    [CreateAssetMenu(fileName = "Unnamed Path Setup", menuName = "Grid/Path/Path Setup", order = 1)]
    public class PathSetup : ScriptableObject
    {
        [SerializeField] [TextArea(15, 20)] private string data;

        private const string CurrentDirectory = "Assets/Scripts/Grid System/GridSystems/PathGridSystem/Placeables/";
        
        public void FromGrid(PathGrid template)
        {
            if (template.Cells == null) return;
            data = string.Empty;

            PathPlaceable lastPlaceable = null;
            foreach (PathCell cell in template.Cells)
            {
                if (!cell.IsPlaced) continue;

                if (lastPlaceable != cell.Placed)
                {
                    data += $"loc {cell.Placed!.PlaceableName}\n";
                    lastPlaceable = cell.Placed;
                }
                
                Vector2Int pos = cell.GridPosition;
                data += $"plc {pos.x},{pos.y} {cell.Properties}\n";
            }
            
            data = data != string.Empty ? data[..^1] : data;
        }
        
        public void SetToGrid(ref PathGrid grid)
        {
            grid.Initialize();
            

            string[] lines = data.Split('\n');
            PathPlaceable placeable = null;
            
            foreach (string line in lines)
            {
                if (line.Length == 0) continue; // Skip blank lines

                string[] tokens = line.Split(' ');
                string command = tokens[0];

                if (command == "plc")
                {
                    string lineData = tokens[1] + " " + tokens[2];
                    Vector2Int pos = GetPos(lineData);
                    
                    grid[pos].Place(placeable);

                    string propertiesString = lineData[(lineData.IndexOf(' ') + 1)..];
                    PathCellProperties properties = PathCellProperties.FromString(propertiesString);
                
                    grid[pos].Properties = properties;
                }
                else if (command == "loc")
                {
                    string placeableName = tokens[1];
                    placeableName += ".prefab";

                    string path = CurrentDirectory + placeableName;

                    placeable = AssetDatabase.LoadAssetAtPath<GameObject>(path).GetComponent<PathPlaceable>();
                }
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