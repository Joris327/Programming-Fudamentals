using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //[SerializeField] GameObject _pickupPanel;
    //[SerializeField] GameObject _subpixelPanel;
    [SerializeField] GameObject _gameUI;
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
        if (Instance == this) return;
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one UIManager in the scene");
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            Debug.Log("UIManager Instance set");
        }

        SetAllPixelsInactive();
    }
    
    public void UpdatePickupText(int pickupCount)
    {
        _pickupText.text = pickupCount.ToString();
    }
    
    public void UpdateColourDisplay(GameManager.Color color, bool r, bool g, bool b)
    {
        SetAllPixelsInactive();
        
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
    
    void SetAllPixelsInactive()
    {
        _redSubpixel.gameObject.SetActive(false);
        _greenSubpixel.gameObject.SetActive(false);
        _blueSubpixel.gameObject.SetActive(false);
        _magentaPixel.gameObject.SetActive(false);
        _cyanPixel.gameObject.SetActive(false);
        _yellowPixel.gameObject.SetActive(false);
        _whitePixel.gameObject.SetActive(false);
    }

    public void EnablePauseMenu()
    {
        _pauseMenu.SetActive(true);
        _gameUI.SetActive(false);
    }

    public void EnableGameUI()
    {
        _pauseMenu.SetActive(false);
        _gameUI.SetActive(true);
    }
}
