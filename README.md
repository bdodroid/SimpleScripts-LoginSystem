# SimpleScripts-LoginSystem
A simple login system for unity.

## Server Side:
 - Copy accounts.php to you server.
	eg, http://yoursite.com/accounts.php

- Create a database(any name) with:
  - 1 table:	Accounts
  - 4 columns:	email, password, secretQuestion, answer

- Open accounts.php and fill in the host, database, username, and password variables at the top of the file.


## Client/App side:
- Import Login.cs to your Unity project (you can drag an drop from windows explorer)
- Drag Login.cs onto an object in your scene. (I use an empty game object called LoginController)
- Create a UI Canvas and add these elements to it:
  - InputFields: email,password,password2,securityQuestion,securityAnswer
  - ToggleSwitch: autoLogin
  - Buttons: login, create account
  - Text field: results
- Attach the UI objects you just created to the corresponding location on your LoginController/Login.cs script in the editor.
- On your "Login" button add an OnClick(by pressing the little + symbol) and drag your login control object to the object slot, then from the drop down box select Login->LoginAccount()
-Do the same with your "Create account" button on this time Login->CreateAccount()

