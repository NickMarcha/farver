using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Paint Color")]
public class PaintColor : ScriptableObject
{
    [HideInSubClass]
    public Color Colorization;

    public virtual void Apply(PaintBlob blob)
    {
        blob.Color = this;
        blob.GetComponent<SpriteRenderer>().color = Colorization;
    }
}
