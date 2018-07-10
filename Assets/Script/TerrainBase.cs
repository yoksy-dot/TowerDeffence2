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
	[SerializeField]
	protected float Buff_ATK;
	[SerializeField]
	protected float Buff_DEF;
	[SerializeField]
	protected float Buff_SPEED;

	[SerializeField, Tooltip("駐屯可能ユニット数")]
	protected int UnitNum;//駐屯可能ユニット数
	public int U_NUM {
		get { return UnitNum; }
	}
	[Tooltip("駐屯可能ユニット")]
	//public UniteBase[] Garrison = new UniteBase[3];//適当に3
	public List<UniteBase> Garrison = new List<UniteBase>();
	public List<UniteBase> MoveGarrison = new List<UniteBase>();

	[SerializeField, Tooltip("道がどこにつながっているか")]
	public TerrainBase[] Node = new TerrainBase[2];//道がどこにつながっているか
	[SerializeField]
	public float[] Way = new float[10];//その距離

	[SerializeField]
	private Text ui_name;
	[SerializeField]
	private Text ui_buff_atk;
	[SerializeField]
	private Text ui_buff_def;
	[SerializeField]
	private Text ui_buff_speed;

	[SerializeField]
	private Text[] ui_buff_sta = new Text[3];

	/*出撃関連*/
	[SerializeField]
	protected LineRenderer _Line;

	protected Vector3 screenPos, ArrowPos;
	protected Vector2 AnsVec, startpos;
	protected bool DragFlag = false;


	protected StringBuilder sb;
	public BattleObserve battleObserve;

	void Start () {
		sb = new StringBuilder();
		PointerExitFunc();
		battleObserve = new BattleObserve(this);
	}
	
	void Update () {
		if(Garrison.Count > 0)
		{

		}

		if (IsOccupation)
		{
			_sp.color = Color.red;
		}
		else
		{
			_sp.color = Color.blue;
		}
	}

	//Garrison内をHPでソート
	public void SortArmy(List<UniteBase> Garrison)
	{
		if (Garrison.Count == 1)//配列が一ならソートしない
			return;
		//Garrison.Sort();
		//キャッシュ
		//UniteBase[] heisi = new UniteBase[3];
		//for (int i = 0; i < Garrison.Count; i++)
		//{
		//	heisi[i] = Garrison[i];
		//}

		UniteBase kari;
		if (Garrison[0]._HP < Garrison[1]._HP)
		{
			//kari = Garrison[0];
			//Garrison[0] = Garrison[1];
			//Garrison[1] = kari;
			kari = Garrison[0];
			Garrison.Remove(Garrison[0]);
		}
		if (Garrison[2] && Garrison[1]._HP < Garrison[2]._HP)
		{
			kari = Garrison[1];
			Garrison[1] = Garrison[2];
			Garrison[2] = kari;
		}

		//for (int i = 0; i < Garrison.Count; i++)
		//{
		//	Garrison[i] = heisi[i];
		//}
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

		sb.Append("SPEED:");
		sb.Append(Buff_SPEED);
		ui_buff_speed.text = sb.ToString();
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

		sb.Append("SPEED:");
		ui_buff_speed.text = sb.ToString();
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

	//public void ClickFunc()
	//{
	//	if (Garrison.Count <= 0)
	//		return;

	//	_Line.gameObject.SetActive(true);
	//}

	public void DragFunc()
	{
		if (Garrison.Count <= 0)
			return;
		_Line.gameObject.SetActive(true);
		screenPos = Input.mousePosition;
		screenPos.z = 10.0f;
		AnsVec = Camera.main.ScreenToWorldPoint(screenPos);

		//transform.localPosition = AnsVec;
		_Line.SetPosition(0, transform.localPosition);
		_Line.SetPosition(1, AnsVec);

	}

	public void EndDragFunc()
	{
		if (Garrison.Count <= 0)
			return;
		_Line.gameObject.SetActive(false);
		//transform.localPosition = startpos;
		DragFlag = true;

	}

	//private void OnTriggerEnter2D(Collider2D coll)
	//{
	//	if (Garrison.Count <= 0)
	//		return;
	//	//Debug.Log("asdhfj");
	//	GameObject _collobj = coll.gameObject;
	//	if (_collobj != NowPos.gameObject && _collobj.CompareTag(terr))
	//	{
	//		GOTarget = _collobj.GetComponent<TerrainBase>();

	//		num = Array.IndexOf(NowPos.Node, GOTarget);
	//		if (num < 0)
	//		{
	//			GOTarget = null;
	//			return;
	//		}

	//		Target_dis = NowPos.Way[num];
	//		DragFlag = false;
	//		MoveTime = Target_dis / SPEED;//残り時間計算
	//		Vector2 move_vec = GOTarget.transform.localPosition - transform.localPosition;

	//		transform.localRotation = Quaternion.FromToRotation(Vector2.up, move_vec);
	//	}
	//}
}
