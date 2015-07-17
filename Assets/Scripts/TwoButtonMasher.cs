using UnityEngine;
using System.Collections;

public class TwoButtonMasher : MonoBehaviour, Masher {

	public KeyCode LeftKeyCode = KeyCode.W;
	public KeyCode RightKeyCode = KeyCode.S;


	public float SpeedScale = 1; // how much you gean each press
	public float DescreaseScale = 1; /// how fast the value decrease
	public float MaxSpeed = 3;
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
	
	// Update is called once per frame
	void Update () {
		this.timer_ += Time.deltaTime;
		this.value_ -= Time.deltaTime * this.DescreaseScale;
		if( value_ < 0 ) value_ = 0;
		var down = left_ ? Input.GetKey(this.LeftKeyCode) : Input.GetKey(this.RightKeyCode);
		if( down == false ) return;
		var delta = Mathf.Abs(timer_)-Mathf.Abs(lastDelta_);
		this.value_ += this.Tweak.RankDelta(timer_, delta) * SpeedScale;
		if( this.value_ > this.MaxSpeed ) {
			this.value_ = this.MaxSpeed;
			Debug.Log("Hit roof");
		}
		lastDelta_ = delta;
		left_ = !left_;
		this.timer_ = 0;
	}
}
