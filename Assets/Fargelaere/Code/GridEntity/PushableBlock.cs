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
        return false;
    }

    public override void OnSlideIntoObject(GridEntity other)
    {
        //Destorys any paint blob it comes in contact with
        if (other is PaintBlob)
        {
            Destroy(other.gameObject);
        }
    }

    private void OnEnable()
    {
#if !UNITY_EDITOR
		 TouchInputController.AddListeners(swipe: Push);   
#else
        if (UnityEditor.EditorApplication.isPlaying)
        {
            TouchInputController.AddListeners(swipe: Push);
        }
#endif
    }

    private void OnDisable()
    {
#if !UNITY_EDITOR
		 TouchInputController.RemoveListeners(swipe: Push);   
#else
        if (UnityEditor.EditorApplication.isPlaying)
        {
            TouchInputController.RemoveListeners(swipe: Push);
        }
#endif
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
