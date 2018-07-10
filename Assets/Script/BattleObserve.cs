using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObserve
{
	private List<UniteBase> Attacker = new List<UniteBase>();
	private List<UniteBase> Diffender = new List<UniteBase>();
	//private TerrainBase _base;

	float[] A = new float[3];
	float[] D = new float[3];

	//戦闘場所と攻撃者のデータ
	public BattleObserve(TerrainBase Current)
	{
		//_base = Current;
	}

	public void BattleCalculate(List<UniteBase> _Diffender, List<UniteBase> _Attacker)
	{
		//Debug.Log("dddd");
		//初期化
		Attacker.Clear();
		Diffender.Clear();
		//キャッシュ
		Diffender =  new List<UniteBase>(_Diffender);
		Attacker = new List<UniteBase>(_Attacker);


		for (int i = 0; i < Attacker.Count; i++)
		{
			if (!Attacker[i])
				A[i] = Attacker[i].UnitAtkCalculate();//攻撃側攻撃力計算
			else//消したい
				A[i] = 0;
		}

		for (int i = 0; i < Diffender.Count; i++)
		{
			if (!Diffender[i])
				D[i] = Diffender[i].UnitAtkCalculate();//防御側攻撃力計算
			else
				D[i] = 0;
		}

		//このへん攻撃受ける側が少ないとダメージが変に減ってる気がする
		
		//防御側ダメージ計算
		for (int i = 0; i < Diffender.Count; i++)
		{
			for (int j = 0; j < Attacker.Count; j++)
			{
				if(Diffender.Count == 1)
				{
					//一人のときは当然すべて受ける
					Battle(Diffender[i], A[j]);
				}
				else
				{
					//0番目は攻撃力の2/3を受ける
					Battle(Diffender[i], (A[j] / 3) * (2 - i));
				}
			}
		}

		//攻撃側ダメージ計算
		for (int i = 0; i < Attacker.Count; i++)
		{
			for (int j = 0; j < Diffender.Count; j++)
			{
				//0番目は攻撃力の2/3を受ける
				Battle(Attacker[i], (A[j] / 3) * (2 - i));
				if (Attacker.Count == 1)
					Debug.Log("戦闘システムに以上あり");
			}
		}

		_Diffender.Clear();
		_Attacker.Clear();
		//値を返却
		for (int i = 0; i < Diffender.Count; i++)
		{
			if (Diffender[i] && Diffender[i]._HP > 0)
				_Diffender.Add(Diffender[i]);
		}
		for (int i = 0; i < Attacker.Count; i++)
		{
			if (Attacker[i] && Attacker[i]._HP > 0)
				_Attacker.Add(Attacker[i]);
		}

	}

	//一人攻撃用
	public void BattleCalculate(List<UniteBase> _Diffender, UniteBase _Attacker)
	{
		//Debug.Log("oson");
		//初期化
		Attacker.Clear();
		Diffender.Clear();
		//キャッシュ
		Diffender = new List<UniteBase>(_Diffender);
		Attacker.Add(_Attacker);




		A[0] = Attacker[0].UnitAtkCalculate();//攻撃側攻撃力計算
		//Debug.Log("A:" + A[0]);

		for (int i = 0; i < Diffender.Count; i++)
		{
			if (Diffender[i] != null)
				D[i] = Diffender[i].UnitAtkCalculate();//防御側攻撃力計算
			else
				D[i] = 0;
		}
		//Debug.Log("D0:" + D[0] + ",D1:" + D[1]);
		//防御側ダメージ計算
		for (int i = 0; i < Diffender.Count; i++)
		{

			//0番目は攻撃力の2/3を受ける
			Battle(Diffender[i], (A[0] / 3) * (2 - i));

		}

		//攻撃側ダメージ計算

		for (int j = 0; j < 3; j++)
		{
			//0番目は攻撃力の2/3を受ける
			Battle(Attacker[0], D[j]);
		}


		_Diffender.Clear();
		//_Attacker.Clear();
		//Debug.Log(Diffender.Count);

		//値を返却
		for (int i = 0; i < Diffender.Count; i++)
		{
			//Debug.Log(Diffender[i].name);

			if (Diffender[i] && Diffender[i]._HP > 0)
				_Diffender.Add(Diffender[i]);
		}
		//for (int i = 0; i < Attacker.Count; i++)
		//{
		//	if (Attacker[i] && Attacker[i]._HP > 0)
		//		_Attacker.Add(Attacker[i]);
		//}

		//HPが0なら攻撃者をnullで返す
		if (Attacker[0]._HP < 0)
		{
			//_Attacker = null;
			Debug.Log("やられたー");
		}
			
				
	}

	private void Battle(UniteBase suffer, float dmg)//受ける側を入れる
	{
		float allHP = suffer._HP * suffer._NUMBER;
		if (allHP > dmg)
		{
			//Debug.Log(suffer.name + "dmg:" + dmg);
			allHP -= dmg;//与ダメ
			//Debug.Log(suffer.name +"HP:" + allHP);
			int nokori = Mathf.CeilToInt(allHP / suffer._HP);//残数計算(切り上げ)
			
			suffer._NUMBER = nokori;//代入
		}
		else
		{
			//撃破の時
			suffer._NUMBER = 0;
		}
	}


}
