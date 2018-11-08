using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour {

	public GameObject ConsoleTextItem;

	private List<ConsoleItem> ConsoleItems = new List<ConsoleItem> ();
	private Vector3 startPos = new Vector3 (-492, 292, 0);

	public void OutputToConsole(string Message, Color color){
		int yOffset = ConsoleItems.Count * -16;
		GameObject newConsoleItem = Instantiate (ConsoleTextItem, this.gameObject.transform);
		newConsoleItem.transform.localPosition = startPos + new Vector3 (0, yOffset, 0);
		newConsoleItem.GetComponent<Text> ().text = Message;
		newConsoleItem.GetComponent<Text> ().color = color;
		ConsoleItems.Add (new ConsoleItem(newConsoleItem));
	}

	private void UpdateList(){
		for (int i = 0; i < ConsoleItems.Count; i++) {
			int yOffset = i * -16;
			ConsoleItems[i].consoleItem.transform.localPosition = startPos + new Vector3 (0, yOffset, 0);
		}
	}

	void Update(){
		for (int i = 0; i < ConsoleItems.Count; i++) {
			if (ConsoleItems [i].timer <= 0) {
				Destroy (ConsoleItems [i].consoleItem);
				ConsoleItems.RemoveAt (i);
				UpdateList ();
			}
		}
	}

	void FixedUpdate(){
		foreach (ConsoleItem c in ConsoleItems) {
			c.timer--;
		}
	}
}

public class ConsoleItem
{

	public GameObject consoleItem;
	public float timer = 300;

	public ConsoleItem(GameObject ConsoleItem){
		consoleItem = ConsoleItem;
	}
}
