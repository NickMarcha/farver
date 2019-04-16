using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ToggleBlock : GridEntity
{
    public bool BlockIsSolid;

    [Header("Sprite")]
    public Sprite BlockSprite;
    public Sprite OpenSprite;

    public override bool CanPass(GridEntity other, Direction4 incommingDirection)
    {
        return !BlockIsSolid;
    }

    private void Start()
    {
        UpdateSprite();
    }

    public void ToggleSolidity()
    {
        if (GetGridEntities(TileMap).Any(i => i.TilePosition == TilePosition && i != this || ((i as Pushable)?.Sliding ?? false)))
        {
            return;
        }

        BlockIsSolid = !BlockIsSolid;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = BlockIsSolid ? BlockSprite : OpenSprite;
    }

    private void ToggleSolidity(GameObject go)
    {
		if (gameObject == go)
		{
			LevelController.SaveOldState();
			StartCoroutine(nameof(CoToggleSolidity));
		}
   }

	private IEnumerator CoToggleSolidity()
	{
		yield return new WaitForEndOfFrame();
		ToggleSolidity();
	}


	private void OnEnable()
    {
        TouchInputController.AddListeners(tapObj: ToggleSolidity);
    }

    private void OnDisable()
    {
        TouchInputController.RemoveListeners(tapObj: ToggleSolidity);
    }

    public override bool Equals(GridEntity other)
    {
        if (other is ToggleBlock c)
        {
            return BlockIsSolid == c.BlockIsSolid && transform.position == c.transform.position;
        }

        return false;
    }

}