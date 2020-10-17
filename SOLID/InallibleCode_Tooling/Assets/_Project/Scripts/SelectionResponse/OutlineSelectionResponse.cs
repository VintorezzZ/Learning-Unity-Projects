using UnityEngine;

public class OutlineSelectionResponse : MonoBehaviour, ISelectionResponse
{
    public Outline.Mode OutlineMode;
    public Color outlineColor;
	public float outlineWidth = 20;
	
    public void OnSelect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineMode = OutlineMode;
            outline.OutlineColor = outlineColor;
            outline.OutlineWidth = outlineWidth;
        }
    }

    public void OnDeselect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineWidth = 0;
        }
    }
}