# SimpleScripts-LoginSystem
A simple login system for unity.

Server Side:
Copy accounts.php to you server.
	eg, http://yoursite.com/accounts.php
Create a database(any name) with:
1 table:	Accounts
4 columns:	email, password, secretQuestion, answer

Then fill in the host, database, username, and password variables in your accounts.php file.


Client/App side:
	Drag Login.cs onto an object in your scene. (I use an empty game object called LoginController)
	Create a UI Canvas and add,
		InputFields: email,password,password2,securityQuestion,securityAnswer
		ToggleSwitch: autoLogin
		Buttons: login, create account
		Text field: results

	Attach InputField objects, toggle switch, and text field, to your Login.cs script on your control object.
	On your login button add an OnClick(by pressing the little + symbol) and drag your login control object to the object slot, then from the drop down box select Login->LoginAccount()
	Do the same with your create account button on this time Login->CreateAccount()

	Get the asset pack on the asset store for a dollar and help support future Simple Scripts! <a href="http://u3d.as/jmX">Asset Store</a>
	Asset store includes a working example scene.

