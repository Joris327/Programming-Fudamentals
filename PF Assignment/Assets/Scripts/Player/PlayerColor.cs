using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Light))]
public class PlayerColor : MonoBehaviour
{
    Renderer _playerRenderer;
    Light _playerLight;
    [SerializeField] float _redLightIntensity = 5;
    [SerializeField] float _defaultLightIntensity = 3;

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
                GameManager.Instance.AdaptPainters(color);
                return;

            case GameManager.Color.red:
                GameManager.Instance.AdaptPainters(color);
                _r = true;
                break;

            case GameManager.Color.green:
                GameManager.Instance.AdaptPainters(color);
                _g = true;
                break;

            case GameManager.Color.blue:
                GameManager.Instance.AdaptPainters(color);
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
        
        if (color == GameManager.Color.red) _playerLight.intensity = _redLightIntensity;
        else _playerLight.intensity = _defaultLightIntensity;

        GameManager.Instance.AdaptFilters(color);
        UIManager.Instance.UpdateColourDisplay(color, _r, _g, _b);
    }
}
