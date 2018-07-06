using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TerrainBase : MonoBehaviour {
	[SerializeField]
	protected string NAME;

	//バフ類　後でインターフェースにするかも
	[SerializeField]
	protected float Buff_ATK;
	[SerializeField]
	protected float Buff_DEF;
	[SerializeField]
	protected float Buff_SPEED;

	[SerializeField, Tooltip("駐屯可能ユニット数")]
	protected int UnitNum;//駐屯可能ユニット数
	[SerializeField, Tooltip("駐屯可能ユニット")]
	protected UniteBase[] Garrison = new UniteBase[3];//適当に3

	[SerializeField, Tooltip("道がどこにつながっているか")]
	public TerrainBase[] Node = new TerrainBase[2];//道がどこにつながっているか
	[SerializeField]
	public float[] Way = new float[10];//その距離

	[SerializeField]
	private Text ui_name, ui_buff_atk, ui_buff_def, ui_buff_speed;
	[SerializeField]
	private Text[] ui_buff_sta = new Text[3];


	protected StringBuilder sb;

	void Start () {
		sb = new StringBuilder();
		PointerExitFunc();
	}
	
	void Update () {
		
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

		if (Garrison[0])
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

		if (Garrison[1])
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

		if (Garrison[2])
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
}
