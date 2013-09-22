using UnityEngine;
using System.Collections;

public class ClienGUI : MonoBehaviour {
	
	private const string CMDLineName = "CMD textField";
	
	private string cmdString = "";
	
	private bool keyPressed = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
	}
	 
	void OnGUI () {
		
	 	GUI.SetNextControlName(CMDLineName);
		cmdString = GUI.TextField(new Rect(10,Screen.height-30-10,Screen.width-130,30), cmdString);
		GUI.SetNextControlName("");
		
		Event e = Event.current;
		if(!keyPressed) {
			if (e.type == EventType.KeyDown) 
			{
				keyPressed = true;
				if (e.keyCode == KeyCode.Return) 
				{
					GUI.FocusControl(CMDLineName);	
				} else {
					if (e.character == '\n') 
					{
						cmdString = "";
						GUI.FocusControl("");
					}
				}
				
			}
		}
		else 
		{
			if (e.type == EventType.KeyUp) 
			{
				keyPressed = false;
			}
		}
	}
}