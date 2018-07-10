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

	}
}
