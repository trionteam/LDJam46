using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public static Score Instance = null;

    [SerializeField]
	private Text _numZombiesText;

	private HashSet<Zombifiable> zombifiables = new HashSet<Zombifiable>();

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		if (null == _numZombiesText)
		{
			_numZombiesText = GameObject.Find("TextNumZombies").GetComponent<Text>();
		}
		Debug.Assert(_numZombiesText != null);
	}

	public void UpdateUI()
	{
		if (null == _numZombiesText)
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
		_numZombiesText.text = $"Z: {numZ} N: {numN}";
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

