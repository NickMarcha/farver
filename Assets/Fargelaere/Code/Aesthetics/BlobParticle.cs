using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlobParticle : MonoBehaviour
{
    public float AnimationSpeed = 1f;
    public AnimationCurve Curve;

    private IEnumerator Start()
    {
        Vector3 originalScale = transform.localScale;

        float elapsedSeconds = 0f;
        while (elapsedSeconds < AnimationSpeed)
        {
            transform.localScale = originalScale * Curve.Evaluate(elapsedSeconds / AnimationSpeed);

            elapsedSeconds += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        Destroy(gameObject);
    }

}
