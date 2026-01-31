using System.Collections.Generic;
using UnityEngine;
using GameVanilla.Core;

namespace GameVanilla.Game.Common
{
    /// <summary>
    /// The class used for the color bomb + candy combo.
    /// </summary>
    public class ColorBombWithCandyCombo : ColorBombCombo
    {
        /// <summary>
        /// Resolves this combo.
        /// </summary>
        /// <param name="board">The game board.</param>
        /// <param name="tiles">The tiles destroyed by the combo.</param>
        /// <param name="fxPool">The pool to use for the visual effects.</param>
        public override void Resolve(GameBoard board, List<GameObject> tiles, FxPool fxPool)
        {
            base.Resolve(board, tiles, fxPool);

            var candy = tileA.GetComponent<Candy>() != null ? tileA : tileB;
            var targetColor = candy.GetComponent<Candy>().color;

            // 🔒 IMPORTANT: Work on a copy to avoid "Collection was modified" crash
            var tilesCopy = new List<GameObject>(tiles);

            foreach (var tile in tilesCopy)
            {
                if (tile == null) continue;

                var candyComp = tile.GetComponent<Candy>();
                if (candyComp != null && candyComp.color == targetColor)
                {
                    board.ExplodeTileNonRecursive(tile);
                }
            }

            SoundManager.instance.PlaySound("ColorBomb");
            VibrationManager.Instance?.Vibrate(VibrationType.Heavy);

            board.ApplyGravity();
        }
    }
}
