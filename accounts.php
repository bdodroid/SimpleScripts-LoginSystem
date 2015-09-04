<?
/////////////////////////
//if you are having promblems please make sure that the table and column names in your database match the ones being used in the sql below
/////////////////////////
// Connection INFO ----------------------------------------------------------
//FILL THIS OUT
$host = "localhost"; //host location (use localhost if your mysql database is hosted on the same machine/account as your site)
$user = ""; //username
$password = ""; //password here
$dbname = ""; //your database
$connection = mysqli_connect($host,$user,$password,$dbname) or die("Error " . mysqli_error($connection));

//--------------------------------------------------------------------------------------------------------------
// Here we protect ourselves from SQL Injection and convert the string to MD5 if we want
function anti_injection_login($sql, $formUse, $encrypt){
	$sql = preg_replace("/(from|select|insert|delete|where|drop table|show tables|,|'|#|\*|--|\\\\)/i","",$sql);
	$sql = trim($sql);
	$sql = strip_tags($sql);
	if(!$formUse || !get_magic_quotes_gpc())
		$sql = addslashes($sql);
		if($encrypt){
			$sql = md5(trim($sql));
		}
	return $sql;
}

//--------------------------------------------------------------------------------------------------------------
$unityHashPass = anti_injection_login($_POST["hash"],true,false);
$phpHashPass = "theHashCode"; // must be the same code you set in unity
$email = anti_injection_login($_POST["email"],true,false);
$pass = anti_injection_login($_POST["pass"],true,true);
$pass2 = anti_injection_login($_POST["pass2"],true,true);
$secQ = anti_injection_login($_POST["securityQuestion"],true,false);
$secA = anti_injection_login($_POST["securityAnswer"],true,false);
$creatingAccount = $_POST["creatingAccount"];

//check if our hashpass's are the same and if an email and password where sent.
if ($unityHashPass != $phpHashPass || !$email || !$pass){
	echo "Username or password can not be empty.";
} else {//if they are the same
	if($creatingAccount == "true"){//if we are creating an account. Variable is sent from the WWWSubmit function in Login.cs in Unity
		$SQL = "SELECT email FROM Accounts WHERE email = '" . $email . "'";
		$result_id = mysqli_query($connection, $SQL) or die("Error in Selecting " . mysqli_error($connection));
		$results = mysqli_num_rows($result_id);
		if($results > 0) {
			echo "That account already exists.";
		}else{
			if(!$secQ || !$secA || !$pass2){
				echo "Please fill out all fields.";
			}else{
				if($pass == $pass2){
					$SQL = "INSERT INTO Accounts (`email`, `password`, `secretQuestion`, `answer`)
							VALUES ('". $email ."', '". $pass ."', '". $secQ ."','". $secA ."')";
					$result = mysqli_query($connection, $SQL) or die("DATABASE ERROR!");
						echo "Account created.";
				}else{
					echo "Passwords must match";
				}
			}
		}
	}else{
		$SQL = "SELECT * FROM Accounts WHERE email = '" . $email . "'";
		$result = mysqli_query($connection, $SQL) or die("Error in Selecting " . mysqli_error($connection));
		$results = mysqli_num_rows($result);
		$temparray[] = array();

		while($row = mysqli_fetch_assoc($result)){
			$temparray[] = $row;
			$comPass = $row['password'];
		}

		if($results) {
			if(!strcmp($pass,$comPass)) {
				echo json_encode($temparray);
			} else {
				echo "Login or password incorrect.";
			}
		} else {
			echo "Email doesnt exist.";
		}
	}
}
mysql_close();
?>
