using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SocketIOClient;
using System;
using AssemblyCSharp;
using SimpleJSON;

public class ClientInterface : MonoBehaviour {
	
	private Client client;
	private Vector2 scrollPosition;
	private string uiMessage;
	private bool conected = false;
	private string chatString = "";
	
	public GameObject Node;
	public GameObject PlayerModel;
	
	private GameObject playerModel;
	
	private Transform nodePosition;
	
	private GameObject currentPlayer;
	private Vector3 currentPosition;
	
	private GameObject game;
	private GameObject player;
	private GameObject players;
	
	private List<Player> playersToAdd;
	private Dictionary<string, Player> allPlayers;
	private List<Player> removeList;
	
	private string currentPlayerName = "test";
	
	private bool tryLogin = false;
	
	private const string LoginCommand = "{\"type\":\"PlayerLogin\",\"name\":\"test\"}";
	
	private bool playerChanged = false;
	private Vector3 updatePosition;
	private Vector3 moveToPosition;
	
	// Use this for initialization
	void Start () {
		playersToAdd = new List<Player>();
		allPlayers = new Dictionary<string, Player>();
		removeList = new List<Player>();
		
		client = new Client("http://birhoff.dyndns.org:3000");
		
		client.Opened += SocketOpened;
		client.Message += SocketMessage;
		client.SocketConnectionClosed += SocketConnectionClosed;
		client.Error += SocketError;
		
		client.Connect();
		
		game = new GameObject("game");
		player = new GameObject("player");
		player.transform.parent = game.transform;
		
		players = new GameObject("players");
		players.transform.parent = game.transform;
		
		StartCoroutine(updateCheck());
	}
	
	IEnumerator updateCheck()
	{
		yield return new WaitForSeconds(0.1f);
		
		if (tryLogin) {
			if (playerChanged) {
				
				string posX = updatePosition.x.ToString();
				string posY = updatePosition.y.ToString();
				string posZ = updatePosition.z.ToString();
				string sendData = "{\"type\":\"PlayerUpdate\",\"position\":\""+posX+";"+posY+";"+posZ+"\"}";
				
				client.Send(sendData);
				playerChanged = false;
			}
		}
		
		StartCoroutine(updateCheck());
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Player pl in playersToAdd) 
		{
			pl.Model = (GameObject)Instantiate(Node, pl.Position, Quaternion.identity);
			if (pl.Name == currentPlayerName) {
				pl.Model.transform.parent = player.transform;
				currentPlayer = (GameObject)pl.Model;
				this.transform.LookAt(pl.Position);
				
				playerModel = (GameObject)Instantiate(PlayerModel, pl.Position, Quaternion.identity);
			} else {
				pl.Model.transform.parent = players.transform;
			}
			
			allPlayers.Add(pl.Name,pl);
		}
		
		playersToAdd.Clear();
		
		if (currentPlayer!=null) {

			if (Input.GetKey(KeyCode.A)) 
			{
				updatePosition.x-=0.1f;
				playerChanged = true;
			}
			
			if (Input.GetKey(KeyCode.D)) 
			{
				updatePosition.x+=0.1f;
				playerChanged = true;
			}
			
			if (Input.GetKey(KeyCode.W)) 
			{
				updatePosition.z+=0.1f;
				playerChanged = true;
			}
			
			if (Input.GetKey(KeyCode.S)) 
			{
				updatePosition.z-=0.1f;
				playerChanged = true;
			}
			
			if (currentPlayer.transform.position.x<moveToPosition.x-0.2f)
			{
				currentPlayer.transform.Translate(0.1f,0,0);
			} else {
				if (currentPlayer.transform.position.x>moveToPosition.x+0.2f)
				{
					currentPlayer.transform.Translate(-0.1f,0,0);
				}	
			}
			
			if (currentPlayer.transform.position.z<moveToPosition.z-0.2f)
			{
				currentPlayer.transform.Translate(0,0,0.1f);
			} else {
				if (currentPlayer.transform.position.z>moveToPosition.z+0.2f)
				{
					currentPlayer.transform.Translate(0,0,-0.1f);
				}	
			}
			
			playerModel.transform.position = Vector3.Lerp(playerModel.transform.position, currentPlayer.transform.position,Time.deltaTime*5.0f);
		}
		
		
		foreach(Player pl in allPlayers.Values)
		{
			if (pl.canKill) {
				removeList.Add(pl);	
			} 
			
			if (pl.Name!=currentPlayerName) 
				pl.Model.transform.position = pl.Position;
		}
		
		foreach(Player pl in removeList)
		{
			Destroy(pl.Model);
			allPlayers.Remove(pl.Name);
		}
		
