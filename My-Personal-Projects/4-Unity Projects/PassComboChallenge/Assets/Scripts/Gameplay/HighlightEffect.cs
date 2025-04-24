using UnityEngine;

public class HighlightEffect : MonoBehaviour
{
    #region fields | variables
    private SpriteRenderer spriteRenderer;
    private Color normalColor = Color.white;
    private Color highlightColor = Color.black;
    private bool isHighlighted = false;
    #endregion

    #region Mono and methods
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Highlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlightColor;
            isHighlighted = true;
        }
    }

    public void RemoveHighlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
            isHighlighted = false;
        }
    }

    public bool IsHighlighted()
    {
        return isHighlighted;
    }
    #endregion
}
