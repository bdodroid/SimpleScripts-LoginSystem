using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using JSONhandler;

public class Login : MonoBehaviour {
	//fields and toggles using unitys new UI system. Create an InputField in the editor (right-click->ui->inputFiled) then attach it from the Hierachy.
	public InputField email; 
	public InputField pass; 
	public InputField pass2;
	public InputField secQ;//I just use an input field for simplicity but you could easily change this to a drop down selection box or something.
	public InputField secA;
	public Toggle autoLogin;

	public GameObject responseText; //a ui text filed we update with information we plan on showing the user, ie. Login Success!/Email not found!

	//variables we want to keep during the enitre session, like PlayerID and Username, etc.
	public static int userID;
	public static string userEmail;
	
	public string URL = ""; //add the address to your php file in this line or the editor (editor overwrites whats here). example: http://yoursite.com/login.php
	public string hash = "theHashCode"; //this is a secret hashcode that needs to match the one you set in your php page. See login.php for example
	
	public void Start(){
		CheckToggle ();
		StartCoroutine(CheckConnection());//check the connection then run FirstRun function
	}
	
	private IEnumerator CheckConnection() {
		Ping pingServer = new Ping("8.8.8.8");
		float startTime = Time.time;
		while (!pingServer.isDone && Time.time < startTime + 2.0f) {
			yield return new WaitForSeconds(0.1f);
		}
		if(pingServer.isDone) {
			LoginSetup(true);//function we run if we have a connection
		} else {
			LoginSetup(false);//function if we do not have a connection
		}
	}

	private void LoginSetup(bool connected){
		if (connected) {
			//check player prefs and login if auto-login is checked
			if(PlayerPrefs.HasKey("email") && PlayerPrefs.HasKey("pass")){
				if(PlayerPrefs.GetInt("autoLogin") == 1){
					email.text = PlayerPrefs.GetString("email");
					pass.text = PlayerPrefs.GetString("pass");
					LoginAccount();
				}else{
					email.text = PlayerPrefs.GetString("email");
				}
			}
		} else {
			if(PlayerPrefs.HasKey("userID") && PlayerPrefs.HasKey("email")){//check for stored login data
				userID = PlayerPrefs.GetInt("userID");
				userEmail = PlayerPrefs.GetString("email");
			}else{//if we have no stored data we create a guest user and continue on to the game.
				userID = -1;
				userEmail = "guest@guest.guest";
				PlayerPrefs.SetInt("userID", userID);
				PlayerPrefs.SetString("email", userEmail);
			}
			//to game scene because we are not using 
		//	Application.loadedLevel(1);
		}
	}

	//used for Unity UI button presses.
	public void LoginAccount(){
		StartCoroutine(WWWSubmit("false"));
	}
	public void CreateAccount(){
		StartCoroutine(WWWSubmit("true"));
	}
	public void ToggleAutoLogin(){
		var toggleValue = 0;
		if (autoLogin.isOn) {
			toggleValue = 1;
		} else {
			toggleValue = 0;
		}
		PlayerPrefs.SetInt("autoLogin", toggleValue);
	}

	private void CheckToggle(){//check if we want to auto login
		if (PlayerPrefs.HasKey ("autoLogin")) {
			var toggleStatus = PlayerPrefs.GetInt ("autoLogin");
			if (toggleStatus == 1) {
				autoLogin.isOn = true;
			}
		}
	}

	IEnumerator WWWSubmit(string creatingAccount) {
		var form = new WWWForm(); //create a new form for submiting

		//here we add all the fields we want to send over. They must match the $_POST["namehere"] in your php script
		form.AddField( "hash", hash ); //hash code must be sent! it is the only thing really securing yout login attempt
		form.AddField( "email", email.text );
		form.AddField( "pass", pass.text );
		form.AddField("pass2", pass2.text);
		form.AddField("securityQuestion", secQ.text);
		form.AddField("securityAnswer", secA.text);
		form.AddField ("creatingAccount", creatingAccount);

		var phpData = new WWW(URL, form); //here we create a variable that submits the form data and returns the response from our php page.

		yield return phpData; //we wait for the response from the server before continuing.

		if (phpData.error != null) {
			responseText.GetComponent<Text>().text = phpData.error; //if for some reason the www failes we report it out here.
		} else {
			//this is assuming you return a JSON string. You can return a single string if you want but JSON is much more flexible, especially when dealing with arrays.
			var parsedData = JSON.Parse(phpData.data);
			if(parsedData != null){//if we get a JSON string back
				userEmail = parsedData[1]["email"]; //an issue where JSON keeps returning slot 0 as empty. Working on it, but in the mean time 1 is the first slot.
				userID = int.Parse(parsedData[1]["ID"]);
				responseText.GetComponent<Text>().text = "CONNECTED";
				SetPlayerPrefs();
			}else{//if we dont get a JSON string back
				responseText.GetComponent<Text>().text = phpData.data; //show the returned string. Login/Failed etc.
			}
			phpData.Dispose(); //clear the stored form data
		}
	}

	private void SetPlayerPrefs(){//add whatever variables you want to store on the device itself here. Im using it here to store auto login detials.
		PlayerPrefs.SetInt("userID", userID);
		PlayerPrefs.SetString("email", userEmail);
		PlayerPrefs.SetString("pass", pass.text);
	}
}
