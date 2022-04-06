using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _audioMixer;

    private Func<string, AudioClip> _AudioClipLoader;

    private long _nextAudioID = 1;

    private Dictionary<long, AudioPlayEntry> _audioEntryDic = new Dictionary<long, AudioPlayEntry>();

    private List<AudioPlayEntry> _audioEntryInFading = new List<AudioPlayEntry>();
    private List<AudioPlayEntry> _audioEntryWaitFinish = new List<AudioPlayEntry>();


    public AudioManager Instance
    {
        get => _instance;
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(this);
        }
    }


    #region - Nested Struct AudioPlayEntry -
    public struct AudioPlayEntry
    {
        [SerializeField] private long _id;
        [SerializeField] private string _soundName;
        [SerializeField] private string _mixerGroup;
        [SerializeField] private bool _loop;
        [SerializeField] private float _minimumDistance;
        [SerializeField] private float _maximumDistance;
        [SerializeField] private float _fadeTime;
        [SerializeField] private GameObject _objectToPlay;
        [SerializeField] private bool _isPoolGameObject;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _oneFadeTimeDiv;
        [SerializeField] private float _fadeTimer;

        public bool Is3D
        {
            get
            {
                return _minimumDistance > 0 && _maximumDistance > 0;
            }
        }
    }

    #endregion
}
