using System.Collections.Generic;
using UnityEngine;
using GameVanilla.Core;

namespace GameVanilla.Game.Common
{
    public class TwoColorBombCombo : ColorBombCombo
    {
        public override void Resolve(GameBoard board, List<GameObject> tiles, FxPool fxPool)
        {
            // FIX: iterate over a copy to avoid collection modification crash
            foreach (var tile in new List<GameObject>(tiles))
            {
                if (tile != null && (tile.GetComponent<Candy>() != null || tile.GetComponent<ColorBomb>() != null))
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
