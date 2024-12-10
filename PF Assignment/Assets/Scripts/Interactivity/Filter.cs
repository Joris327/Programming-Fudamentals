using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class Filter : MonoBehaviour
{
    Renderer _filterRenderer;
    BoxCollider _filterCollider;
    [SerializeField] GameManager.Color _color;
    public GameManager.Color Color { get{return _color;} }

    [Tooltip("How transparent this objects material should be when the player is allowed through.")]
    [Range(0,1), SerializeField] float _alphaValue = 0.5f;

    void Awake()
    {
        if (!TryGetComponent<Renderer   >(out _filterRenderer)) _filterRenderer = gameObject.AddComponent<Renderer   >();
        if (!TryGetComponent<BoxCollider>(out _filterCollider)) _filterCollider = gameObject.AddComponent<BoxCollider>();
        
        GameManager.filters.Add(this);
    }

    public void AllowPassage(bool set)
    {
        _filterCollider.enabled = !set;

        Color c = _filterRenderer.material.color;
        if (set) c.a = _alphaValue;
        else c.a = 1;
        _filterRenderer.material.SetColor("_BaseColor", c);
        
        Light[] lights = GetComponentsInChildren<Light>();
        foreach (Light l in lights) l.enabled = !set;
    }

    void OnDestroy()
    {
        GameManager.filters.Remove(this);
    }
}
