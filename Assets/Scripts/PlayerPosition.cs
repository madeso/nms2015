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
	
	public int donottouch_track_index_ = -1;
	public int track_index {
		get {
			return this.donottouch_track_index_;
		}
		private set {
			// Debug.Log (string.Format("Changing track index from {0} to {1}", this.track_index_, value));
			this.players_.NotifyNewTrack(this, value);
			this.donottouch_track_index_ = value;
			this.sprite_renderer_.sortingOrder = this.players_.GetNumberOfPlayers() - this.track_index;
		}
	}
	
	private Masher masher_;
	
	public KeyCode UpKey;
	public KeyCode DownKey;
	
	private Tweaks tweaks_;
	
	private float push_timer_ = 0.0f;
	
	public float Position {
		get {
			return this.position_on_track_;
		}
	}

	public static Rect CalculatePseudoRect (float postition_on_track, Tweaks tweaks)
	{
		return new Rect (postition_on_track - tweaks.PlayerWidth / 2.0f, 0, tweaks.PlayerWidth, 1);
	}

	
	public Rect PseudoRect {
		get {
			return CalculatePseudoRect (this.position_on_track_, this.tweaks_);
		}
	}
	private bool penalized_ = false;
	public void Penalize ()
	{
		this.penalized_ = true;
		PenalizeClearStartPosition ();
	}

	void PenalizeClearStartPosition ()
	{
		this.masher_.Clear ();
		this.push_timer_ = -1;
		this.track_index = this.players_.FindFirstFreeTrack (this.track_index, this.start_track_index_);
	}

	public bool IsBeeingPenalized {
		get {
			return this.penalized_;
		}
	}

	int start_track_index_ = -1;
	
	public static bool IsOverlapping (PlayerPosition left, PlayerPosition right)
	{
		return left.PseudoRect.Overlaps(right.PseudoRect);
	}
	
	public void Setup (Players players, int index)
	{
		this.sprite_renderer_ = this.GetComponent<SpriteRenderer>();
		this.tweaks_ = Tweaks.Find();
		this.players_ = players;
		this.track_index = index;
		this.start_track_index_ = index;
	}
	
	void Start() {
		this.masher_ = this.GetComponent<Masher>();
		if( this.masher_ == null ) throw new UnityException("no masher component on object");
	}

	private float push_speed {
		get {
			if( this.push_timer_ <= 0 ) return 0;
			return this.tweaks_.PushSpeed * (this.push_timer_/this.tweaks_.PushTime);
		}
	}
	
	public void Update() {
		if( this.penalized_ ) {
			this.position_on_track_ -= this.tweaks_.PenalizedSpeed * Time.deltaTime;
			if( this.position_on_track_ < 0 ) {
				this.position_on_track_ = 0;
				this.PenalizeClearStartPosition();
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
		this.push_timer_ += this.tweaks_.PushTime;
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
