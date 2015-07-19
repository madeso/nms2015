using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VisibleIfHasText : MonoBehaviour {

	public Text TextToCheck;

	private CanvasRenderer renderer_;

	public void Start() {
		if( this.TextToCheck == null ) {
			TextToCheck = this.gameObject.GetComponentInParent<Text>();
		}
		this.renderer_ = this.GetComponent<CanvasRenderer>();
	}

	public void LateUpdate() {
		var text = this.TextToCheck.text;
		var has_text = text.Length > 0;
		renderer_.SetAlpha(has_text ? 1.0f : 0.0f);
	}
}
