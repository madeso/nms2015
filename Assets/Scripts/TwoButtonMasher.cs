using UnityEngine;
using System.Collections;

public class TwoButtonMasher : MonoBehaviour, Masher {

	public KeyCode LeftKeyCode = KeyCode.W;
	public KeyCode RightKeyCode = KeyCode.S;
	
	private Tweaks tweaks;

	private float timer_;
	private float last_timer_ = 0.0f;
	private bool expecting_left_ = false;
	private float value_ = 0.0f;

	void Masher.Clear() {
		this.value_ = 0;
		this.expecting_left_ = false;
		this.last_timer_ = 0;
		this.timer_ = 0;
	}

	// Use this for initialization
	void Start () {
		this.tweaks = Tweaks.Find();
		this.timer_ = 0.0f;
	}

	float Masher.GetValue() {
		return value_;
	}

	public float RankDelta (float d, float i)
	{
		var delta = Mathf.Abs(d);
		var interval = Mathf.Abs(i);
		var beatmatch = this.tweaks.BeatMatch.Evaluate(interval);
		var speedinterval = this.tweaks.SpeedInterval.Evaluate(delta);

		// Debug.Log(string.Format("delta={0}, interval={1}, beatmatch={2}, imatch={3}", delta, interval, beatmatch, speedinterval));
		return beatmatch * speedinterval;
	}
	
	// Update is called once per frame
	void Update () {
		this.timer_ += Time.deltaTime;
		this.value_ -= Time.deltaTime * this.tweaks.DescreaseScale;
		if( value_ < 0 ) value_ = 0;
		var down = expecting_left_ ? Input.GetKeyDown(this.LeftKeyCode) : Input.GetKeyDown(this.RightKeyCode);
		if( down == false ) return;
		var delta = Mathf.Abs(timer_)-Mathf.Abs(last_timer_);
		this.value_ += this.RankDelta(timer_, delta) * this.tweaks.SpeedScale;
		if( this.value_ > this.tweaks.MaxSpeed ) {
			this.value_ = this.tweaks.MaxSpeed;
			// Debug.Log("Hit roof");
		}
		last_timer_ = this.timer_;
		expecting_left_ = !expecting_left_;
		this.timer_ = 0;
	}
}
