using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public enum Sound
{
    Master,
    BGM,
    SFX
}

public class OptionPopup : UIBase
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Button _leftButton;

    [Header("Volume")]
    [SerializeField] private Button _masterVolumeLeftButton;
    [SerializeField] private Button _masterVolumeRightButton;
    [SerializeField] private Button _bgmVolumeLeftButton;
    [SerializeField] private Button _bgmVolumeRightButton;
    [SerializeField] private Button _sfxVolumeLeftButton;
    [SerializeField] private Button _sfxVolumeRightButton;
    [SerializeField] private Image _masterVolumeImage;
    [SerializeField] private Image _bgmVolumeImage;
    [SerializeField] private Image _sfxVolumeImage;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private List<Resolution> _resolutions = new List<Resolution>();
    private int _resolutionNumber;

    [Header("MouseSensitive")]
    [SerializeField] private Slider _mouseSensitive;
    [SerializeField] private TextMeshProUGUI _sensitivePercentText;

    [Header("Quit")]
    [SerializeField] private Button _cancelButton;
    private void Start()
    {
        _leftButton.onClick.AddListener(() =>
        {
            Inventory.instance.optionUI.SetActive(false);
            Inventory.instance._inventoryUI.SetActive(true);
        });
        _masterVolumeRightButton.onClick.AddListener(() => OnClickRightButton(Sound.Master));
        _masterVolumeLeftButton.onClick.AddListener(() => OnClickVolumeLeftButton(Sound.Master));
        _bgmVolumeRightButton.onClick.AddListener(() => OnClickRightButton(Sound.BGM));
        _bgmVolumeLeftButton.onClick.AddListener(() => OnClickVolumeLeftButton(Sound.BGM));
        _sfxVolumeRightButton.onClick.AddListener(() => OnClickRightButton(Sound.SFX));
        _sfxVolumeLeftButton.onClick.AddListener(() => OnClickVolumeLeftButton(Sound.SFX));
        _mouseSensitive.onValueChanged.AddListener((value) =>
        {
            CameraTPP.mouseSmoothSpeed = value + 0.5f;
            _sensitivePercentText.text = $"{(int)(value * 100) - 50}%";
        });

        Initialize();

        _cancelButton.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    #region resolution
    public override void Initialize()
    {
        _audioMixer = GameManager.Instance.soundManager.mixer;
        //mouse init
        _mouseSensitive.value = CameraTPP.mouseSmoothSpeed - 0.5f;

        //sound init
        SetInitialFillAmount(Sound.Master, _masterVolumeImage);
        SetInitialFillAmount(Sound.BGM, _bgmVolumeImage);
        SetInitialFillAmount(Sound.SFX, _sfxVolumeImage);

        //resolution init
        ResolutionInitialize();
    }
    void ResolutionInitialize()
    {
        _resolutions.AddRange(Screen.resolutions);
        _resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution resolution in _resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{resolution.width}X{resolution.height} {resolution.refreshRateRatio.value}hz";
            _resolutionDropdown.options.Add(option);
            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                _resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        _resolutionDropdown.RefreshShownValue();
        _resolutionDropdown.onValueChanged.AddListener((x) =>
        {
            _resolutionNumber = x;
            Debug.Log("OnValueChange" + _resolutions[_resolutionNumber].width);
            Screen.SetResolution(_resolutions[_resolutionNumber].width, _resolutions[_resolutionNumber].height, true);
        });
    }
    #endregion

    #region Sound
    private void SetInitialFillAmount(Sound type, Image volumeImage)
    {
        _audioMixer.GetFloat(Enum.GetName(typeof(Sound), type), out float value);
        volumeImage.fillAmount = ConvertDecibelToFillAmount(value);
    }
    private float ConvertDecibelToFillAmount(float decibel)
    {
        float minDecibel = -40f;
        float maxDecibel = 10f;

        if (decibel < minDecibel) return 0f;
        if (decibel > maxDecibel) return 1f;
        return (decibel - minDecibel) / (maxDecibel - minDecibel);
    }
    void OnClickRightButton(Sound type)
    {
        switch (type)
        {
            case Sound.Master:
                _masterVolumeImage.fillAmount += 0.1f;
                SetVolume(_masterVolumeImage.fillAmount, type);
                break;
            case Sound.BGM:
                _bgmVolumeImage.fillAmount += 0.1f;
                SetVolume(_bgmVolumeImage.fillAmount, type);
                break;
            case Sound.SFX:
                _sfxVolumeImage.fillAmount += 0.1f;
                SetVolume(_sfxVolumeImage.fillAmount, type);
                break;
        }

    }
    void OnClickVolumeLeftButton(Sound type)
    {
        switch (type)
        {
            case Sound.Master:
                _masterVolumeImage.fillAmount -= 0.1f;
                SetVolume(_masterVolumeImage.fillAmount, type);
                break;
            case Sound.BGM:
                _bgmVolumeImage.fillAmount -= 0.1f;
                SetVolume(_bgmVolumeImage.fillAmount, type);
                break;
            case Sound.SFX:
                _sfxVolumeImage.fillAmount -= 0.1f;
                SetVolume(_sfxVolumeImage.fillAmount, type);
                break;
        }
    }

    public void SetVolume(float volume, Sound type)
    {

        float newVolume = volume <= 0.0001f ? -80 : -40 + 50 * volume;
        _audioMixer.SetFloat(Enum.GetName(typeof(Sound), type), newVolume);
    }
    #endregion

    public void OverrideCancelButtonEvent(UnityAction evt)
    {
        _cancelButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.AddListener(evt);
    }
}
