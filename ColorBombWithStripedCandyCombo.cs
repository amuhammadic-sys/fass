using System.Collections.Generic;
using UnityEngine;
using GameVanilla.Core;

namespace GameVanilla.Game.Common
{
    public class ColorBombWithStripedCandyCombo : ColorBombCombo
    {
        public override void Resolve(GameBoard board, List<GameObject> tiles, FxPool fxPool)
        {
            base.Resolve(board, tiles, fxPool);

            var striped = tileA.GetComponent<StripedCandy>() != null ? tileA : tileB;
            var stripedColor = striped.GetComponent<Candy>().color;

            var newTiles = new List<GameObject>();

            // FIX: iterate over copy
            foreach (var tile in new List<GameObject>(tiles))
            {
                if (tile != null && tile.GetComponent<Candy>() != null &&
                    tile.GetComponent<Candy>().color == stripedColor)
                {
                    var tileComp = tile.GetComponent<Tile>();
                    var x = tileComp.x;
                    var y = tileComp.y;

                    board.ExplodeTileNonRecursive(tile);

                    GameObject newTile;
                    if (Random.Range(0, 2) == 0)
                        newTile = board.CreateHorizontalStripedTile(x, y, stripedColor);
                    else
                        newTile = board.CreateVerticalStripedTile(x, y, stripedColor);

                    newTiles.Add(newTile);
                }
            }

            SoundManager.instance.PlaySound("ColorBomb");
            VibrationManager.Instance?.Vibrate(VibrationType.Heavy);
            board.ExplodeGeneratedTiles(newTiles);
        }
    }
}
