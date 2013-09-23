using UnityEngine;
using System.Collections;

public class LoginSideMenuLayerGUI : MonoBehaviour {
	
	public Vector2 Size;
	public Vector2 Position;
	
	public bool alignToCenter = false;

	public GUISkin LoginSkin;
	
	public Vector2 ConfigButtonIntent;
	
	public Texture2D ConfigButtonTexture;
	public Texture2D ConfigButtonTextureHover;

	private GUIStyle configButtonStyle;
	
	
	public Vector2 ServerButtonIntent;
	
	public Texture2D ServerButtonTexture;
	public Texture2D ServerButtonTextureHover;

	private GUIStyle serverButtonStyle;
	
	void Start() {
		if (alignToCenter) {
			Position = new Vector2(Screen.width/2, Screen.height/2);
		}
		
		configButtonStyle = new GUIStyle();
		configButtonStyle.normal.background = ConfigButtonTexture;
		configButtonStyle.hover.background = ConfigButtonTextureHover;
		
		serverButtonStyle = new GUIStyle();
		serverButtonStyle.normal.background = ServerButtonTexture;
		serverButtonStyle.hover.background = ServerButtonTextureHover;
	}
	
    void OnGUI() {
		GUI.skin = LoginSkin;
		SideMenu();
		GUI.skin = null;
    }
	
	private void SideMenu() {
		GUI.Box(new Rect(Position.x-Size.x/2, Position.y-Size.y/2, Size.x, Size.y), "");
		
		
		GUI.Button(new Rect(
				Position.x-Size.x/2+ConfigButtonIntent.x, 
				Position.y+ConfigButtonIntent.y, 
				ConfigButtonTexture.width,
				ConfigButtonTexture.height
			),
			"", 
			configButtonStyle);
		
		
		GUI.Button(new Rect(
				Position.x-Size.x/2+ServerButtonIntent.x, 
				Position.y+ServerButtonIntent.y, 
				ServerButtonTexture.width,
				ServerButtonTexture.height
			),
			"", 
			serverButtonStyle);
	}
}
