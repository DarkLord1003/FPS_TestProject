using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair:MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] private float _height;
    [SerializeField] private float _width;
    [SerializeField] private float _defaultSpread;
    [SerializeField] private float _resizeSpread;
    [SerializeField] private float _resizeSpeed;
    [SerializeField] private Color _color;
    [SerializeField] private bool _resizeble = true;

    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    private Texture2D _textureCrosshair;
    private float _spread;
    private bool _resizing = false;
    private bool _isShoot;
    private bool _isRealised = true;

    private void Awake()
    {
        _spread = _defaultSpread;
    }

    private void Start()
    {
        CreateTextureCrosshair();
    }
    private void Update()
    {
        CheckRealisedShoot();
        CheckShoot();

        _resizing = _isShoot;

        if (_resizeble)
        {
            if (_resizing)
            {
                _spread = Mathf.Lerp(_spread, _resizeSpread, _resizeSpeed * Time.deltaTime);
            }
            else
            {
                _spread = Mathf.Lerp(_spread, _defaultSpread, _resizeSpeed * Time.deltaTime);
            }

            _spread = Mathf.Clamp(_spread, _defaultSpread, _resizeSpread);
        }
    }

    private void OnGUI()
    {
        CreateTextureCrosshair();

        GUI.DrawTexture(new Rect(Screen.width / 2f - 2f, Screen.height / 2f - 1f, 2f, 2f), _textureCrosshair);

        GUI.DrawTexture(new Rect(Screen.width / 2f - 2, (Screen.height / 2f) + _spread / 2f, 
                                                                   _width, _height),_textureCrosshair);

        GUI.DrawTexture(new Rect(Screen.width / 2f - 2, (Screen.height / 2f) - _spread / 2f -
                                                              10, _width, _height), _textureCrosshair);

        GUI.DrawTexture(new Rect((Screen.width / 2f) + _spread / 2f, Screen.height / 2f - 1.2f, 
                                                                   _height, _width), _textureCrosshair);

        GUI.DrawTexture(new Rect((Screen.width / 2f) - _spread / 2f - 10, Screen.height / 2f - 1.2f,
                                                                   _height, _width), _textureCrosshair);
    }

    public void SetResizing(bool state)
    {
        _resizing = state;
    }

    public void HideCrosshair(bool hide)
    {
        if (hide)
        {
            _color.a = 0f;
        }
        else
        {
            _color.a = 0.5f;
        }
    }
    private void CreateTextureCrosshair()
    {
        _textureCrosshair = new Texture2D(1, 1);
        _textureCrosshair.SetPixel(0, 0, _color);
        _textureCrosshair.wrapMode = TextureWrapMode.Repeat;
        _textureCrosshair.Apply();
    }


    private void CheckShoot()
    {
        if (_inputManager.ShootIsTrigger || Mathf.Abs(_inputManager.MoveInput.x) > 0.25f || Mathf.Abs(_inputManager.MoveInput.y) > 0.25f)
        {
            _isShoot = true;
        }
        else if((Mathf.Abs(_inputManager.MoveInput.x) < 0.25f && Mathf.Abs(_inputManager.MoveInput.y) < 0.25f) && _isRealised)
        {
            _isShoot = false;
        }
    }

    private void CheckRealisedShoot()
    {
        if (_inputManager.ShootRealisedTrigger)
        {
            _isRealised = true;
        }
        else if (_inputManager.ShootIsTrigger)
        {
            _isRealised = false;
        }
    }

}
