using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tiles
{
    public class TileAttackSpeed : Tile
    {
        public override void ApplyPowerUP()
        {
            Player.timeBetweenShots *= 0.7f;
        }
    }
}
