using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] InputField playerName;
	[SerializeField] RectTransform buttonHolder;
	[SerializeField] VerticalLayoutGroup group;
	[SerializeField] GameObject listButtonPrefab;

	// Use this for initialization
	void Start () {
		for (int i=0; i<4; i++) {
			SetButton("server " + i);
		}
	}

	public void SetButton (string serverName){
		GameObject button = Instantiate(listButtonPrefab) as GameObject;
		button.transform.parent = buttonHolder;
		button.transform.localPosition = Vector3.zero;
		Vector2 size = buttonHolder.sizeDelta;
		size.y += 35.0f;
		buttonHolder.sizeDelta = size;
		button.GetComponentInChildren<Text> ().text = serverName;

		Button btn = button.GetComponent<Button> ();
		btn.onClick.AddListener(() => { 
			OnServerButtonClick(button);
		});

	}

	void OnServerButtonClick(GameObject go){
		Debug.Log (go.GetComponentInChildren<Text>().text);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
