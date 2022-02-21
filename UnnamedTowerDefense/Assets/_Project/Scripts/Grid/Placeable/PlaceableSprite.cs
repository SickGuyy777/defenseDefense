using _Project.Scripts.Grid.Placeholders;

namespace _Project.Scripts.Grid
{
    public class PlaceableSprite : Placeable<PlaceableCell, PlaceableGrid, SpriteCellPlaceholder>
    {
        protected override void OnPlace(PlaceableCell cell) => Cell.Placeholder.SetAlpha(1f);
        protected override void OnRelease() => Cell.Placeholder.SetAlpha(0f);
    }
}