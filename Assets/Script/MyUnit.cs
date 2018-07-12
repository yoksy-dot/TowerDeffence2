using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnit : UniteBase
{

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

		int go_num = -1;

		//領土の近くでボタンを離すと
		for (int i = 0; i < NowPos.Node.Length; i++)
		{
			if (Vector3.SqrMagnitude((Vector3)AnsVec - NowPos.Terra_Pos[i]) < 1.0f)
			{
				//Debug.Log("なんですと");
				go_num = i;
				break;
			}
		}

		if(go_num >= 0)
			MoveFunc(NowPos, NowPos.Node[go_num], go_num);

	}
}
