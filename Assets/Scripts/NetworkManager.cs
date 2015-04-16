using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Camera sceneCamera;
	[SerializeField] Text statusText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] MainMenu mainMenu;

	//main menu panel
	[SerializeField] GameObject connectionPanel;
	[SerializeField] InputField username;

	GameObject player;
	
	void Start () {
		connectionPanel.SetActive (false);
		
		PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
		PhotonNetwork.ConnectUsingSettings ("0.1");

		StartCoroutine ("UpdateConnectionString");
	}
	
	IEnumerator UpdateConnectionString () {
		while (true) {
			statusText.text = PhotonNetwork.connectionStateDetailed.ToString ();
			yield return null;
		}
	}

	public void JoinRoom(){
		PhotonNetwork.player.name = username.text;
		string roomName = mainMenu.roomName;
		if(string.IsNullOrEmpty(roomName)) roomName = username.text;
		RoomOptions ro = new RoomOptions (){isVisible = true, maxPlayers = 10};
		PhotonNetwork.JoinOrCreateRoom (roomName, ro, TypedLobby.Default);
	}

	void OnReceivedRoomListUpdate(){
		mainMenu.ClearButtons ();
		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
		foreach (RoomInfo room in rooms) {
			mainMenu.SetButton(room.name);
		}
	}
	
	void OnJoinedLobby(){
		connectionPanel.SetActive (true);
	}
	
	void OnJoinedRoom()
	{
		connectionPanel.SetActive (false);
		StopCoroutine ("UpdateConnectionString");
		statusText.text = string.Empty;
		StartSpawnProcess (0f);
	}
	
	void StartSpawnProcess (float respawnTime)
	{
		sceneCamera.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}
	
	IEnumerator SpawnPlayer(float respawnTime)
	{
		yield return new WaitForSeconds(respawnTime);
		
		int index = Random.Range (0, spawnPoints.Length);
		player = PhotonNetwork.Instantiate ("FPSCharacter", 
		                                    spawnPoints [index].position,
		                                    spawnPoints [index].rotation,
		                                    0);
		player.GetComponent<PlayerNetworkMover> ().RespawnMe += StartSpawnProcess;
		sceneCamera.enabled = false;
	}
}