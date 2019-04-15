using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class ColorTarget : GridEntity
{
    [Description("The color of blob this target requires")]
    public PaintColor RequiredColor;

    /// <summary>
    /// Is this target cleared right now. Does it have a matching color blob ontop of itself?
    /// </summary>
    /// <returns></returns>
    public bool IsTargetCleared
    {
        get
        {
            return GetGridEntities(TileMap)

            //Get all paint blobs
            .OfType<PaintBlob>()

            //Are any of the paint blobs on this tile and have matching color
            .Any(i => i.TilePosition == TilePosition && i.Color == RequiredColor);
        }
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = RequiredColor?.Colorization ?? Color.white;
    }

    public override void OnSlideIntoObject(GridEntity other)
    {
        if (other is PaintBlob && CheckForWinState())
        {
			LevelController.WonGame();
        }

    }

    /// <summary>
    /// Checks whether the level has been cleared (all targets have their respective colors on them)
    /// </summary>
    /// <returns></returns>
    private bool CheckForWinState()
    {
        return GetGridEntities(TileMap)

            //Get all color targets
            .OfType<ColorTarget>()

            //Are all color targets cleared
            .All(i => i.IsTargetCleared);
    }

    public override bool Equals(GridEntity other)
    {
        if (other is ColorTarget c)
        {
            return RequiredColor == c.RequiredColor && transform.position == c.transform.position;
        }

        return false;
    }
}
