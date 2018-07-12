using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UniteBase : MonoBehaviour {

	[SerializeField, Tooltip("名前")]
	protected string NAME;
	[SerializeField, Tooltip("ランク もしかしたら倍率あるかも")]
	protected int RANK;
	[SerializeField]
	protected float HP;
	[SerializeField]
	protected float ATK;
	//[SerializeField]
	//protected float DEF;
	[SerializeField]
	protected float SPEED;
	[SerializeField, Tooltip("兵数")]
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
	public TerrainBase NowPos;
	[SerializeField, Tooltip("移動場所")]
	protected TerrainBase GOTarget;

	protected bool IsMove;//移動中かどうか

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
	protected Vector2 AnsVec, startpos;
	protected Vector3 vec;
	public Vector2 START
	{
		set { startpos = value; }
		get { return startpos; }
	}


	private const string terr = "Terrain";
	private const string mytag = "Player", enetag = "Enemy";
	private const float singunPos = 0.5f;
	protected bool DragFlag = false, ReTurn = false;
	private bool FirstFlag = false;
	private bool IsFriend = false;

	private float timer = 0;

	//同行する部隊
	public List<UniteBase> FriendArmy = new List<UniteBase>();
	private static float[] GoudouPos_X = { 0,-0.6f, 0.6f };


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
		IsFriend = false;

		if (IsMove)
			timer += Time.deltaTime;

		if (GOTarget != null && (DragFlag || ReTurn))
		{
			if (!FirstFlag)//最初のみ
				MovingFarst();
			
			if (MoveTime <= timer)//移動完了時
			{
				IsMove = false;
				DragFlag = false;

				timer = 0;


				//敵味方判別及び戦闘部分は関数に
				ArraivalFunc(ReTurn);

				if (!ReTurn)
				{
					
					//戦闘後、全滅させることができなければ撤退する
					if (GOTarget.Garrison.Count > 0 && !IsFriend)
					{
						float _speed = SPEED;

						IsMove = true;
						Debug.Log("かえります");
						num = Array.IndexOf(GOTarget.Node, NowPos);

						Target_dis = GOTarget.Way[num];
						DragFlag = false;
						ReTurn = true;
						if (FriendArmy.Count > 0)
						{
							//速度でソート
							for (int i = 0; i < FriendArmy.Count; i++)
								_speed = FriendArmy[i].SPEED;
						}
						MoveTime = Target_dis / _speed;//残り時間計算
						Vector2 move_vec = NowPos.transform.localPosition - transform.localPosition;

						transform.localRotation = Quaternion.FromToRotation(Vector2.up, move_vec);

					}
					else if(IsFriend || GOTarget.Garrison.Count <= 0)//占領成功
					{
						Debug.Log("いすわり！");
						transform.localPosition = GOTarget.transform.localPosition;

						

						AfterArrivalFunc(true);
					}
				}
				else
				{
					//Debug.Log("そんなー");

					//戦闘後、全滅させることができなければ消滅する
					if (NowPos.Garrison.Count > 0 && !IsFriend)
					{
						Destroy(gameObject);
					}
					else //帰還成功
					{
						Debug.Log("ただいま！");
						transform.localPosition = NowPos.transform.localPosition;
						
						AfterArrivalFunc(false);
					}

				}
				

			}
			else if (MoveTime > timer)
			{
				float gg = Mathf.InverseLerp(0, MoveTime, timer);
				if (!ReTurn)
					AnsVec = Vector2.Lerp(NowPos.transform.localPosition, GOTarget.transform.localPosition, gg);
				else
					AnsVec = Vector2.Lerp(GOTarget.transform.localPosition, NowPos.transform.localPosition, gg);

				transform.localPosition = AnsVec;
			}
		}
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

	public void MoveFunc(TerrainBase Home, TerrainBase _GoTarget, int NodeNum)
	{
		float _speed = SPEED;

		GOTarget = _GoTarget;
		Target_dis = Home.Way[NodeNum];
		DragFlag = true;
		if (FriendArmy.Count > 0)
		{
			//速度でソート
			for(int i = 0; i < FriendArmy.Count; i++)
			{
				_speed = FriendArmy[i].SPEED;
				FriendArmy[i].transform.parent = transform;
				FriendArmy[i].transform.localPosition += new Vector3(GoudouPos_X[(i + 1) % 3], Mathf.Floor((i + 1) / 3));
			}

			
		}

		MoveTime = Target_dis / _speed;//残り時間計算
		Vector2 move_vec = GOTarget.transform.localPosition - transform.localPosition;
		
		transform.localRotation = Quaternion.FromToRotation(Vector2.up, move_vec);

	}

	//初期化
	public void ClearMove()
	{
		GOTarget = null;
		transform.localRotation = Quaternion.identity;
	}

	//移動時に最初だけ呼ばれる
	public virtual void MovingFarst()
	{
		IsMove = true;
		FirstFlag = true;
		NowPos.Garrison.Remove(this);
		NowPos.MoveGarrison.Add(this);//移動中の枠に移す
	}

	//到着直後
	private void ArraivalFunc(bool flag)
	{
		TerrainBase _go;
		if (flag)//trueなら帰還時
		{
			_go = NowPos;
		}
		else
			_go = GOTarget;

		//到着後戦闘判定
		if (_go.Garrison.Count > 0)
		{
			if (tag == _go.Garrison[0].tag)
			{
				//味方なので合流
				IsFriend = true;
			}
			else
			{
				//敵軍駐屯につき戦闘
				if (FriendArmy.Count == 0)
					_go.battleObserve.BattleCalculate(GOTarget.Garrison, this, _go);
				else
				{
					//FriendArmy.Insert(0, this);
					_go.battleObserve.BattleCalculate(GOTarget.Garrison, FriendArmy, _go);

				}
				//Debug.Log(_go);
				_go.Garrison_nonHPFind();
			}

		}

		if (NUMBER <= 0)
		{
			//Debug.Log("さよならー");
			Destroy(gameObject);
			return;
		}
	}

	//占領後
	private void AfterArrivalFunc(bool flag)
	{
		TerrainBase _before;
		if (flag)
		{
			_before = NowPos as TerrainBase;
			NowPos = GOTarget;
		}
		else
			_before = NowPos;


		if (FriendArmy.Count > 0)
		{
			for(int i = 1; i < FriendArmy.Count; i++)
			{
				//Debug.Log(flag);
				_before.MoveGarrison.Remove(FriendArmy[i]);

				FriendArmy[i].NowPos = NowPos;
				FriendArmy[i].startpos = NowPos.transform.localPosition;
				NowPos.Garrison.Add(FriendArmy[i]);
				
				FriendArmy[i].transform.parent = null;
				FriendArmy[i].ForFrinedATKInit();
			}
			FriendArmy.Clear();
		}
		startpos = NowPos.transform.localPosition;
		_before.MoveGarrison.Remove(this);//元の場所の移動兵力枠から消去
		GOTarget = null;
		ReTurn = false;
		FirstFlag = false;
		

		NowPos.Garrison.Add(this);

		//NowPos.ArmmyArrival();
		NowPos.SortArmy();//ソートする

		if ((CompareTag(mytag) && NowPos.ISOCCUPATION) || (CompareTag(enetag) && !NowPos.ISOCCUPATION))
		{
			NowPos.ISOCCUPATION = !NowPos.ISOCCUPATION;
		}
		NowPos.ArmmyArrival();
	}

	public void ForFrinedATKInit()
	{
		FirstFlag = false;
		IsMove = false;
	}

}
