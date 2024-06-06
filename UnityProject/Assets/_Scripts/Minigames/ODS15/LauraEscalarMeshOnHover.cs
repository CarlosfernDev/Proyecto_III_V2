using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauraEscalarMeshOnHover : SelectionInteractableParent
{
    private float maxSize;
    private float originalSizeX;
    private float originalSizeY;
    private float originalSizeZ;
    public float size; 
    public float growFactor;
    public float waitTime;
    public bool hovered = false;
    private Coroutine scaleupcor;
    private Coroutine scaledowncor;

    public void Awake()
    {
        originalSizeX = transform.localScale.x;
        originalSizeY = transform.localScale.y;
        originalSizeZ = transform.localScale.z;
    }
    public override void Hover()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Play("ScaleUp");
        hovered = true;
        maxSize = size* originalSizeX;
        if (scaledowncor != null)
        {
            StopCoroutine(scaledowncor);
        }
        scaleupcor = StartCoroutine(ScaleUp());
        base.Hover();
    }

    public override void UnHover()
    {
       // if (AudioManager.Instance != null) AudioManager.Instance.Play("ScaleDown");
        if (scaleupcor != null)
        {
            StopCoroutine(scaleupcor);
        }
        maxSize = 0;
        hovered = false;
        scaledowncor = StartCoroutine(ScaleDown());
        base.UnHover();
    }

    public void OnEnable()
    {
        transform.localScale = new Vector3(originalSizeX, originalSizeY, originalSizeZ);
        growFactor = originalSizeX / size;


    }



    IEnumerator ScaleUp()
    {
        float timer = 0;

        while (hovered) // this could also be a condition indicating "alive or dead"
        {
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (maxSize > transform.localScale.x)
            {
                timer += Time.deltaTime;
                transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator ScaleDown()
    {
        float timer = 0;

        while (!hovered) // this could also be a condition indicating "alive or dead"
        { 
            timer = 0;
            while (originalSizeX < transform.localScale.x)
            {
                timer += Time.deltaTime;
                transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
