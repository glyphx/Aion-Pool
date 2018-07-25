using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status_Indicator : MonoBehaviour {

	public bool inactive = false;
	public bool loading = false;

	public float loadIndicatorTime = 0.0f;
	public float loadTime = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (loading) {
			loadIndicatorTime += Time.deltaTime;
			loadTime += Time.deltaTime;
			if(loadIndicatorTime < 0.2f){
				GetComponent<Text>().text = "loading";
			}
			else if(loadIndicatorTime < 0.4f){
				GetComponent<Text>().text = "loading.";
			} 
			else if(loadIndicatorTime < 0.6f){
				GetComponent<Text>().text = "loading..";
			}
			else if(loadIndicatorTime < 0.8f){
				GetComponent<Text>().text = "loading..";
			}
			else{
				loadIndicatorTime = 0.0f;
			}

			if(loadTime > 10.0f){
				IndicateError ("Error: Failed to load before timeout.  Please check your internet connection.");
				loadTime = 0.0f;
				loadIndicatorTime = 0.0f;
				loading = false;
			}
		}
	}

	public void SetLoading(){
		loading = true;
	}

	public void SetLoaded(){
		GetComponent<Text>().text = "";
		loading = false;
	}

	public void IndicateError(string error){
		loadTime = 0.0f;
		loadIndicatorTime = 0.0f;
		loading = false;

		GetComponent<Text>().text = "Error: " + error;
	}
}
