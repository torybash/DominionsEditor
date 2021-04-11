using System;
using UnityEngine;
internal static class InputHelper
{
	public static bool GetKeyNumberPressed (out int numberPressed)
	{
		numberPressed = -1;
		if (Input.GetKeyDown(KeyCode.Alpha0)) numberPressed = 0;
		else if (Input.GetKeyDown(KeyCode.Alpha1)) numberPressed = 1;
		else if (Input.GetKeyDown(KeyCode.Alpha2)) numberPressed = 2;
		else if (Input.GetKeyDown(KeyCode.Alpha3)) numberPressed = 3;
		else if (Input.GetKeyDown(KeyCode.Alpha4)) numberPressed = 4;
		else if (Input.GetKeyDown(KeyCode.Alpha5)) numberPressed = 5;
		else if (Input.GetKeyDown(KeyCode.Alpha6)) numberPressed = 6;
		else if (Input.GetKeyDown(KeyCode.Alpha7)) numberPressed = 7;
		else if (Input.GetKeyDown(KeyCode.Alpha8)) numberPressed = 8;
		else if (Input.GetKeyDown(KeyCode.Alpha9)) numberPressed = 9;

		return numberPressed >= 0;
	}
}