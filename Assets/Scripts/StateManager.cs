using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent (typeof(AudioManager))]
public class StateManager : Singleton<StateManager>
{
	public GameStates state;

	private GameObject poster, title;

	private void Start()
	{
		AudioManager.Instance.PlayMusic();
		
		title = GameObject.Find("Title");
		poster = GameObject.Find("Poster");
		poster.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			switch(state)
			{
			case GameStates.Title:
				title.SetActive(false);
				poster.GetComponent<SpriteRenderer>().DOFade(1f, 1f);
				state = GameStates.Poster;
				break;
			case GameStates.Poster:
				Application.LoadLevel("main");
				break;
			case GameStates.Gameplay:

				break;
			case GameStates.EndGameplay:
				Application.LoadLevel("title");
				break;
			}
		}
	}

	public enum GameStates
	{
		Title,
		Poster,
		Gameplay,
		EndGameplay,
	}
}
