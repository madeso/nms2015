using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour {

	private float position_ = 0;

	int index_;

	Masher masher;

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
		position_ += this.masher.GetValue() * Time.deltaTime;
		this.transform.position = this.players_.GetStartPosition(this.index_) + Vector3.right * position_;
	}

	private Players players_;
}