		removeList.Clear();
	}
	
	private void SocketOpened(object sender, EventArgs e) {
    	//invoke when socket opened
		uiMessage += "Socket Open\n";
		conected = true;
		
		client.On("test", (data)=> {
		});
	}
	
	private void SocketMessage(object sender, SocketIOClient.MessageEventArgs e) {
	   	if ( e!= null && e.Message.Event == "message") {
	        string msg = e.Message.MessageText; 
			//uiMessage += "Success: "+msg+"\n";
			
			if (!tryLogin) return;
			
			var node = SimpleJSON.JSON.Parse(msg);
			
			if (node["type"].Value=="PlayerJoin")
			{
				var data = node["data"];
				
				string[] position = data["position"].ToString().Replace("\"","").Split(';');
				Player tmpPlayer = new Player(data["name"], 
					new Vector3(
						float.Parse(position[0]), 
						float.Parse(position[1]), 
						float.Parse(position[2]))
					);
				
				playersToAdd.Add(tmpPlayer);
			}
			
			if (node["type"].Value=="PlayerLogin") 
			{
				var data = node["data"];
				
				string[] position = data["position"].ToString().Replace("\"","").Split(';');
				Player tmpPlayer = new Player(data["name"], 
					new Vector3(
						float.Parse(position[0]), 
						float.Parse(position[1]), 
						float.Parse(position[2]))
					);
				
				updatePosition = tmpPlayer.Position;
				moveToPosition = updatePosition;
				
				JSONArray otherPLayers = data["players"].AsArray;
				foreach (var objJSON in otherPLayers.Childs) 
				{
					
					string[] othPosition = objJSON["position"].ToString().Replace("\"","").Split(';');
					Player othPlayer = new Player(objJSON["name"], 
					new Vector3(
						float.Parse(othPosition[0]), 
						float.Parse(othPosition[1]), 
						float.Parse(othPosition[2]))
					);
					
					playersToAdd.Add(othPlayer);
				}
				
				playersToAdd.Add(tmpPlayer);
				return;
			}
			
			if (node["type"].Value=="PlayerUpdate")
			{
				var data = node["data"];
				
				if (allPlayers.ContainsKey(data["name"]))
				{
					string[] position = data["position"].Value.Split(';');
					updatePosition = new Vector3(
							float.Parse(position[0]),
							float.Parse(position[1]),
							float.Parse(position[2])
						);
					moveToPosition = updatePosition;
				}
			}
			
			if (node["type"].Value=="PlayerDisconnect")
			{
				var data = node["data"];
				
				if (allPlayers.ContainsKey(data["name"]))
				{
					allPlayers[data["name"]].canKill=true;
				}	
			}
	    }
	}
	
	IEnumerator moveNode(Vector3 moveTo)
	{
		yield return null;
		
		Node.transform.Translate(moveTo);
	}
	
	private void SocketConnectionClosed(object sender, EventArgs e) {
		uiMessage += "SocketClose\n";
		client.Close();
	}
	
	private void SocketError(object sender, SocketIOClient.ErrorEventArgs e) {
    	if ( e!= null) {
	        string msg = e.Message;
			uiMessage += "Error: "+msg+"\n";
	    }
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(Screen.width-110,Screen.height-30-10,100,30), "Send Message")) {
			if (conected) {
				client.Send("teeest");
				
			}
		}
		
		Event e = Event.current;
		if (e.keyCode == KeyCode.Return)
		{
			if (GUI.GetNameOfFocusedControl()=="CMD") 
			{
				if (chatString.Length>0) 
				{
					if (chatString=="login") {
						client.Send(LoginCommand);
						tryLogin = true;
					}
					
					chatString="";
				}
				
				GUI.SetNextControlName("");
				GUI.FocusControl("");
			} else {
				GUI.FocusControl("CMD");
			}
		}
		
		GUI.SetNextControlName("CMD");
		chatString = GUI.TextField(new Rect(10,Screen.height-30-10,Screen.width-130,30), chatString);
		
		/*GUI.SetNextControlName("DebugTextView");
		GUILayout.BeginArea(new Rect(50, 50, Screen.width-100, 100));
		
 			scrollPosition = GUILayout.BeginScrollView (
			scrollPosition, 
			GUILayout.Width (Screen.width-100), 
			GUILayout.Height (Screen.height-100));
		
			GUILayout.Label (uiMessage);
		 	GUILayout.EndScrollView ();
		 
		 GUILayout.EndArea();*/
		
	}
	
	
	void OnApplicationQuit() {
		if (conected) {
			client.Close();	
		}
    }
}