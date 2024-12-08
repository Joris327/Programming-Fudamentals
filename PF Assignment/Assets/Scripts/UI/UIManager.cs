using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] GameObject _pickupPanel;
    [SerializeField] GameObject _subpixelPanel;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] TextMeshProUGUI _pickupText;
    [SerializeField] RawImage _redSubpixel;
    [SerializeField] RawImage _greenSubpixel;
    [SerializeField] RawImage _blueSubpixel;
    [SerializeField] RawImage _magentaPixel;
    [SerializeField] RawImage _cyanPixel;
    [SerializeField] RawImage _yellowPixel;
    [SerializeField] RawImage _whitePixel;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more then one UIManager in the scene, the duplicate will destroy itself");
            Destroy(this);
        }
        else Instance = this;
        
        SetAllInactive();

        _pauseMenu.SetActive(false);
    }
    
    public void UpdatePickupText(int pickupCount)
    {
        _pickupText.text = pickupCount.ToString();
    }
    
    public void UpdateColourDisplay(GameManager.Color color, bool r, bool g, bool b)
    {
        SetAllInactive();
        
        if (color == GameManager.Color.black) return;
        if (r) _redSubpixel.gameObject.SetActive(true);
        if (g) _greenSubpixel.gameObject.SetActive(true);
        if (b) _blueSubpixel.gameObject.SetActive(true);
        
        switch (color)
        {
            case GameManager.Color.yellow: _yellowPixel.gameObject.SetActive(true); break;
            case GameManager.Color.magenta: _magentaPixel.gameObject.SetActive(true); break;
            case GameManager.Color.cyan: _cyanPixel.gameObject.SetActive(true); break;
            case GameManager.Color.white:  _whitePixel. gameObject.SetActive(true); break;
        }
    }
    
    void SetAllInactive()
    {
        _redSubpixel.gameObject.SetActive(false);
        _greenSubpixel.gameObject.SetActive(false);
        _blueSubpixel.gameObject.SetActive(false);
        _magentaPixel.gameObject.SetActive(false);
        _cyanPixel.gameObject.SetActive(false);
        _yellowPixel.gameObject.SetActive(false);
        _whitePixel.gameObject.SetActive(false);
    }

    public void SwitchPaused()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        _subpixelPanel.SetActive(!_subpixelPanel.activeSelf);
        _pickupPanel.SetActive(!_pickupPanel.activeSelf);

        if (Time.timeScale > 0) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }
}
