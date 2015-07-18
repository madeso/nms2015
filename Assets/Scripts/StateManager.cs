using UnityEngine;
using System.Collections;


[RequireComponent (typeof(AudioManager))]
public class StateManager : Singleton<StateManager>
{
	private void Start()
	{
		AudioManager.Instance.PlayMusic();
	}
}
