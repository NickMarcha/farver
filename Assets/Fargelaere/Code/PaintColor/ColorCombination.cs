using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Color Combination")]
public class ColorCombination : ScriptableObject
{
    [Header("Input")]
    public PaintColor ColorA;
    public PaintColor ColorB;

    [Header("Output")]
    public PaintColor Result;

    /// <summary>
    /// Do the given colors match the input colors of this color combination?
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool Matches(PaintColor a, PaintColor b)
    {
        return (ColorA == a && ColorB == b) || (ColorA == b && ColorB == a);
    }
}