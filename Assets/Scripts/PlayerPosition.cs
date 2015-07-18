using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour {

	public float position_on_track_ = 0;

	private SpriteRenderer sprite_renderer_;

	private int track_index_ = -1;
	public int track_index {
		get {
			return this.track_index_;
		}
		private set {
			// Debug.Log (string.Format("Changing track index from {0} to {1}", this.track_index_, value));
			this.players_.NotifyNewTrack(this, value);
			this.track_index_ = value;
			this.sprite_renderer_.sortingOrder = this.players_.GetNumberOfPlayers() - track_index_;
		}
	}

	private Masher masher_;

	public KeyCode UpKey;
	public KeyCode DownKey;

	Tweaks tweaks;

	public float Position {
		get {
			return this.position_on_track_;
		}
	}

	public Rect PseudoRect {
		get {
			return new Rect(position_on_track_ - this.tweaks.PlayerWidth/2.0f, 0, this.tweaks.PlayerWidth, 1);
		}
	}

	public static bool IsOverlapping (PlayerPosition left, PlayerPosition right)
	{
		return left.PseudoRect.Overlaps(right.PseudoRect);
	}

	public void Setup (Players players, int index)
	{
		this.sprite_renderer_ = this.GetComponent<SpriteRenderer>();
		this.tweaks = Tweaks.Find();
		this.players_ = players;
		this.track_index = index;
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
		this.transform.position = this.players_.GetStartPosition(this.track_index) + Vector3.right * position_on_track_;
	}

	void MoveUp (int i)
	{
		this.track_index = this.players_.Move(this, i);
	}

	private Players players_;
}
