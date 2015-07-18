using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(AudioListener))]
public class AudioManager : Singleton<AudioManager>
{
	//from Inspector

	private AudioClip mainMusic;
	private AudioSource musicSource;
	private float musicSpeed;

	public enum SFX
	{
		Step,

	}

	private void Awake()
	{
		mainMusic = Resources.Load<AudioClip>("Audio/Chariots");

		musicSource = GetComponent<AudioSource>();
	}

	public void PlayMusic()
	{
		if(musicSource.clip == null)
		{
			Awake();
		}
		musicSource.clip = mainMusic;
		musicSource.Play();
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	public void PauseMusic()
	{
		musicSource.Pause();
	}

	public void UnpauseMusic()
	{
		musicSource.UnPause();
	}

	public void PlaySFX(SFX clip)
	{
		AudioClip sfxClip = Resources.Load<AudioClip>("Audio/" + clip.ToString());
		musicSource.PlayOneShot(sfxClip);
	}

	private void Update()
	{
		if(musicSource.isPlaying)
		{
			if(Input.GetKey(KeyCode.Space))
			{
				musicSpeed += Time.deltaTime;
			}
			else
			{
				musicSpeed -= Time.deltaTime;
			}

			musicSpeed = Mathf.Clamp(musicSpeed, .7f, 1.2f);

			musicSource.pitch = musicSpeed;
		}
	}
}
