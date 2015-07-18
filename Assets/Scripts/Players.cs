using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Assertions;

/// <summary>
///  Global object tracking the players and game logic things
/// </summary>
using UnityEngine.Assertions;


public class Players : MonoBehaviour {

	public PlayerPosition[] PlayerList;
	public GameObject[] StartPositions;
	public Eyes[] Judges;

	public static Players Find() {
		var ch = GameObject.Find("Characters");
		var pl = ch.GetComponent<Players>();
		return pl;
	}

	public GameObject End;

	// number of seconds that have elapsed since no input has been given
	// reset game after some time have elapsed, 30 seconds?
	private float idle_timer_ = 0;

	public List<PlayerPosition>[] players_on_track_;

	public Vector3 GetStartPosition (int index_)
	{
		return this.StartPositions[index_].transform.position;
	}

	private List<PlayerPosition> winners_ = new List<PlayerPosition>();

	public bool FeedPosition (PlayerPosition p)
	{
		if( p.transform.position.x >= this.End.transform.position.x ) {
			if( winners_.Contains(p) == false ) {
				winners_.Add(p);
				Debug.Log(string.Format("{0} won!", p.name));
			}
			return true;
		}

		return winners_.Count > 0;
	}

	public int ChangeTrackForPlayer (PlayerPosition player, int track_change)
	{
		int next_track = GetNextTrack(player.track_index, track_change);
		var track = players_on_track_[next_track];
		foreach(var other_player in track) {
			if( PlayerPosition.IsOverlapping(player, other_player) ) {
				return player.track_index;
			}
		}
		return next_track;
	}

	public void OnPlayerMoved (PlayerPosition player)
	{
		this.idle_timer_ = 0;
		foreach(var p in players_on_track_[player.track_index]) {
			if( p != player && PlayerPosition.IsOverlapping(p, player) ) {
				p.GetPushed();
			}
		}
	}

	public void NotifyNewTrack (PlayerPosition player, int track_index)
	{
		if( player.track_index == track_index ) {
			// Debug.Log("Same index, not changing track");
			return;
		}
		foreach(var track in this.players_on_track_) {
			track.Remove(player);
		}
		this.players_on_track_[track_index].Add(player);
		// Debug.Log(string.Format ("Moved {0} to track {1}", player.name, track_index+1));
	}

	private int GetNextTrack (int current_track, int track_change)
	{
		var next_track = current_track + track_change;
		if( next_track < 0 ) return 0;
		var total_number_of_tracks = StartPositions.Length;
		if( next_track >= total_number_of_tracks) return total_number_of_tracks-1;
		return next_track;
	}

	public int GetNumberOfPlayers ()
	{
		return this.PlayerList.Length;
	}
	private Tweaks tweaks_;
	public void Start() {
		this.tweaks_ = Tweaks.Find();
		Assert.AreEqual(this.StartPositions.Length, this.PlayerList.Length);
		this.players_on_track_ = new List<PlayerPosition>[this.PlayerList.Length];
		for(int i=0; i<this.PlayerList.Length; ++i) {
			this.players_on_track_[i] = new List<PlayerPosition>();
		}
		int index = 0;
		foreach(var pl in PlayerList) {
			pl.Setup(this, index);
			++index;
		}
	}

	public void Update() {
		this.idle_timer_ += Time.deltaTime;

		foreach(var pl in this.PlayerList) {
			pl.is_detected = false;
		}

		foreach(var judge in this.Judges) {
			foreach(var dp in judge.detected_players) {
				dp.is_detected = true;

				if( dp.IsBeeingPenalized == false ) {
					var speed = dp.GetSpeed();
					if( speed > this.tweaks_.SpeedLimmit ) {
						Debug.Log(string.Format("{0} penalized {1}", dp.name, speed));
						dp.Penalize();
					}
					else {
						Debug.Log (dp.name + " ok");
					}
				}
			}
		}
	}
}
