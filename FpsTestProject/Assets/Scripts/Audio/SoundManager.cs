using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour,IListener
{
    private static SoundManager _instance;

    [SerializeField] private AudioTrack[] _tracks;
    public static SoundManager Instance
    {
        get => _instance;
    }

    private Hashtable _audioTable;
    private Hashtable _jobTable;

    private void Awake()
    {
        if (_instance==null)
        {
            _instance = this;

            _audioTable = new Hashtable();
            _jobTable = new Hashtable();
            GenerateAudioTable();
        }
    }

    private void Start()
    {
        Debug.Log("Содержание массива:" + _tracks.Length);
        EventManager.Instance.AddListener(Event_Type.Weapon_Reload1, this);
        EventManager.Instance.AddListener(Event_Type.Weapon_Reload2, this);
        EventManager.Instance.AddListener(Event_Type.Weapon_Reload3, this);
        EventManager.Instance.AddListener(Event_Type.Weapon_Reload4, this);
        EventManager.Instance.AddListener(Event_Type.Weapon_Reload5, this);
    }

    #region - Settings Sound Manager Methods

    private void Dispose()
    {
        if (_jobTable == null)
            return;

        foreach(DictionaryEntry dictionaryEntry in _jobTable)
        {
            IEnumerator job = (IEnumerator)dictionaryEntry.Value;
            StopCoroutine(job);
        }
    }

    private void GenerateAudioTable()
    {
        Debug.Log("1");

        foreach(AudioTrack track in _tracks)
        {
            Debug.Log("2");
            foreach(AudioObject audioObject in track.AudioObjects)
            {
                if (_audioTable.ContainsKey(audioObject.AudioType))
                {
                    LogWarning("Ключ уже содержится");
                }
                else
                {
                    _audioTable.Add(audioObject.AudioType, track);
                    Log("Звук добавлен в таблицу");
                }
            }
        }
    }

    private void AddJob(AudioJob audioJob)
    {
        RemoveConflictingJobs(audioJob.AudioType);

        IEnumerator jobRunner = RunAudioJob(audioJob);
        _jobTable.Add(audioJob.AudioAction, jobRunner);
        StartCoroutine(jobRunner);
    }

    private void RemoveConflictingJobs(AudioType type)
    {
        if (_jobTable.ContainsKey(type))
        {
            RemoveJob(type);
        }

        AudioType conflictAudio = AudioType.Noene;

        foreach(DictionaryEntry entry in _jobTable)
        {
            AudioType audioType = (AudioType)entry.Key;
            AudioTrack audioTrackInUse = (AudioTrack)_audioTable[audioType];
            AudioTrack audioTrackNeeded = (AudioTrack)_audioTable[type];

            if(audioTrackInUse.AudioSource == audioTrackNeeded.AudioSource)
            {
                conflictAudio = audioType;
            }
        }

        if(conflictAudio != AudioType.Noene)
        {
            RemoveJob(conflictAudio);
        }
    }

    private void RemoveJob(AudioType type)
    {
        if (!_jobTable.ContainsKey(type))
        {
            LogWarning("Ключ отсутствует");
            return;
        }

        IEnumerator jobRunner = (IEnumerator)_jobTable[type];
        StopCoroutine(jobRunner);
        _jobTable.Remove(type);
    }

    private IEnumerator RunAudioJob(AudioJob audioJob)
    {
        AudioTrack track = (AudioTrack)_audioTable[audioJob.AudioType];
        track.AudioSource.clip = GetAudioClipFromAudioTrack(audioJob.AudioType, track);

        switch (audioJob.AudioAction)
        {
            case AudioAction.Play:
                track.AudioSource.Play();
                break;

            case AudioAction.PlayOneShot:
                track.AudioSource.PlayOneShot(track.AudioSource.clip);
                break;

            case AudioAction.Stop:
                track.AudioSource.Stop();
                break;

            case AudioAction.Restart:
                track.AudioSource.Stop();
                track.AudioSource.Play();
                break;

            default:
                LogWarning("Такого действия нет!");
                break;

        }

        _jobTable.Remove(audioJob.AudioAction);

        yield return null;
    }

    private AudioClip GetAudioClipFromAudioTrack(AudioType audioType,AudioTrack audioTrack)
    {
        foreach(AudioObject audioObject in audioTrack.AudioObjects)
        {
            if (audioObject.AudioType == audioType)
                return audioObject.AudioClip;
        }

        return null;
    }
    #endregion


    #region - General Methods -

    public void PlayAudio(AudioType type)
    {
        AudioJob job = new AudioJob();
        job.AudioAction = AudioAction.Play;
        job.AudioType = type;

        AddJob(job);
    }

    public void PlayOneShot(AudioType type)
    {
        AudioJob job = new AudioJob();
        job.AudioAction = AudioAction.PlayOneShot;
        job.AudioType = type;

        AddJob(job);
    }

    public void StopAudio(AudioType type)
    {
        AudioJob job = new AudioJob();
        job.AudioAction = AudioAction.Stop;
        job.AudioType = type;

        AddJob(job);
    }

    public void PauseAudio(AudioType type)
    {

    }

    public void RestartAudio(AudioType type)
    {
        AudioJob job = new AudioJob();
        job.AudioAction = AudioAction.Restart;
        job.AudioType = type;

        AddJob(job);
    }

    #endregion


    #region - ILIstener - 

    public void OnEvent(Event_Type eventType, Component sender, UnityEngine.Object param = null)
    {
        switch (eventType)
        {
            case Event_Type.Weapon_Reload1:
                PlayOneShot(AudioType.Weapon_Reload1);
                break;

            case Event_Type.Weapon_Reload2:
                PlayOneShot(AudioType.Weapon_Reload2);
                break;

            case Event_Type.Weapon_Reload3:
                PlayOneShot(AudioType.Weapon_Reload3);
                break;

            case Event_Type.Weapon_Reload4:
                PlayOneShot(AudioType.Weapon_Reload4);
                break;

            case Event_Type.Weapon_Reload5:
                PlayOneShot(AudioType.Weapon_Reload5);
                break;

            case Event_Type.Weapon_Reload6:
                PlayOneShot(AudioType.Weapon_Reload6);
                break;

            default:
                LogWarning("Такого типа нет!");
                break;

        }
    }

    #endregion


    #region - Debug Methods

    private void Log(string msg)
    {
        Debug.Log("[Sound Manager]: " + msg);
    }

    private void LogWarning(string msg)
    {
        Debug.LogWarning("[Sound Manager]: " + msg);
    }

    #endregion


    #region - OnDisable/Enable -

    private void OnDisable()
    {
        Dispose();
        EventManager.Instance.RemoveListener(this);
    }

    private void OnEnable()
    {
        
    }
    #endregion


    #region - Audio Classes -

    [Serializable]
    public class AudioObject
    {
        [SerializeField] private AudioType _audioType;
        [SerializeField] private AudioClip _audioClip;

        public AudioType AudioType
        {
            get => _audioType;
        }
        public AudioClip AudioClip
        {
            get => _audioClip;
        }
    }

    [Serializable]
    public class AudioTrack
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioObject[] _audioObjects;

        public AudioSource AudioSource
        {
            get => _audioSource;
        }
        public AudioObject[] AudioObjects
        {
            get => _audioObjects;
        }
    }

    private class AudioJob
    {
        private AudioAction _audioAction;
        private AudioType _audioType;

        public AudioAction AudioAction
        {
            get => _audioAction;
            set => _audioAction = value;
        }

        public AudioType AudioType
        {
            get => _audioType;
            set => _audioType = value;
        }
    }

    private enum AudioAction
    {
        None,
        Play,
        PlayOneShot,
        Stop,
        Pause,
        Restart
    }

    #endregion
}
