using UnityEngine;
using System.Collections;

public class TwoButtonMasher : MonoBehaviour, Masher {

	public KeyCode LeftKeyCode = KeyCode.W;
	public KeyCode RightKeyCode = KeyCode.S;



	public GlobalTwoButtonTweaks Tweak;

	public float timer_;
	public float lastDelta_ = 0.0f;
	private bool left_ = false;
	public float value_ = 0.0f;

	// Use this for initialization
	void Start () {
		this.timer_ = 0.0f;
	}

	float Masher.GetValue() {
		return value_;
	}

	public float interval;

	public float delta;

	public float beatmach;

	public float speedinterval;

	public float RankDelta (float d, float i)
	{
		var delta = Mathf.Abs(d);
		var interval = Mathf.Abs(i);
		var beatmatch = this.Tweak.BeatMatch.Evaluate(interval);
		var speedinterval = this.Tweak.SpeedInterval.Evaluate(delta);

		this.interval = interval;
		this.delta = delta;
		this.beatmach = beatmatch;
		this.speedinterval = speedinterval;

		Debug.Log(string.Format("delta={0}, interval={1}, beatmatch={2}, imatch={3}", delta, interval, beatmatch, speedinterval));
		return beatmatch * speedinterval;
	}
	
	// Update is called once per frame
	void Update () {
		this.timer_ += Time.deltaTime;
		this.value_ -= Time.deltaTime * this.Tweak.DescreaseScale;
		if( value_ < 0 ) value_ = 0;
		var down = left_ ? Input.GetKey(this.LeftKeyCode) : Input.GetKey(this.RightKeyCode);
		if( down == false ) return;
		var delta = Mathf.Abs(timer_)-Mathf.Abs(lastDelta_);
		this.value_ += this.RankDelta(timer_, delta) * this.Tweak.SpeedScale;
		if( this.value_ > this.Tweak.MaxSpeed ) {
			this.value_ = this.Tweak.MaxSpeed;
			Debug.Log("Hit roof");
		}
		lastDelta_ = this.timer_;
		left_ = !left_;
		this.timer_ = 0;
	}
}
