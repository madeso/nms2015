using UnityEngine;
using System.Collections;
//using UnityEngine.Assertions;

public class Players : MonoBehaviour {

	public PlayerPosition[] PlayerList;
	public GameObject[] StartPositions;

	public Vector3 GetStartPosition (int index_)
	{
		return this.StartPositions[index_].transform.position;
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
