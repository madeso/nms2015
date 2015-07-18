using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Eyes : MonoBehaviour {
	public float VisionConeAngle = 45;
	public float movement_ = -90;
	private Players players_;


	private static float GetGroundDistance(float height, float angle) {
		return Mathf.Tan ( Mathf.Deg2Rad * angle) * height;
	}

	// Use this for initialization
	void Start () {
		this.players_ = Players.Find();
	}
	
	void Update () {
		int track = 0;
		float start_position_latest_ = 0;
		float end_position_latest_ = 0;
		float track_y_latest = 0;
		bool has = false;
		this.movement_ += Time.deltaTime;

		foreach(var pos in this.players_.StartPositions ) {
			var players = this.players_.players_on_track_[track];
			var track_y = pos.transform.position.y;
			var height = Mathf.Abs( track_y - this.transform.position.y );
			Assert.IsTrue(height>0);
			Debug.DrawLine(this.transform.position, pos.transform.position, Color.cyan);
			var movement = Mathf.Sin (this.movement_) * 45;
			var start_position = GetGroundDistance(height, movement);
			var end_position = GetGroundDistance(height, movement + this.VisionConeAngle);

			if( has == false ) {
				has = true;
				start_position_latest_ = Mathf.Min (start_position, end_position);
				end_position_latest_ = Mathf.Max (start_position, end_position);
				track_y_latest = track_y;
			}
			foreach(var pla in players) {
				CheckPlayer(pla, start_position, end_position);
			}
			++track;
		}

		UpdateVisionConeMesh(start_position_latest_, end_position_latest_, track_y_latest);
	}

	void CheckPlayer (PlayerPosition pla, float start, float end)
	{
	}

	public float start_ = 0;
	public float end_ = 0;
	public float y_ = 0;
	GameObject visionconeobject_;

	public Vector3 size_;

	public Material VisionConeMaterial;

	void CreateVisionCone ()
	{
		if (this.visionconeobject_ != null) return;

		this.visionconeobject_ = new GameObject ();
		this.visionconeobject_.name = this.name + "_VisionConeObject";
		this.visionconeobject_.AddComponent<MeshFilter> ();
		this.visionconeobject_.AddComponent<MeshRenderer> ();
		this.visionconeobject_.AddComponent<Mover> ();
		var renderer = this.visionconeobject_.GetComponent<MeshRenderer> ();
		renderer.material = this.VisionConeMaterial;
		renderer.receiveShadows = false;
		renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
	}

	void UpdateVisionConeMesh (float start, float end, float y)
	{
		CreateVisionCone ();
		this.start_ = start;
		this.end_ = end;
		this.y_ = y;
		var ex = this.gameObject.transform.position.x;
		var ey = this.gameObject.transform.position.y;
		var dx = -0.1f;
		var rndr = this.visionconeobject_.GetComponent<MeshRenderer>();
		var mesh = this.visionconeobject_.GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		var z = 5;
		mesh.vertices = new Vector3[] {
			new Vector3(ex-dx, ey, z), new Vector3(ex+dx, ey, z),
			new Vector3(start, y, z), new Vector3(end, y, z)
		};
		mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};
		mesh.triangles = new int[] {2, 1, 0, 3, 2, 0};
		var wi = ey;
		var wo = ex;
		this.size_ = rndr.bounds.size;
		this.visionconeobject_.GetComponent<Mover>().SetPosition( this.transform.position - new Vector3(wo,wi,0) + new Vector3(0,0,-10));


		for(int i=0; i<3; ++i) {
			Debug.DrawLine(mesh.vertices[0], mesh.vertices[i], Color.black);
		}
	}
}
