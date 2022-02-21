using _Project.Scripts.Grid.Placeholders;

namespace _Project.Scripts.Grid
{
    public class PlaceablePath : Placeable<PathCell, PathGrid, PathPlaceholder>
    {
        protected override void OnPlace(PathCell pathCell) => pathCell.Placeholder.SetAlpha(1f);
        protected override void OnRelease() => Cell.Placeholder.SetAlpha(0f);
    }
}