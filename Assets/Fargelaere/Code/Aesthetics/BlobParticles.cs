using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobParticles : MonoBehaviour
{
    public Color Color;
    public BlobParticle Particle;

    public float FadeSpeedBase;
    public float WaitMinTime;
    public float WaitMaxTime;

    public float FadeSpeedRandomness = 1f;
    public float ScaleMinMultiplier = 1f;
    public float ScaleMaxMultiplier = 1f;
    public float PositionRandomness = 1f;
    public Vector2 PositionOffset = new Vector2(0, 0);

    private void OnEnable()
    {
        StartCoroutine(nameof(CoCreateParticles));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(CoCreateParticles));
    }

    private IEnumerator CoCreateParticles()
    {
        //This is because Nico fucks shit up for me :)
        yield return new WaitForEndOfFrame();

        while (true)
        {
            //Do not create paint particles for badly mixed paint blobs 
            if (GetComponent<PaintBlob>()?.Color.Equals(GetComponent<PaintBlob>()?.Fallback) == true)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            CreateBlob();
            yield return new WaitForSecondsRealtime(Random.Range(WaitMinTime, WaitMaxTime));
        }
    }

    private void CreateBlob()
    {
        BlobParticle particle = Instantiate(
            original: Particle, 
            position: new Vector3(Random.Range(-PositionRandomness, PositionRandomness) + transform.position.x + PositionOffset.x, Random.Range(-PositionRandomness, PositionRandomness) + transform.position.y + PositionOffset.y, transform.position.z),
            rotation: Quaternion.identity
        );

        particle.transform.localScale *= Random.Range(ScaleMinMultiplier, ScaleMaxMultiplier);

        if (GetComponent<PaintBlob>())
        {
            particle.GetComponent<SpriteRenderer>().color = GetComponent<PaintBlob>().Color.Colorization;
        }
    }
}
