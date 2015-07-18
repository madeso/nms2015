using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Flag : MonoBehaviour
{
	void Start ()
	{
		transform.DOScaleX(.7f, 2f).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(0f, .3f));
	}
}
