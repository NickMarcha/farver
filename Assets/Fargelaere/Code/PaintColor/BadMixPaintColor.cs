using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Poorly mixed paint")]
public class BadMixPaintColor : PaintColor
{
    public Sprite BadMixSprite;

    public override void Apply(PaintBlob blob)
    {
        Colorization = Color.white;
        base.Apply(blob);
        blob.GetComponent<SpriteRenderer>().sprite = BadMixSprite;
    }
}