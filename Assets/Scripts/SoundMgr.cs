using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SoundMgr : MonoBehaviour
{
	public SoundMgr Instance;
	private class CachedAudio
	{
		public AudioSource audio;
		public float lastPlayed;
	};
	private static Dictionary<string, CachedAudio> Clips = new Dictionary<string, CachedAudio>();
	//AudioSource music = null;
	public static bool ENABLED = true;
	public static string SoundsRoot = "Sounds";
	//public static string MusicRoot = "Music";

	private void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		Clips.Clear();

		//AudioClip ac = Resources.Load<AudioClip>(MusicRoot + "/" + "abc");
		//if (ac)
		//{
		//	music = new GameObject().AddComponent<AudioSource>();
		//	music.clip = ac;
		//	music.loop = true;
		//}

		ENABLED = true;
		//music.Play();
	}

	public void Toggle()
	{
		ENABLED = !ENABLED;
	}

	public static void Play(string name)
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
