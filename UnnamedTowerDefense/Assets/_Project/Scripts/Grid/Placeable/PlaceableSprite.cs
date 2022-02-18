namespace _Project.Scripts.Grid
{
    public class PlaceableSprite : Placeable<PlaceableCell>
    {
        protected override void OnPlace(PlaceableCell cell) => Cell.Placeholder.SetAlpha(1f);
        protected override void OnRelease() => Cell.Placeholder.SetAlpha(0f);
    }
}