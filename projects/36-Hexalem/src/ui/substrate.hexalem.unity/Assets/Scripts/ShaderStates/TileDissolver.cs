using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDissolver : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers = new Renderer[0];

    private float targetProgress = 1f;
    private float currentProgress = 0.5f;

    // Update is called once per frame
    private void Update()
    {
        currentProgress = Mathf.Lerp(currentProgress, targetProgress, 2f * Time.deltaTime);

        Debug.Log("Tile Dissolver udpate: " + currentProgress);

        foreach(Renderer renderer in renderers)
        {
            renderer.material.SetFloat("_Progress", currentProgress);
        }
    }
}
