using UnityEngine;
using System.Collections;

public class GlobalTwoButtonTweaks : MonoBehaviour {

	public float RankDelta (float d, float i)
	{
		var delta = Mathf.Abs(d);
		var interval = Mathf.Abs(i);
		var beatmatch = this.BeatMatch.Evaluate(interval);
		var speedinterval = this.SpeedInterval.Evaluate(delta);
		Debug.Log(string.Format("delta={0}, interval={1}, beatmatch={2}, imatch={3}", delta, interval, beatmatch, speedinterval));
		return beatmatch * speedinterval;
	}

	public AnimationCurve BeatMatch; /// how good you match the beat
	public AnimationCurve SpeedInterval; /// how fast you move per beat
}
