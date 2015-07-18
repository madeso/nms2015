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

	public int Move (int p, int i)
	{
		var np = p + i;
		if( np < 0 ) return 0;
		var s = StartPositions.Length;
		if( np >= s) return s-1;
		return np;
	}

	void Start() {
		//UnityEngine.Assert.AreEqual
		AssertAreEqual(this.StartPositions.Length, this.PlayerList.Length);
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
