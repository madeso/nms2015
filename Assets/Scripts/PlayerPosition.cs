using UnityEngine;
using System.Collections;

/// <summary>
/// Class containing the player data
/// </summary>
public class PlayerPosition : MonoBehaviour {


	// true if the player is detected, false if not
	public bool is_detected;

	
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
	
	private Tweaks tweaks;
	
	private float push_timer_ = 0.0f;
	
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
	private bool penalized_ = false;
	public void Penalize ()
	{
		this.penalized_ = true;
	}

	public bool IsBeeingPenalized {
		get {
			return this.penalized_;
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

	private float push_speed {
		get {
			if( this.push_timer_ <= 0 ) return 0;
			return this.tweaks.PushSpeed * (this.push_timer_/this.tweaks.PushTime);
		}
	}
	
	public void Update() {
		if( this.penalized_ ) {
			this.position_on_track_ -= this.tweaks.PenalizedSpeed * Time.deltaTime;
			if( this.position_on_track_ < 0 ) {
				this.position_on_track_ = 0;
				this.push_timer_ = 0;
				this.penalized_ = false;
			}
		}
		else {
			if( this.players_.FeedPosition(this) ) return;
			
			if( Input.GetKeyUp(this.UpKey) ) {
				ChangeTrack(1);
			}
			if( Input.GetKeyUp(this.DownKey) ) {
				ChangeTrack(-1);
			}
			position_on_track_ += this.masher_.GetValue() * Time.deltaTime;
			if( this.push_timer_ > 0 ) {
				this.position_on_track_ += this.push_speed * Time.deltaTime;
				this.push_timer_ -= Time.deltaTime;
				if( this.push_timer_ <= 0 ) {
					this.push_timer_ = 0;
				}
			}
			this.players_.OnPlayerMoved(this);
		}

		// update graphics based on the position
		this.transform.position = this.players_.GetStartPosition(this.track_index) + Vector3.right * position_on_track_;
	}
	
	public void GetPushed ()
	{
		this.push_timer_ += this.tweaks.PushTime;
	}

	public float GetSpeed() {
		return this.masher_.GetValue() + this.push_speed;
	}

	
	// direction is either -1 or +1 depending on the direction
	void ChangeTrack (int direction)
	{
		this.track_index = this.players_.ChangeTrackForPlayer(this, direction);
	}
	
	private Players players_;
}
