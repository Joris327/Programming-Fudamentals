using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    Interface _interfaceInput;
    
    [SerializeField] GameObject _gameUI;
    [SerializeField] GameObject _pauseMenu;
    public RectTransform _currentPopup;

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

        _interfaceInput = new();

        SetAllPixelsInactive();
        
        Time.timeScale = 0;
        GameManager.Instance.CurrentState = GameManager.GameState.onLevelStart;
    }

    private void Start()
    {
        _interfaceInput.GameInterface.Pause.performed += SwitchPause;
        _interfaceInput.GameInterface.Start.performed += CloseCurrentPopup;
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
    
    void CloseCurrentPopup(InputAction.CallbackContext context)
    {
        if (!_currentPopup) return;
        
        //_currentPopup.gameObject.SetActive(false);
        Destroy(_currentPopup.gameObject);
        GameManager.Instance.CurrentState = GameManager.GameState.inGame;
        Time.timeScale = 1;
    }
    
    public void PauseGame(bool doPause)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.onLevelStart) return;
        if (!Instance) return;

        if (doPause)
        {
            Time.timeScale = 0;
            GameManager.Instance.CurrentState = GameManager.GameState.isPaused;
            
            _pauseMenu.SetActive(true);
            _gameUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            GameManager.Instance.CurrentState = GameManager.GameState.inGame;
            
            _pauseMenu.SetActive(false);
            _gameUI.SetActive(true);
        }
    }

    public void SwitchPause(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.isPaused) PauseGame(false);
        else PauseGame(true);
    }
    
    void OnEnable()
    {
        _interfaceInput.Enable();
    }
    
    void OnDisable()
    {
        _interfaceInput.Disable();
    }
}
