using System.Collections.Generic;
using UnityEngine;
using GameVanilla.Core;

namespace GameVanilla.Game.Common
{
    public class ColorBombWithWrappedCandyCombo : ColorBombCombo
    {
        public override void Resolve(GameBoard board, List<GameObject> tiles, FxPool fxPool)
        {
            base.Resolve(board, tiles, fxPool);

            var wrapped = tileA.GetComponent<WrappedCandy>() != null ? tileA : tileB;
            var wrappedColor = wrapped.GetComponent<Candy>().color;

            var newTiles = new List<GameObject>();

            // FIX: iterate over copy
            foreach (var tile in new List<GameObject>(tiles))
            {
                if (tile != null && tile.GetComponent<Candy>() != null &&
                    tile.GetComponent<Candy>().color == wrappedColor)
                {
                    var tileComp = tile.GetComponent<Tile>();
                    var x = tileComp.x;
                    var y = tileComp.y;

                    board.ExplodeTileNonRecursive(tile);

                    var newTile = board.CreateWrappedTile(x, y, wrappedColor);
                    newTiles.Add(newTile);
                }
            }

            SoundManager.instance.PlaySound("ColorBomb");
            VibrationManager.Instance?.Vibrate(VibrationType.Heavy);

            board.ExplodeGeneratedTiles(newTiles);
        }
    }
}
