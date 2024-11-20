using Assets.Scripts.Entity.Buildings;
using UnityEngine;
using UnityEngine.UI;

public class DraggableBuilding : Building, IDraggable
{
    
    public float SelectedAlpha = 0.5f;
    
    private SpriteRenderer _image;
    private Color _startColor;
    private void Awake()
    {
        _image = GetComponent<SpriteRenderer>();
        _startColor = _image.color;
    }

    public void Drag()
    {
        _image.color = new Color(_startColor.r, _startColor.g, _startColor.b, SelectedAlpha);
    }

    public void UnDrag()
    {  
        _image.color = _startColor;
    }
}
