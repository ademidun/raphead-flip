using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	public static Main S;
	public static Camera gameCam; 
	public static int[]numPairs = new int[12];
	[HideInInspector]
	public static int cardCount = 0;
	public static GameObject[] CardGO = new GameObject[12];
	public static Card[] battleCards = new Card[2];
	public static System.Random rand = new System.Random();
	public static AudioSource matchSound;
	public static int matchCount=0;
	public static AudioSource kanyeMatchSound;
	public static AudioSource clickSound;
	public static AudioSource winSound;
	public static AudioSource loseSound;
	public static TextMesh gameScoreText;
	public static int gameScore =1000;
	public static TextMesh playingTimeText;
	public static float playingTime;
	public static bool isCounting = false;

	void Awake(){
		if (S == null) {
			S = this;
		}else if(S!=this){
			Destroy (gameObject);
			}
		//perhaps there is a way to do these 9 lines in fewer, cleaner lines of code
		//also do I need different references of go* and go*A, or could I keep using the same GO
		//I could have made a dictionary of sounds

		GameObject go = Resources.Load ("Sounds/MatchSound") as GameObject;
		GameObject go2 = Resources.Load ("Sounds/SoundCard11") as GameObject;
		GameObject go3 = Resources.Load ("Sounds/ClickSound") as GameObject;
		GameObject go4 = Resources.Load ("Sounds/WinSound") as GameObject;
		GameObject go5 = Resources.Load ("Sounds/LoseSound") as GameObject;

		go = Instantiate (go) as GameObject;
		go2 = Instantiate (go2) as GameObject;
		go3 = Instantiate (go3) as GameObject;
		go4 = Instantiate (go4) as GameObject;
		go5 = Instantiate (go5) as GameObject;


		matchSound = go.GetComponent<AudioSource> ();
		kanyeMatchSound = go2.GetComponent<AudioSource> ();
		clickSound = go3.GetComponent<AudioSource> ();
		winSound = go4.GetComponent<AudioSource> ();
		loseSound = go5.GetComponent<AudioSource> ();

		GameObject goA = Resources.Load ("GameScore") as GameObject;
		GameObject go2A = Resources.Load ("PlayingTime") as GameObject;

		goA = Instantiate (goA) as GameObject;
		go2A = Instantiate (go2A) as GameObject;




		gameScoreText = goA.GetComponent<TextMesh> ();

		gameScoreText.text = "Game Score: " + gameScore;

		playingTime = 0f;
		playingTimeText = go2A.GetComponent<TextMesh> ();
		playingTimeText.text = "Elapsed Time: " + playingTime;
		for (int i = 0; i < 6; i++) {
			numPairs [i] = i + 1;
			numPairs [i+6] = i + 1;
		}

		Shuffle (numPairs);




		//Singleton design pattern
		if (S == null) {
			S = this;
		} else if (S != this) {
			Destroy (gameObject);
		}
			
		DontDestroyOnLoad (this);


			
			
	}

	// Use this for initialization
	void Start () {
	}

	void OnLevelWasLoaded( int level){
		if (level > 0) {
            /*Reposition the Camera from Orthogaphic to 
            perspective as level changes from 2D to 3D*/
            
			playingTime = 0f; isCounting = true;
			gameCam = S.GetComponent<Camera> ();
			gameCam.orthographic = false;
			Vector3 pos = S.transform.position;
			pos.x = -1.6f;
			pos.y = 16f;
			pos.z = -21f;
			S.transform.position = pos;
            
			/*I could have used pos again, but this makes code easier to 
			 * understand and more intuitive*/
			S.transform.Rotate (Vector3.up * 0);//set rotation to zero
			S.transform.Rotate (Vector3.right * (45/2));

			LoadCards ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (isCounting == true) {
			playingTime += Time.deltaTime;
		}
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public void PlayGame(){
		SceneManager.LoadScene (1);
	}
		
	public static void Shuffle<T>(T[] list)  
	{  
		int n = list.Length;  
		while (n > 1) {  
			n--;  
			int k = rand.Next (n); 
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public static void LoadCards(){
		int i; int count;
		float z;
		for (count = 0,i=0, z=1f; count < 3; z-=5.5f,count++) {
			int j; 
			float x;
			for (j=0,x = -8f; i<12&&j<4; j++,x+=4f,i++) {
				
				Vector3 pos = new Vector3 (x, 0, z);
				int k = numPairs [i];
				string name = "PlayingCard" + k;
				Quaternion rot = new Quaternion (0f, 180f, 180f, 1);
				if (CardGO [i] == null) {
					CardGO [i] = Instantiate (Resources.Load (name) as GameObject, pos, rot) as GameObject;
					name = "Sounds/SoundCard" + k;
					GameObject soundGO = Resources.Load (name) as GameObject;
					soundGO = Instantiate (soundGO) as GameObject;
					Card card = CardGO [i].GetComponent<Card> ();
					card.cardID = k;
					card.clickSound = soundGO.GetComponent<AudioSource> ();
				}

			}
		}

	}

	public static void ReShuffle(){
		
		playingTime = 0f;
		playingTimeText.text = "Elapsed Time: " +playingTime;
		gameScoreText.text = "Game Score: " + gameScore;
		foreach (GameObject g in CardGO) {
			Destroy (g);
		}
		for(int i=0;i<12;i++){
			CardGO [i] = null;
		}

		Card[] cardTemp = GameObject.FindObjectsOfType<Card> ();
		foreach (Card g in cardTemp) {
			Destroy (g);
		}
		GameObject[] audios = GameObject.FindGameObjectsWithTag ("CardSound");
		foreach (GameObject g in audios) {
			Destroy (g);
		}
		audios = GameObject.FindGameObjectsWithTag ("Sounds");
		foreach (GameObject g in audios) {
			Destroy (g);
		}



		Main.Shuffle (numPairs);
		LoadCards ();
		isCounting = true;
	}
/*
Reshuffle the deck for UI Elements which cannot access static methods

*/
	public void ReShuffleNonStatic(){
		playingTime = 0f;
		matchCount = 0;
		gameScore = 1000;

		playingTimeText.text = "Elapsed Time: " +playingTime;
		gameScoreText.text = "Game Score: " + gameScore;
		foreach (GameObject g in CardGO) {
			Destroy (g);
		}
		for(int i=0;i<12;i++){
			CardGO [i] = null;
		}

		Card[] cardTemp = GameObject.FindObjectsOfType<Card> ();
		foreach (Card g in cardTemp) {
			Destroy (g);
		}
		GameObject[] audios = GameObject.FindGameObjectsWithTag ("CardSound");
		foreach (GameObject g in audios) {
			Destroy (g);
		}
		audios = GameObject.FindGameObjectsWithTag ("Sounds");
		foreach (GameObject g in audios) {
			Destroy (g);
		}



		Main.Shuffle (numPairs);
		LoadCards ();
		isCounting = true;
	}
    ///We are simply comparing cardIDs to see if we have a match
    
	public static void Battle(){
		cardCount = 0;
		int key1 = Main.battleCards [0].cardID, key2 = Main.battleCards [1].cardID;

		if (key1 == key2) {
			matchCount++;
			string name = "PlayingCard" + key1;
			GameObject[] go = GameObject.FindGameObjectsWithTag (name);
			foreach (GameObject g in go) {
				Destroy (g);
			}
			if (key1 == 1) {//we do not want the 'Kanye' card (card1) to play on 
				//the second card
				Main.kanyeMatchSound.Play ();
			} else {
				Main.matchSound.Play ();
			}

			if (Main.matchCount>=6) {
				playingTimeText.text = "Nice, you won in: " +playingTime+"s";
				winSound.Play ();
				S.Invoke ("MatchWon", 2.5f);
			}
				
		} //end if (key1==key2)

		else {
			gameScore -= 40;
			gameScoreText.text = "Game Score: " + gameScore;
			for (int i = 0; i < 12; i++) {
				if (CardGO [i] != null) {//make sure we are not flipping a deleted card
					Card card = CardGO [i].GetComponent<Card> ();
					card.isFlipped = false;
				}
			}

			if (gameScore <= 0) {
				loseSound.Play ();
				isCounting = false;
				matchCount = 0;
				playingTimeText.text = "Elapsed Time: " +playingTime;
				gameScoreText.text = "Sorry, You Lost";
				S.Invoke ("MatchLost", 2.5f);
			}

		}


	}


	public void MatchWon(){
		isCounting = false;
		matchCount = 0;
		playingTime = 0;
		ReShuffle ();
	}

		public void MatchLost(){
		gameScore = 100;
		matchCount = 0;
		gameScoreText.text = "Game Score: " + gameScore;
			ReShuffle ();
		}


}
