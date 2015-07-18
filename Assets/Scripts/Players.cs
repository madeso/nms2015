using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Assertions;

public class Players : MonoBehaviour {

	public PlayerPosition[] PlayerList;
	public GameObject[] StartPositions;

	public GameObject End;

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

	public int Move (PlayerPosition player, int track_change)
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

	public List<PlayerPosition>[] players_on_track_;

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

	void Start() {
		//UnityEngine.Assert.AreEqual
		AssertAreEqual(this.StartPositions.Length, this.PlayerList.Length);
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

	private static void AssertAreEqual (int i, int i2)
	{
		if( i != i2 ) throw new UnityException("Assert failure");
	}
}
