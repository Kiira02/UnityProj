using UnityEngine;
using System.Collections;

public class ClienGUI : MonoBehaviour {
	
	private const string CMDLineName = "CMD textField";
	private const float TextFieldHeight = 20.0f;
	
	private static readonly Vector2 DefaultWindowSize = new Vector2(400, 200);
	
	private string cmdString = "";
	private bool keyPressed = false;
	
	public Vector2 LoginWindowSize = new Vector2(DefaultWindowSize.x, DefaultWindowSize.y);
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
	}
	
	void OnGUI () {
		
	 	GUI.SetNextControlName(CMDLineName);
		cmdString = GUI.TextField(new Rect(10,Screen.height-30-10,Screen.width-130,20), cmdString);
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
						if (cmdString.Length>0)
						{
							if (cmdString=="/disconect")
							{
								this.GetComponent<ClientInterface>().Disconect();
							}
							
							if (cmdString=="/help")
							{
								this.GetComponent<ClientInterface>().PrintHelp();
							}
						}
						
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