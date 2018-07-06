using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UniteBase : MonoBehaviour {

	[SerializeField, Tooltip("名前")]
	protected string NAME;
	[SerializeField,Tooltip("ランク もしかしたら倍率あるかも")]
	protected int RANK;
	[SerializeField]
	protected float HP;
	[SerializeField]
	protected float ATK;
	[SerializeField]
	protected float DEF;
	[SerializeField]
	protected float SPEED;
	[SerializeField,Tooltip("兵数")]
	protected int NUMBER;

	public string _NAME
	{
		get { return NAME; }
	}

	[SerializeField, Tooltip("現在の場所")]
	protected TerrainBase NowPos;
	[SerializeField,Tooltip("移動場所")]
	protected TerrainBase GOTarget;
	
	protected bool IsMove;//移動中かどうか
	//private float Move_distance;//移動距離
	private float Target_dis;//目的地までの距離
	private float MoveTime;//移動完了までの時間

	int num;

	protected StringBuilder sb;

	[SerializeField]
	private LineRenderer _Line;

	//[SerializeField]
	//private GameObject UI_Panel;
	[SerializeField]
	private Text ui_Name,ui_HP,ui_Atk,ui_Def,ui_Speed,ui_num,ui_rank;

	Vector3 screenPos, ArrowPos;
	Vector2 AnsVec,startpos;
	Vector3 vec;

	private string terr = "Terrain";
	private bool DragFlag = false;

	private float timer = 0;
	//float startTime;


	private void Awake()
	{
		startpos = transform.localPosition;
		sb = new StringBuilder();
		PointerExitFunc();
	}

	private void OnEnable()
	{
		//_Line.gameObject.SetActive(false);
	}

	void Start () {
		
	}
	

	void Update () {
		if(IsMove)
			timer += Time.deltaTime;

		if (GOTarget != null && DragFlag)
		{
			IsMove = true;
			//Move_distance += SPEED;
			if (MoveTime <= timer)//移動完了時
			{
				IsMove = false;
				transform.localPosition = GOTarget.transform.localPosition;
				startpos = transform.localPosition;
				NowPos = GOTarget;
				GOTarget = null;
				DragFlag = false;
				timer = 0;
			}
			else if (MoveTime > timer)
			{
				float gg = Mathf.InverseLerp(0, MoveTime, Time.deltaTime);
				transform.localPosition = Vector2.Lerp(transform.localPosition, GOTarget.transform.localPosition, gg);
			}
		}
	}

	protected float  UnitAtkCalculate(float atk, int num)
	{
		switch (num)
		{
			case 1:
				return atk * num * 1;
			case 2:
				return atk * num * 0.98f;
			case 3:
				return atk * num * 0.95f;
			case 4:
				return atk * num * 0.92f;
			case 5:
				return atk * num * 0.88f;
			case 6:
				return atk * num * 0.83f;
			case 7:
				return atk * num * 0.77f;
			case 8:
				return atk * num * 0.71f;
			case 9:
				return atk * num * 0.66f;
			case 10:
				return atk * num * 0.60f;
		}
		Debug.Log("ユニット数バグ");
		return 0;
	}

	public void DragFunc()
	{
		if (IsMove)
			return;
		_Line.gameObject.SetActive(true);
		screenPos = Input.mousePosition;
		screenPos.z = 10.0f;
		AnsVec = Camera.main.ScreenToWorldPoint(screenPos);

		transform.localPosition = AnsVec;
		_Line.SetPosition(0, startpos);
		_Line.SetPosition(1, AnsVec);
		
	}

	public void EndDragFunc()
	{
		if (IsMove)
			return;
		_Line.gameObject.SetActive(false);
		transform.localPosition = startpos;
		DragFlag = true;

	}

	public void ClickGunc()
	{
		//Debug.Log("qwwe");
	}

	public void PointerEnterFunc()
	{
		//sb.Append(NAME);
		sb.Append(NAME);
		ui_Name.text = sb.ToString();
		sb.Length = 0;

		sb.Append("HP:");
		sb.Append(HP);
		ui_HP.text = sb.ToString();
		sb.Length = 0;

		sb.Append("ATK:");
		sb.Append(ATK);
		ui_Atk.text = sb.ToString();
		sb.Length = 0;

		sb.Append("DEF:");
		sb.Append(DEF);
		ui_Def.text = sb.ToString();
		sb.Length = 0;

		sb.Append("SPEED:");
		sb.Append(SPEED);
		ui_Speed.text = sb.ToString();
		sb.Length = 0;

		sb.Append("NUM:");
		sb.Append(NUMBER);
		ui_num.text = sb.ToString();
		sb.Length = 0;

		sb.Append("RANK:");
		sb.Append(RANK);
		ui_rank.text = sb.ToString();
		sb.Length = 0;
	}

	public void PointerExitFunc()
	{

		sb.Append("NAME");
		ui_Name.text = sb.ToString();
		sb.Length = 0;

		sb.Append("HP:");
		ui_HP.text = sb.ToString();
		sb.Length = 0;

		sb.Append("ATK:");
		ui_Atk.text = sb.ToString();
		sb.Length = 0;

		sb.Append("DEF:");
		ui_Def.text = sb.ToString();
		sb.Length = 0;

		sb.Append("SPEED:");
		ui_Speed.text = sb.ToString();
		sb.Length = 0;

		sb.Append("NUM:");
		ui_num.text = sb.ToString();
		sb.Length = 0;

		sb.Append("RANK:");
		ui_rank.text = sb.ToString();
		sb.Length = 0;
	}

	private void OnTriggerStay2D(Collider2D coll)
	{
		if (IsMove)
			return;
		GameObject _collobj = coll.gameObject;
		if (_collobj != NowPos.gameObject && _collobj.CompareTag(terr))
		{
			GOTarget = _collobj.GetComponent<TerrainBase>();

			num = Array.IndexOf(NowPos.Node, GOTarget);
			Target_dis = GOTarget.Way[num];
			DragFlag = false;
			MoveTime = Target_dis / SPEED;//残り時間計算
			Vector2 move_vec = GOTarget.transform.localPosition - transform.localPosition;
			
			transform.localRotation = Quaternion.FromToRotation(Vector2.up, move_vec);
		}

	}


	private void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag(terr) && !DragFlag)
		{
			//DragFlag = false;
			GOTarget = null;
		}
	}
}
