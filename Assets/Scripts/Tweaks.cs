using UnityEngine;
using System.Collections;

public class Tweaks : MonoBehaviour {

	public static Tweaks Find() {
		var go = GameObject.Find("Characters");
		var tw = go.GetComponent<Tweaks>();
		return tw;
	}

	public AnimationCurve BeatMatch; /// how good you match the beat
	public AnimationCurve SpeedInterval; /// how fast you move per beat

	public float SpeedScale = 1; // how much you gain each press
	public float DescreaseScale = 1; /// how fast the value decrease
	public float MaxSpeed = 3;

	public float PlayerWidth = 3;
}
