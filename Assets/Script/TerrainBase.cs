using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TerrainBase : MonoBehaviour {
	[SerializeField]
	protected string NAME;

	[SerializeField,Tooltip("敵に占領されていればTrueで赤色になる")]
	private bool IsOccupation;

	public bool ISOCCUPATION
	{
		get { return IsOccupation; }
		set { IsOccupation = value; }
	}

	[SerializeField]
	private SpriteRenderer _sp;

	//バフ類　後でインターフェースにするかも
	[SerializeField,Tooltip("防御側に％で加算")]
	protected float Buff_ATK = 0;
	[SerializeField,Tooltip("攻撃側のダメージを%で軽減")]
	protected float Buff_DEF = 0;
	//[SerializeField,Tooltip("ここに来るまでに%でバフ")]
	//protected float Buff_SPEED = 100;

	public float B_ATK
	{
		get { return Buff_ATK; }
	}
	public float B_DEF
	{
		get { return Buff_DEF; }
	}


	//[SerializeField, Tooltip("駐屯可能ユニット数")]
	//protected int UnitNum;//駐屯可能ユニット数
	//public int U_NUM {
	//	get { return UnitNum; }
	//}

	[Tooltip("駐屯可能ユニット")]
	//public UniteBase[] Garrison = new UniteBase[3];//適当に3
	public List<UniteBase> Garrison = new List<UniteBase>();
	public List<UniteBase> MoveGarrison = new List<UniteBase>();

	[SerializeField, Tooltip("道がどこにつながっているか")]
	public TerrainBase[] Node = new TerrainBase[2];//道がどこにつながっているか
	[SerializeField]
	public float[] Way = new float[2];//その距離
	//[SerializeField]
	public readonly Vector3[] Terra_Pos = new Vector3[10];

	[SerializeField]
	private Text ui_name;
	[SerializeField]
	private Text ui_buff_atk;
	[SerializeField]
	private Text ui_buff_def;


	[SerializeField]
	private Text[] ui_buff_sta = new Text[3];

	/*出撃関連*/
	[SerializeField]
	protected LineRenderer _Line;

	protected Vector3 screenPos, ArrowPos;
	protected Vector2 AnsVec, startpos;
	protected bool DragFlag = false;
	private static float ui_dis = 2.0f;

	/*待機中*/
	private int gari_num = 0;
	private static float taiki = 0.3f;
	private Vector2 ThisPos;

	protected StringBuilder sb;
	public BattleObserve battleObserve;

	void Start () {
		sb = new StringBuilder();
		PointerExitFunc();
		battleObserve = new BattleObserve(this);

		for(int i = 0; i < Node.Length; i++)
		{
			Terra_Pos[i] = Node[i].transform.localPosition;
		}

		ThisPos = transform.position;
		//初期位置ずらし
		SortArmy();

		gari_num = Garrison.Count;

		if (IsOccupation)
		{
			_sp.color = Color.red;
		}
		else
		{
			_sp.color = Color.blue;
		}

		
	}
	
	void Update () {
		screenPos = Input.mousePosition;
		screenPos.z = 10.0f;
		AnsVec = Camera.main.ScreenToWorldPoint(screenPos);

		if (Garrison.Count > 0)
		{
			if(gari_num != Garrison.Count)
			{
				switch (Garrison.Count)
				{
					case 0:
						break;
					case 1:
						Garrison[0].START = ThisPos + new Vector2(0, taiki);
						break;
					case 2:
						Garrison[0].START = ThisPos + new Vector2(0, taiki);
						Garrison[1].START = ThisPos + new Vector2(-taiki, -taiki);
						break;
					case 3:
						Garrison[0].START = ThisPos + new Vector2(0, taiki);
						Garrison[1].START = ThisPos + new Vector2(-taiki, -taiki);
						Garrison[2].START = ThisPos + new Vector2(taiki, -taiki);
						break;
					case 4:
						Garrison[0].START = ThisPos + new Vector2(-taiki, taiki);
						Garrison[1].START = ThisPos + new Vector2(taiki, taiki);
						Garrison[2].START = ThisPos + new Vector2(-taiki, -taiki);
						Garrison[3].START = ThisPos + new Vector2(taiki, -taiki);

						break;
					case 5:
						Garrison[0].START = ThisPos + new Vector2(-taiki, taiki);
						Garrison[1].START = ThisPos + new Vector2(-0, taiki);
						Garrison[2].START = ThisPos + new Vector2(taiki, taiki);
						Garrison[3].START = ThisPos + new Vector2(-taiki, -taiki);
						Garrison[4].START = ThisPos + new Vector2(0, -taiki);
						break;
					case 6:
						Garrison[0].START = ThisPos + new Vector2(-taiki, taiki);
						Garrison[1].START = ThisPos + new Vector2(-0, taiki);
						Garrison[2].START = ThisPos + new Vector2(taiki, taiki);
						Garrison[3].START = ThisPos + new Vector2(-taiki, -taiki);
						Garrison[4].START = ThisPos + new Vector2(0, -taiki);
						Garrison[4].START = ThisPos + new Vector2(taiki, -taiki);
						break;

				}
				gari_num = Garrison.Count;
			}
		}

		
	}

	//Garrison内をHPでソート
	public void SortArmy()
	{
		if (Garrison.Count > 1)//配列が1ならソートしない
		{

			UniteBase kari;
			if (Garrison[0]._HP < Garrison[1]._HP)
			{
				kari = Garrison[0];
				Garrison.Remove(Garrison[0]);
				Garrison.Add(kari);
			}
			if (Garrison.Count > 2 && Garrison[1]._HP < Garrison[2]._HP)
			{
				kari = Garrison[1];
				Garrison.Remove(Garrison[1]);
				Garrison.Add(kari);

			}
		}

		switch (Garrison.Count)
		{
			case 0:
				break;
			case 1:
				Garrison[0].START = ThisPos + new Vector2(0, taiki);
				break;
			case 2:
				Garrison[0].START = ThisPos + new Vector2(0, taiki);
				Garrison[1].START = ThisPos + new Vector2(-taiki, -taiki);
				break;
			case 3:
				Garrison[0].START = ThisPos + new Vector2(0, taiki);
				Garrison[1].START = ThisPos + new Vector2(-taiki, -taiki);
				Garrison[2].START = ThisPos + new Vector2(taiki, -taiki);
				break;
			case 4:
				Garrison[0].START = ThisPos + new Vector2(-taiki, taiki);
				Garrison[1].START = ThisPos + new Vector2(taiki, taiki);
				Garrison[2].START = ThisPos + new Vector2(-taiki, -taiki);
				Garrison[3].START = ThisPos + new Vector2(taiki, -taiki);

				break;
			case 5:
				Garrison[0].START = ThisPos + new Vector2(-taiki, taiki);
				Garrison[1].START = ThisPos + new Vector2(-0, taiki);
				Garrison[2].START = ThisPos + new Vector2(taiki, taiki);
				Garrison[3].START = ThisPos + new Vector2(-taiki, -taiki);
				Garrison[4].START = ThisPos + new Vector2(0, -taiki);
				break;
			case 6:
				Garrison[0].START = ThisPos + new Vector2(-taiki, taiki);
				Garrison[1].START = ThisPos + new Vector2(-0, taiki);
				Garrison[2].START = ThisPos + new Vector2(taiki, taiki);
				Garrison[3].START = ThisPos + new Vector2(-taiki, -taiki);
				Garrison[4].START = ThisPos + new Vector2(0, -taiki);
				Garrison[4].START = ThisPos + new Vector2(taiki, -taiki);
				break;

		}

		for(int i = 0; i < Garrison.Count; i++)
		{
			Garrison[i].transform.localPosition = Garrison[i].START;
			Garrison[i].transform.localRotation = Quaternion.identity;
		}
	}

	public void PointerEnterFunc()
	{
		sb.Append(NAME);
		ui_name.text = sb.ToString();
		sb.Length = 0;

		sb.Append("ATK:");
		sb.Append(Buff_ATK);
		ui_buff_atk.text = sb.ToString();
		sb.Length = 0;

		sb.Append("DEF:");
		sb.Append(Buff_DEF);
		ui_buff_def.text = sb.ToString();
		sb.Length = 0;

		if (Garrison.Count > 1)
		{
			sb.Append(Garrison[0]._NAME);
			ui_buff_sta[0].text = sb.ToString();
			sb.Length = 0;
		}
		else
		{
			sb.Append("STATION");
			ui_buff_sta[0].text = sb.ToString();
			sb.Length = 0;
		}

		if (Garrison.Count > 2)
		{
			sb.Append(Garrison[1]._NAME);
			ui_buff_sta[1].text = sb.ToString();
			sb.Length = 0;
		}
		else
		{
			sb.Append("");
			ui_buff_sta[1].text = sb.ToString();
			sb.Length = 0;
		}

		if (Garrison.Count > 3)
		{
			sb.Append(Garrison[2]._NAME);
			ui_buff_sta[2].text = sb.ToString();
			sb.Length = 0;
		}
		else
		{
			sb.Append("");
			ui_buff_sta[2].text = sb.ToString();
			sb.Length = 0;
		}
	}

	public void PointerExitFunc()
	{
		sb.Append("NAME");
		ui_name.text = sb.ToString();
		sb.Length = 0;

		sb.Append("ATK:");
		ui_buff_atk.text = sb.ToString();
		sb.Length = 0;

		sb.Append("DEF:");
		ui_buff_def.text = sb.ToString();
		sb.Length = 0;


		sb.Append("STATION");
		ui_buff_sta[0].text = sb.ToString();
		sb.Length = 0;

		sb.Append("");
		ui_buff_sta[1].text = sb.ToString();
		sb.Length = 0;

		sb.Append("");
		ui_buff_sta[2].text = sb.ToString();
		sb.Length = 0;
	}

	public void DragFunc()
	{
		if (Garrison.Count <= 0 || IsOccupation)
			return;
		_Line.gameObject.SetActive(true);
		

		//transform.localPosition = AnsVec;
		_Line.SetPosition(0, transform.localPosition);
		_Line.SetPosition(1, AnsVec);

	}

	public void EndDragFunc()
	{
		if (Garrison.Count <= 0 || IsOccupation)
			return;

		int go_num = -1;

		_Line.gameObject.SetActive(false);
		//transform.localPosition = startpos;
		DragFlag = true;

		//領土の近くでボタンを離すと
		for (int i = 0; i < Node.Length; i++)
		{
			if (Vector3.SqrMagnitude((Vector3)AnsVec - Terra_Pos[i]) < ui_dis)
			{
				//Debug.Log("なんですと");
				go_num = i;
				break;
			}
		}
		//Debug.Log(go_num);
		//近くで離したときには出撃
		if (go_num >= 0)
		{
			int num = Garrison.Count;
			for (int i = 0; i < num; i++)
			{
				//Debug.Log(Garrison[0].name);
				Garrison[0].MovingFarst();
				//if (i > 0)
					MoveGarrison[0].FriendArmy.Add(MoveGarrison[i]);
				//MoveGarrison[i].MoveFunc(this, Node[go_num], go_num);//移動設定関数呼び出し
			}
			MoveGarrison[0].MoveFunc(this, Node[go_num], go_num);
		}
		else
		{
			for (int i = 0; i < Garrison.Count; i++)
			{
				Garrison[i].ClearMove();//初期化
			}
		}
	}

	//色変え
	public void ArmmyArrival()
	{
		if (IsOccupation)
		{
			_sp.color = Color.red;
		}
		else
		{
			_sp.color = Color.blue;
		}


	}

	//HP0の兵隊を探す
	public void Garrison_nonHPFind()
	{
		
		for(int i = 0; i < Garrison.Count; i++)
		{
			if (Garrison[i]._NUMBER <= 0)
			{
				GameObject aaa = Garrison[i].gameObject;
				//if(Garrison[i].FriendArmy.Count > 0)
				//{
				//	Garrison[i].transform.DetachChildren();//対象の兵隊の子をすべて切り離し
				//	Garrison[i].FriendArmy[1]
				//	if (Garrison[i].FriendArmy.Count > 3)//友軍数が3以上なら(1は対象の奴)
				//	{

				//		for (int k = 2; k < Garrison[i].FriendArmy.Count; k++)//親子関係組み直し
				//		{
				//			Garrison[i].FriendArmy[k].transform.parent = Garrison[i].FriendArmy[1].transform;
				//			//Garrison[i].FriendArmy[1].FriendArmy;
				//		}
				//	}
				//}
				Garrison.Remove(Garrison[i]);
				Destroy(aaa);
			}

		}
	}
}
