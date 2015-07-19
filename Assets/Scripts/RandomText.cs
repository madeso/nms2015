using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomText : MonoBehaviour {
	public Text Text;
	public float timer_ = 0;

	// Use this for initialization
	void Start () {
		if( this.Text == null ) {
			this.Text = GetComponentInChildren<Text>();
		}
		this.timer_ = -2;
	}

	float TimeToNextText ()
	{
		return Random.Range(3, 12);
	}

	float TimeToRead ()
	{
		return Random.Range(3,5);
	}

	string Select(params string[] data) {
		return data[Random.Range(0, data.Length)];
	}

	string GenerateRandomString ()
	{
		return Select("Obey", "Dictator-sloth rules!", "Do not break the\nspeed limit", "Consume!!",
		              "Marry and reproduce!", "#plutoflybye", "Mega welcome!!!", "Magnetic sausages?",
		              "No independent\nthought!", "Sleep 8 hours!", "Work 8 hours!", "Play 8 hours!",
		              "Buy!", "Stay asleep!", "Submit!", "Do not question\nauthority!", "Obey and conform",
		              "Watch T.V", "No imagination!", "OBEY!!!", "#nomoresweden", "Obey the sloths", "The sloths are\nyour friends"
		              );
	}
	
	// Update is called once per frame
	void Update () {
		this.timer_ -= Time.deltaTime;
		if( this.timer_ <= 0 ) {
			if( this.Text.text.Length > 0 ) {
				this.Text.text = string.Empty;
				this.timer_ += TimeToNextText();
			}
			else {
				this.Text.text = GenerateRandomString();
				this.timer_ += TimeToRead();
			}
		}
	}
}
