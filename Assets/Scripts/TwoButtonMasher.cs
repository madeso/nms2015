using UnityEngine;
using System.Collections;

public class TwoButtonMasher : MonoBehaviour, Masher {

	public KeyCode LeftKeyCode = KeyCode.W;
	public KeyCode RightKeyCode = KeyCode.S;

	public AnimationCurve BeatMatch; /// how good you match the beat
	public AnimationCurve SpeedInterval; /// how fast you move per beat
	public float SpeedScale = 1; // how much you gean each press
	public float DescreaseScale = 1; /// how fast the value decrease
	public float MaxSpeed = 3;

	public float timer;
	public float lastDelta = 0.0f;
	private bool left = false;
	public float value = 0.0f;

	// Use this for initialization
	void Start () {
		this.timer = 0.0f;
	}

	float Masher.GetValue() {
		return value;
	}
	
	// Update is called once per frame
	void Update () {
		this.timer += Time.deltaTime;
		this.value -= Time.deltaTime * this.DescreaseScale;
		if( value < 0 ) value = 0;
		var down = left ? Input.GetKey(this.LeftKeyCode) : Input.GetKey(this.RightKeyCode);
		if( down == false ) return;
		var delta = Mathf.Abs(timer)-Mathf.Abs(lastDelta);
		this.value += RankDelta(timer, delta);
		if( this.value > this.MaxSpeed ) {
			this.value = this.MaxSpeed;
			Debug.Log("Hit roof");
		}
		lastDelta = delta;
		left = !left;
		this.timer = 0;
	}

	float RankDelta (float d, float i)
	{
		var delta = Mathf.Abs(d);
		var interval = Mathf.Abs(i);
		var beatmatch = this.BeatMatch.Evaluate(interval);
		var speedinterval = this.SpeedInterval.Evaluate(delta);
		Debug.Log(string.Format("delta={0}, interval={1}, beatmatch={2}, imatch={3}", delta, interval, beatmatch, speedinterval));
		return beatmatch * speedinterval * SpeedScale;
	}
}
