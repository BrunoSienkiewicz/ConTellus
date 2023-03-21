namespace Tiles
{
    public class TileSpeedBoost : Tile
    {
        public override void ApplyPowerUP()
        {
            Player.moveSpeed *= 1.5f;
        }
    }
}