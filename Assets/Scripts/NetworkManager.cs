using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
	
	[SerializeField] Text statusText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] MainMenu mainMenu;

	GameObject player;
	
	void Start () {
		
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
	
	void OnJoinedLobby()
	{
		RoomOptions ro = new RoomOptions (){isVisible = true, maxPlayers = 10};
		PhotonNetwork.JoinOrCreateRoom ("TestRoom", ro, TypedLobby.Default);
	}
	
	void OnJoinedRoom()
	{
		StopCoroutine ("UpdateConnectionString");
		statusText.text = string.Empty;
		//StartSpawnProcess (0f);
	}
	
	void StartSpawnProcess (float respawnTime)
	{
		//sceneCamera.enabled = true;
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
		//sceneCamera.enabled = false;
	}
}