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
	//[SerializeField]
	//protected float DEF;
	[SerializeField]
	protected float SPEED;
	[SerializeField,Tooltip("兵数")]
	protected int NUMBER;


	public float _HP
	{
		set { HP = value; }
		get { return HP; }
	}
	public int _NUMBER
	{
		set { NUMBER = value; }
		get { return NUMBER; }
	}

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
	protected LineRenderer _Line;

	//[SerializeField]
	//private GameObject UI_Panel;
	[SerializeField]
	private Text ui_Name;
	[SerializeField]
	private Text ui_HP;
	[SerializeField]
	private Text ui_Atk;
	//[SerializeField]
	//private Text ui_Def;
	[SerializeField]
	private Text ui_Speed;
	[SerializeField]
	private Text ui_num;
	[SerializeField]
	private Text ui_rank;

	protected Vector3 screenPos, ArrowPos;
	protected Vector2 AnsVec,startpos;
	protected Vector3 vec;

	
	private const string terr = "Terrain";
	protected bool DragFlag = false , ReTurn = false;

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

		if (GOTarget != null && (DragFlag || ReTurn))
		{
			
			MovingFarst();
			
			if (MoveTime <= timer)//移動完了時
			{
				IsMove = false;
				//transform.localPosition = GOTarget.transform.localPosition;
				//startpos = transform.localPosition;
				//NowPos = GOTarget;
				//GOTarget = null;
				DragFlag = false;
				//ReTurn = false;
				timer = 0;

				//Debug.Log("おわったーん");
				if (!ReTurn)
				{
					//到着後戦闘判定
					if (GOTarget.Garrison.Count > 0)
						GOTarget.battleObserve.BattleCalculate(GOTarget.Garrison, this);
					if (NUMBER <= 0)
					{
						Debug.Log("さよならー");
						Destroy(gameObject);
						return;
					}

					//戦闘後、全滅させることができなければ撤退する
					if (GOTarget.Garrison.Count > 0)
					{
						IsMove = true;
						//Debug.Log("かえります");
						num = Array.IndexOf(GOTarget.Node, NowPos);

						Target_dis = GOTarget.Way[num];
						DragFlag = false;
						ReTurn = true;
						MoveTime = Target_dis / SPEED;//残り時間計算
						Vector2 move_vec = NowPos.transform.localPosition - transform.localPosition;

						transform.localRotation = Quaternion.FromToRotation(Vector2.up, move_vec);

					}
					else //占領成功
					{
						//Debug.Log("いすわり！");
						transform.localPosition = GOTarget.transform.localPosition;
						startpos = transform.localPosition;
						NowPos.MoveGarrison.Remove(this);//元の場所の移動兵力枠から消去
						NowPos = GOTarget;
						GOTarget = null;
						ReTurn = false;
						NowPos.Garrison.Add(this);
					}
				}
				else
				{
					//Debug.Log("そんなー");
					//到着後戦闘判定
					if (NowPos.Garrison.Count > 0)
						NowPos.battleObserve.BattleCalculate(NowPos.Garrison, this);
					//戦闘後、全滅させることができなければ消滅する
					if (NowPos.Garrison.Count > 0)
					{
						Destroy(gameObject);
					}
					else //帰還成功
					{
						//Debug.Log("ただいま！");
						transform.localPosition = NowPos.transform.localPosition;
						startpos = transform.localPosition;
						//NowPos = GOTarget;
						GOTarget = null;
						ReTurn = false;
						NowPos.Garrison.Add(this);
						NowPos.MoveGarrison.Remove(this);
					}

				}

			}
			else if (MoveTime > timer)
			{
				float gg = Mathf.InverseLerp(0, MoveTime, timer);
				if (!ReTurn)
				{
					//Debug.Log(gg);
					AnsVec = Vector2.Lerp(NowPos.transform.localPosition, GOTarget.transform.localPosition, gg);
					transform.localPosition = AnsVec;
				}

				else
				{
					AnsVec = Vector2.Lerp(GOTarget.transform.localPosition, NowPos.transform.localPosition, gg);
					transform.localPosition = AnsVec;
				}
			}
		}
	}

	protected virtual void MovingFarst()
	{
		IsMove = true;
		NowPos.Garrison.Remove(this);
		NowPos.MoveGarrison.Add(this);//移動中の枠に移す
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

		//sb.Append("DEF:");
		//sb.Append(DEF);
		//ui_Def.text = sb.ToString();
		//sb.Length = 0;

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

		//sb.Append("DEF:");
		//ui_Def.text = sb.ToString();
		//sb.Length = 0;

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
		//Debug.Log("asdhfj");
		GameObject _collobj = coll.gameObject;
		if (_collobj != NowPos.gameObject && _collobj.CompareTag(terr))
		{
			GOTarget = _collobj.GetComponent<TerrainBase>();

			num = Array.IndexOf(NowPos.Node, GOTarget);
			if(num < 0)
			{
				GOTarget = null;
				return;
			}

			Target_dis = NowPos.Way[num];
			DragFlag = false;
			MoveTime = Target_dis / SPEED;//残り時間計算
			Vector2 move_vec = GOTarget.transform.localPosition - transform.localPosition;
			
			transform.localRotation = Quaternion.FromToRotation(Vector2.up, move_vec);
		}

	}


	private void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag(terr) && !(DragFlag || ReTurn))
		{
			//DragFlag = false;
			GOTarget = null;
		}
	}

	public virtual float UnitAtkCalculate()
	{
		float a = ATK * NUMBER;
		switch (NUMBER)
		{
			case 1:
				return a * 1;
			case 2:
				return a * 0.98f;
			case 3:
				return a * 0.95f;
			case 4:
				return a * 0.92f;
			case 5:
				return a * 0.88f;
			case 6:
				return a * 0.83f;
			case 7:
				return a * 0.77f;
			case 8:
				return a * 0.71f;
			case 9:
				return a * 0.66f;
			case 10:
				return a * 0.60f;
		}
		Debug.Log("ユニット数バグ");
		return 0;
	}
}
