using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SoundMgr : MonoBehaviour
{
	public static SoundMgr Instance;
	private class CachedAudio
	{
		public AudioSource audio;
		public float lastPlayed;
	};
	private static Dictionary<string, CachedAudio> Clips = new Dictionary<string, CachedAudio>();
	private static AudioSource music = null;
	public static bool ENABLED = true;
	public static bool MUSIC = true;
	public static string SoundsRoot = "Sounds";
	public static string MusicClip = "Plague_of_Zombies";
	public float MusicVolume = 0.2f;

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

		ENABLED = true;
		MUSIC = true;
		music?.Play();
	}

	public void Toggle(UnityEngine.UI.Button button = null)
	{
		ENABLED = !ENABLED;
		Debug.Log($"Sounds {(ENABLED ? "ON" : "OFF")}");

		if (button)
		{
			// TODO - hardcoded color
			button.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = ENABLED ? new Color(0, 0.5f, 0): Color.red;
		}
	}

	public void ToggleMusic(UnityEngine.UI.Button button = null)
	{
		MUSIC = !MUSIC;
		Debug.Log($"Music {(MUSIC ? "ON" : "OFF")}");

		if (button)
		{
			// TODO - hardcoded color
			button.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = MUSIC ? new Color(0, 0.5f, 0) : Color.red;
		}

		if (!MUSIC) music?.Stop();
		else music?.Play();
	}

	public void Play(string name)
	{
		if (!ENABLED)
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
}
