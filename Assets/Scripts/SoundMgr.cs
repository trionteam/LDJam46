using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class SoundMgr : MonoBehaviour
{
    public static readonly string SoundEnabledKey = "SoundEnabled";
    public static readonly string MusicEnabledKey = "MusicEnabled";

    public static SoundMgr Instance;
    private class CachedAudio
    {
        public AudioSource audio;
        public float lastPlayed;
    };
    private static Dictionary<string, CachedAudio> Clips = new Dictionary<string, CachedAudio>();
    private static AudioSource music = null;
    public static string SoundsRoot = "Sounds";
    public static string MusicClip = "Plague_of_Zombies";
    public float MusicVolume = 0.2f;

    public static bool MusicEnabled
    {
        get => PlayerPrefs.GetInt(MusicEnabledKey, 1) > 0;
        set => PlayerPrefs.SetInt(MusicEnabledKey, value ? 1 : 0);
    }

    public static bool SoundEnabled
    {
        get => PlayerPrefs.GetInt(SoundEnabledKey, 1) > 0;
        set => PlayerPrefs.SetInt(SoundEnabledKey, value ? 1 : 0);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Clips.Clear();

        AudioClip ac = Resources.Load<AudioClip>(SoundsRoot + "/" + MusicClip);
        if (ac)
        {
            music = new GameObject().AddComponent<AudioSource>();
            music.clip = ac;
            music.loop = true;
            music.volume = MusicVolume;
        }

        if (MusicEnabled) music?.Play();

        UpdateButtons();
    }

    public void OnEnable()
    {
        UpdateButtons();
    }

    public void Toggle(UnityEngine.UI.Button button = null)
    {
        SoundEnabled = !SoundEnabled;
        Debug.Log($"Sounds {(SoundEnabled ? "ON" : "OFF")}");

        UpdateButtons();
    }

    public void ToggleMusic(UnityEngine.UI.Button button = null)
    {
        MusicEnabled = !MusicEnabled;
        Debug.Log($"Music {(MusicEnabled ? "ON" : "OFF")}");

        UpdateButtons();

        if (!MusicEnabled) music?.Stop();
        else music?.Play();
    }

    public void Play(string name)
    {
        if (!SoundEnabled)
        {
            return;
        }
        CachedAudio ca = null;
        if (!Clips.ContainsKey(name))
        {
            AudioClip ac = Resources.Load<AudioClip>(SoundsRoot + "/" + name);
            if (ac)
            {
                AudioSource g = new GameObject().AddComponent<AudioSource>();
                g.clip = ac;
                ca = new CachedAudio();
                ca.audio = g;
                ca.lastPlayed = 0;
                Clips.Add(name, ca);
            }
        }
        else
        {
            ca = Clips[name];
        }
        if (null != ca && ca.audio && ca.audio.clip && Time.time > ca.lastPlayed + ca.audio.clip.length)
        {
            ca.audio.Play();
            ca.lastPlayed = Time.time;
        }
    }

    private void UpdateButtons()
    {
        foreach (var button in GameObject.FindGameObjectsWithTag("ToggleSoundButton"))
        {
            button.GetComponentInChildren<Text>().color = SoundEnabled ? new Color(0, 0.5f, 0) : Color.red;
        }
        foreach(var button in GameObject.FindGameObjectsWithTag("ToggleMusicButton"))
        {
            button.GetComponentInChildren<Text>().color = MusicEnabled ? new Color(0, 0.5f, 0) : Color.red;
        }
    }
}
