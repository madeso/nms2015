using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Crowd : MonoBehaviour
{
	public Sprite[] crowdSprites;
	public Color[] colors;
	public GameObject[] crowdGOs;
	public bool[] moving;

	void Start ()
	{
		crowdGOs = new GameObject[3*10];
		moving = new bool[3*10];

		int spriteId = 0;

		for(int r=0; r<3; r++)
		{
			for(int l=0; l<10; l++)
			{
				int spriteNo = l%4;
				GameObject newCrowdComponent = new GameObject("CrowdMember" + l, typeof(SpriteRenderer));
				newCrowdComponent.GetComponent<SpriteRenderer>().sprite = crowdSprites[spriteNo];
				newCrowdComponent.GetComponent<SpriteRenderer>().sortingLayerName = "Crowd";
				newCrowdComponent.GetComponent<SpriteRenderer>().sortingOrder = -(l*r);
				newCrowdComponent.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, 4)] * Random.Range(1f, 1.6f);
				newCrowdComponent.transform.SetParent(this.transform, false);
				newCrowdComponent.transform.localPosition = new Vector3(l*.6f, r*.8f, 0f);

				crowdGOs[spriteId] = newCrowdComponent;
				moving[spriteId] = false;
				spriteId++;
			}
		}
	}
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			ExciteCrowd();
		}
	}

	private void ExciteCrowd()
	{
		for(int i=0; i<crowdGOs.Length; i++)
		{
			if(moving[i] == false)
			{
				crowdGOs[i].transform.DOPunchPosition(new Vector3(0f, .3f, 0f), 1f, 2).SetDelay(Random.Range(0f, .5f));
				//moving[i] = true;
			}
		}
	}
}
