using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public static Score Instance = null;
	public Text NumZombiesText;

	private HashSet<Zombifiable> zombifiables = new HashSet<Zombifiable>();

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		if (null == NumZombiesText)
		{
			NumZombiesText = GameObject.Find("TextNumZombies").GetComponent<Text>();
		}
		Debug.Assert(NumZombiesText != null);
	}

	public void UpdateUI()
	{
		if (null == NumZombiesText)
		{
			return;
		}
		int numZ = 0;
		int numN = 0;
		int numI = 0;
		foreach (Zombifiable z in zombifiables)
		{
			switch(z.CurrentState)
			{
				case Zombifiable.State.Zombie: numZ++; break;
				case Zombifiable.State.Normal: numN++; break;
				case Zombifiable.State.Immune: numI++; break;
			}
		}
		NumZombiesText.text = $"Z: {numZ} N: {numN}";
	}

	public void RegisterActor(Zombifiable z)
	{
		zombifiables.Add(z);
		UpdateUI();
	}

	public void UnregisterActor(Zombifiable z)
	{
		zombifiables.Remove(z);
		UpdateUI();
	}
}

