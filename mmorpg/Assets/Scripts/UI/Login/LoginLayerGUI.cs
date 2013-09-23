using UnityEngine;
using System.Collections;

public class LoginLayerGUI : MonoBehaviour {

	public Vector2 Size;
	public Vector2 Position;
	
	public bool alignToCenter = false;
	
	private string accountName = "";
	private string password = "";
	
	public GUISkin LoginSkin;
	
	public float TextFieldIntentWidth = 10;
	public int MaxTextFieldCharacters = 22;
	
	public Vector2 AccountNameTextFieldIntent;
	public Vector2 AccountNameLabelIntent;
	public float AccountNameTextfieldHeight;
	
	public Vector2 PasswordTextFieldIntent;
	public Vector2 PasswordLabelIntent;
	public float PasswordTextfieldHeight;
	
	public Vector2 LoginButtonIntent;
	public float LoginButtonHeight;
	
	private bool rememberAccount = false;
	public Vector2 RememberAccountIntent;
	
	void Start() {
		if (alignToCenter) {
			Position = new Vector2(Screen.width/2, Screen.height/2);
		}
		
		this.GetComponent<LoginSideMenuLayerGUI>().enabled = false;
		
		this.GetComponent<LoginSideMenuLayerGUI>().enabled = true;
	}
	
    void OnGUI() {
		GUI.skin = LoginSkin;
		LoginInterface();

		// AllRights
		GUI.Label( new Rect(
				0,
				Screen.height-25,
				Screen.width,
				20
			),
			"Copyright 2013 LoLWhatTeam. All Right not Reserved.");
		
		GUI.skin = null;
		
		
		// Version
		GUI.Label( new Rect(
				5,
				Screen.height-40,
				300,
				20
			),
			"Version 0.0.1 (Build) (Develop Version)");
		
		// Date Time
		GUI.Label( new Rect(
				5,
				Screen.height-25,
				200,
				20
			),
			System.DateTime.Now.ToString("HH:mm dd MMMM, yyyy"));
    }
	
	private void LoginInterface() {
		GUI.Box(new Rect(Position.x-Size.x/2, Position.y-Size.y/2, Size.x, Size.y), "");
		
		// Account Name
		GUI.Label(new Rect(
				Position.x-Size.x/2+AccountNameLabelIntent.x, 
				Position.y+AccountNameLabelIntent.y, 
				Size.x, 
				20
			), 
			"Account Name");
	    accountName =	GUI.TextField(new Rect(
				Position.x-Size.x/2+TextFieldIntentWidth/2.0f+AccountNameTextFieldIntent.x, 
				Position.y+AccountNameTextFieldIntent.y, 
				Size.x-TextFieldIntentWidth, 
				AccountNameTextfieldHeight
			), 
			accountName,
			MaxTextFieldCharacters);
		
		// Password
		GUI.Label(new Rect(
				Position.x-Size.x/2+PasswordLabelIntent.x, 
				Position.y+PasswordLabelIntent.y, 
				Size.x, 
				20
			), 
			"Password");
		password = GUI.TextField(new Rect(
				Position.x-Size.x/2+TextFieldIntentWidth/2.0f+PasswordTextFieldIntent.x, 
				Position.y+PasswordTextFieldIntent.y, 
				Size.x-TextFieldIntentWidth, 
				PasswordTextfieldHeight
			), 
			password,
			MaxTextFieldCharacters);
		
		// Buttons
		if (GUI.Button(new Rect(
				Position.x-Size.x/2+TextFieldIntentWidth/2.0f+LoginButtonIntent.x, 
				Position.y+LoginButtonIntent.y, 
				Size.x-TextFieldIntentWidth, 
				LoginButtonHeight
			), 
			"Login")) {
			
			ClientInterface client = this.GetComponent<ClientInterface>();
			client.Login();
		}	
		
		// CheckBox
		rememberAccount = GUI.Toggle(new Rect(
				Position.x-Size.x/2+TextFieldIntentWidth/2.0f+RememberAccountIntent.x, 
				Position.y+RememberAccountIntent.y, 
				Size.x-TextFieldIntentWidth, 
				20
			), 
			rememberAccount, 
			"Remember Account Name");
	}
}
