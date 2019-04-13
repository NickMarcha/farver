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
}