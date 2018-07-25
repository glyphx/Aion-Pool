using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.Serialization;
using System.Linq;
using System.Linq.Expressions;


public class DataHandler : MonoBehaviour {
	public GameObject inputField;
	public GameObject statusText;

	public GameObject dataSegment1;
	public GameObject dataSegment2;
	public GameObject dataSegment3;

	public GameObject dataWindow1;
	public GameObject dataWindow2;

	public UnityEngine.UI.Dropdown Rigs;

	public string urlBase = "https://aionmine.org:22200/api/pools/aion/miners/";

	// Use this for initialization
	void Start () {
		//string url = "https://aionmine.org:22200/api/pools/aion/miners/0xa01394cb07a68c0079c23708a7be1212b9b8b4c1e7cc5c3c67b2b76530a8a449";
		//WWW www = new WWW (url);
		//StartCoroutine(WaitForRequest(www));

		Rigs.ClearOptions ();
	}

	IEnumerator WaitForRequest(WWW www) {
		Debug.Log("about to send request");
		yield return www;
		Debug.Log("request sent");


		if(www.error == null) {
			Debug.Log("www OK!  " + www.text);
			UserInfo data = JsonUtility.FromJson<UserInfo>(www.text);



			Debug.Log (data.pendingShares);
			Debug.Log (data.pendingBalance);
			Debug.Log (data.totalPaid);
			Debug.Log (data.performance);
			Debug.Log (data.hashrate);
			Debug.Log (data.sharesPerSecond);

			/*
			string jsonResult = 
				System.Text.Encoding.UTF8.GetString(www.ToString());

			RootObject[] entities = Helper.getJsonArray<RootObject>(www.text);
			Rigs.options.AddRange(
				entities.Select(x =>
					new UnityEngine.UI.Dropdown.OptionData()
					{
						text = x.performance
					
				}).ToList());
			*/


			//Need to parse json and display here instead...
			if (www.text.Length < 40) {
				//send error mesage, stay search mode
				//dataSegment1.GetComponent<Text> ().text = "json return very short... +" + www.text.Substring (0, dataSegment1.GetComponent<Text> ().text.Length);
			} else if(www.text.Length > 1000) {
				dataSegment1.GetComponent<Text> ().text = "Pending Shares: " + data.pendingShares;
				dataSegment2.GetComponent<Text> ().text = "Pending Balance: " + data.pendingBalance;
				dataSegment3.GetComponent<Text> ().text = "Total Paid: " + data.totalPaid;
			}

			statusText.GetComponent<Status_Indicator> ().SetLoaded();
			dataWindow1.SetActive (true);
			dataWindow2.SetActive (true);
			//activate dataFrame;
		}
		else {
			// Show results as text
			Debug.Log("www error!  " + www.error);
			statusText.GetComponent<Status_Indicator> ().IndicateError (www.error);
		}
	}

	public void SearchAndLoad(){
		dataWindow1.SetActive (false);
		dataWindow2.SetActive (false);
		statusText.GetComponent<Status_Indicator> ().SetLoading();
		string url = urlBase + inputField.GetComponent<Text> ().text;
		WWW www = new WWW (url);
		StartCoroutine(WaitForRequest(www));
	}

	public void Exit(){
		Application.Quit ();
	}

	[System.Serializable]
	public class UserInfo
	{
		public string pendingShares;
		public string pendingBalance;
		public string totalPaid;
		public string performance;
		public string hashrate;
		public string sharesPerSecond;

		// Given JSON input:
		// {"name":"Dr Charles","lives":3,"health":0.8}
		// this example will return a PlayerInfo object with
		// name == "Dr Charles", lives == 3, and health == 0.8f.
	}

}
