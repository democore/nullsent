using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostVisual : MonoBehaviour {

    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    public LineRenderer Smile;

    private SpriteRenderer sRender;

    private void Awake()
    {
        sRender = GetComponent<SpriteRenderer>();
    }

    public void SetAlpha(float alpha)
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            Color c = sprite.color;
            sprite.color = new Color(c.r, c.g, c.b, alpha);
        }

        Smile.startColor = new Color(0f, 0f, 0f, alpha);
        Smile.endColor = new Color(0f, 0f, 0f, alpha);
        Smile.SetPosition(1, new Vector3(0f, Mathf.Lerp(0.15f, -0.05f, alpha), 0f));
    }

    // Dirty method so I don't have to edit CharResource for now.
    void LateUpdate()
    {
        SetAlpha(sRender.color.a);
    }

    void Squeak()
    {
        AudioManager.Instance.PlayEffect("hide");
    }
}
