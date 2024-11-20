using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class Filter : MonoBehaviour
{
    Renderer _filterRenderer;
    Collider _filterCollider;
    [SerializeField] GameManager.Color _color;
    public GameManager.Color Color { get{return _color;} }

    [Tooltip("How transparent this objects material should be when the player is allowed through.")]
    [Range(0,1), SerializeField] float _alphaValue = 0.7f;

    void Awake()
    {
        if (!TryGetComponent<Renderer>(out _filterRenderer)) _filterRenderer = gameObject.AddComponent<Renderer>();
        if (!TryGetComponent<Collider>(out _filterCollider)) _filterCollider = gameObject.AddComponent<BoxCollider>();

        GameManager.Instance.filters.Add(this);
    }

    public void AllowPassage(bool set)
    {
        _filterCollider.enabled = !set;

        Color c = _filterRenderer.material.color;
        if (set) c.a = _alphaValue;
        else c.a = 1;
        _filterRenderer.material.SetColor("_BaseColor", c);
        //Debug.Log(c.a + ", " + _filterRenderer.material.color.a);
    }

    void OnDestroy()
    {
        GameManager.Instance.filters.Remove(this);
    }
}
