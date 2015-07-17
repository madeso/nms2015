using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour {

	private float position_ = 0;

	private int index_;

	private Masher masher;

	public KeyCode UpKey;
	public KeyCode DownKey;

	public float Position {
		get {
			return this.position_;
		}
	}

	public void Setup (Players players, int index)
	{
		this.players_ = players;
		this.index_ = index;
	}

	void Start() {
		this.masher = this.GetComponent<Masher>();
		if( this.masher == null ) throw new UnityException("no masher component on object");
	}

	public void Update() {
		if( this.players_.FeedPosition(this) ) return;

		if( Input.GetKeyUp(this.UpKey) ) {
			MoveUp(1);
		}
		if( Input.GetKeyUp(this.DownKey) ) {
			MoveUp(-1);
		}
		position_ += this.masher.GetValue() * Time.deltaTime;
		this.transform.position = this.players_.GetStartPosition(this.index_) + Vector3.right * position_;
	}

	void MoveUp (int i)
	{
		this.index_ = this.players_.Move(index_, i);
	}

	private Players players_;
}
