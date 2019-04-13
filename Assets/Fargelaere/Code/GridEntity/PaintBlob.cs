using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

/// <summary>
/// Represents a paint blob that can combine with other paintblobs
/// </summary>
public class PaintBlob : Pushable
{
    private void OnEnable()
    {
        TouchInputController.AddListeners(swipe: Push);
    }

    private void OnDisable()
    {
        TouchInputController.RemoveListeners(swipe: Push);
    }
}
