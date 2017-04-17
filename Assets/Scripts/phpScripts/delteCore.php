<?php
date_default_timezone_set("Australia/Melbourne");


final class DeltaCoreDB {
	
	private $debugOn = true ;
	private localLog( $msg) { localLog("LevelData", $msg); }
    private localLog( $topic,  $msg)
    {
        if (debugOn)
        {
			print date(DATE_RFC2822) + "[$topic] $msg"
        }
    }
	
	
	private $pdo;
	private $DELTACORE_HOST			= "130.56.248.86";
	private $DELTACORE_USERNAME		= "dcremote";
	private $DELTACORE_PASSWORD		= "1qazxcde3@WS";
	private $DELTACORE_DBNAME		= "playertw_deltacore";
	
	// USER
	// >> Table constants
	private $USR_TBL_NAME 		= $DELTACORE_DBNAME + ".users";
	private $USR_TBL_KEY 		= "id";
	private $USR_TBL_ROLE 		= "role";
	private $USR_TBL_STATUS 	= "status";
	private $USR_TBL_FIRSTNAME 	= "first_name";
	private $USR_TBL_LASTNAME 	= "last_name";
	private $USR_TBL_EMAIL 		= "email";
	private $USR_TBL_APPROVER 	= "approver";
	private $USR_TBL_SALT 		= "pw_salt";
	private $USR_TBL_HASH 		= "pw_hash";
	private $USR_TBL_PW_RESET 	= "pw_reset_hash";
	private $USR_TBL_PW_RESET_TS = "pw_reset_hash_ts";
	private $USR_TBL_AO 		= "added_on";
	private $USR_TBL_AB 		= "added_by";
	
	public static function i()
	{
		static $inst = null;
		if ($inst === null) {
			$inst = new DeltaCoreDB();
		}
		return $inst;
	}
	
	private function __construct()
	{
		try {
			$this->pdo = new PDO(
				'mysql:host=' . $this->DB_HOST . ';'
				.'dbname=' . $this->DB_NAME . ';'
				.'charset=utf8',
				$this->DB_USERNAME, $this->DB_PASSWORD);
		} catch (PDOException $ex) {
			if ($this->debugMode) {
				var_dump($ex);
			}
		}
	}
	
	function recordIsInTable($record_value, $record_name, $table_name)
	{
		$SQL = "SELECT *
				FROM 	$table_name
				WHERE	$record_name=:record_value
				";
		$stmt = $this->pdo->prepare($SQL);
		$stmt->bindValue(':record_value', $record_value, PDO::PARAM_STR);
		$stmt->execute();
		$result = $stmt->fetchAll(PDO::FETCH_ASSOC);
		if ($result != -1 && sizeof($result) > 0) {
			return true;
		} else {
			return false;
		}
	}
	
	function executeSetStatement($statement)
	{
		$this->pdo->exec($statement);
	}

	function loginUser($mail, $password)
	{
		// Get User Info 
		$SQL = "
			SELECT
				$this->USER_TABLE_KEY,
				$this->USER_TABLE_SALT
			FROM 	$this->USER_TABLE_NAME
			WHERE	$this->USER_TABLE_EMAIL=:mail
		";
		
		$stmt = $this->pdo->prepare($SQL);
		$stmt->bindValue(':mail', $mail, PDO::PARAM_STR);
		$stmt->execute();
		$result = $stmt->fetchAll(PDO::FETCH_ASSOC);
		if ( ! $result){
			localLog("user $mail not found"); 
		}
		
		$user_id = $result[0][$this->USER_TABLE_KEY];
		$salt = $result[0][$this->USER_TABLE_SALT];
		$password_hash = $this->getPasswordHash($password, $salt);
		$status = $this->mapUserStatus('approved');
		
		$SQL = "SELECT $this->USER_TABLE_KEY
				FROM $this->USER_TABLE_NAME
				WHERE
					$this->USER_TABLE_HASH = :password_hashAND
					$this->USER_TABLE_STATUS = :status
					AND
					$this->USER_TABLE_EMAIL = :mail
				";
		$stmt = $this->pdo->prepare($SQL);
		$stmt->bindValue(':password_hash', $password_hash, PDO::PARAM_STR);
		$stmt->bindValue(':status', $status, PDO::PARAM_INT);
		$stmt->bindValue(':mail', $mail, PDO::PARAM_STR);
		$stmt->execute();
		$result = $stmt->fetchAll(PDO::FETCH_ASSOC);
		if (sizeof($result) != 1) {
			$this->handleFailedLogin($user_id);
			return false;
		} else {
			$this->removeFailedLoginAttempts($user_id);
			return $this->getUser($result[0][$this->USER_TABLE_KEY]);
		}
	}
	