using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Light))]
public class PlayerColor : MonoBehaviour
{
    Renderer _playerRenderer;
    Light _playerLight;

    [SerializeField] GameManager.Color _startColor = 0;

    bool _r = false;
    bool _g = false;
    bool _b = false;

    void Awake()
    {
        if (!TryGetComponent<Renderer>(out _playerRenderer)) _playerRenderer = gameObject.AddComponent<Renderer>();
        if (!TryGetComponent<Light   >(out _playerLight   )) _playerLight    = gameObject.AddComponent<Light   >();
    }

    void Start()
    {
        SetColor(_startColor);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Painter>(out Painter p)) return;

        AddColor(p.color);
    }

    void AddColor(GameManager.Color color)
    {
        switch (color)
        {
            case GameManager.Color.black:
                _r = false;
                _g = false;
                _b = false;
                SetColor(GameManager.Color.black);
                return;

            case GameManager.Color.red:
                _r = true;
                break;

            case GameManager.Color.green:
                _g = true;
                break;

            case GameManager.Color.blue:
                _b = true;
                break;

            default:
                return;
        }

        float colorValue = 0;

        if (_r) colorValue += (int)GameManager.Color.red;
        if (_g) colorValue += (int)GameManager.Color.green;
        if (_b) colorValue += (int)GameManager.Color.blue;

        SetColor((GameManager.Color)colorValue);
    }

    void SetColor(GameManager.Color color)
    {
        Material newMaterial = GameManager.Instance.GetMaterial(color);
        _playerRenderer.material = newMaterial;
        _playerLight.color = newMaterial.color;

        GameManager.Instance.AdaptFilters(color);
    }
}
