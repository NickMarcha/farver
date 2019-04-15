using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class PushableBlock : Pushable
{
    public override bool CanPass(Direction4 incommingDirection)
    {
        return Sliding;
    }

    protected override void OnSlideStop()
    {
		base.OnSlideStop();

        //Destorys any paint blob it comes in contact with
        GetGridEntities(TileMap)
            .OfType<PaintBlob>()
            .Where(i => TilePosition == i.TilePosition)
            .ForEach(i => Destroy(i.gameObject));
    }

    public override bool Equals(GridEntity other)
    {
        if (other is PushableBlock c)
        {
            return transform.position == c.transform.position;
        }

        return false;
    }
}
