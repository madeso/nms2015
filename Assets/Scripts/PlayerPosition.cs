using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour {

	private float position_on_track_ = 0;

	private int track_index_;

	private Masher masher_;

	public KeyCode UpKey;
	public KeyCode DownKey;

	public float Position {
		get {
			return this.position_on_track_;
		}
	}

	public void Setup (Players players, int index)
	{
		this.players_ = players;
		this.track_index_ = index;
	}

	void Start() {
		this.masher_ = this.GetComponent<Masher>();
		if( this.masher_ == null ) throw new UnityException("no masher component on object");
	}

	public void Update() {
		if( this.players_.FeedPosition(this) ) return;

		if( Input.GetKeyUp(this.UpKey) ) {
			MoveUp(1);
		}
		if( Input.GetKeyUp(this.DownKey) ) {
			MoveUp(-1);
		}
		position_on_track_ += this.masher_.GetValue() * Time.deltaTime;
		this.transform.position = this.players_.GetStartPosition(this.track_index_) + Vector3.right * position_on_track_;
	}

	void MoveUp (int i)
	{
		this.track_index_ = this.players_.Move(track_index_, i);
	}

	private Players players_;
}
