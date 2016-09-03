using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	//If there was c# equivalent of C++ I would have made Main Class a friend to Card Class
	//There is a rough equivalent called InternalsVisibleTo but it seems a bit superflous to our needs

	public bool isFlipped = false;
	float speed = 4f;
	Quaternion finalRot;
	Vector3 finalHeight;
	public int cardID;
	public AudioSource clickSound;

	// Use this for initialization
	void Start () {
		//this is kind of redundant but perhaps, it makes code easier to understand and intuitive
		finalRot = this.transform.rotation;
		finalHeight = this.transform.position;
	}

	// Update is called once per frame
	void Update () {
        //Animation sequence using ternary operators
        //to find status of the card and animate accordingly
		finalRot = Quaternion.AngleAxis (isFlipped ? 0 : 180, Vector3.right);//set rotation to zero
		finalHeight.y = isFlipped ? 3 : 0;
		transform.position = Vector3.Slerp (transform.position, finalHeight, Time.deltaTime * speed);
		transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, Time.deltaTime*speed);
	}
		
	public void OnMouseUp(){
		if (!isFlipped && Main.cardCount!=2) {
			Main.cardCount++;
			isFlipped = !isFlipped;
			clickSound.Play ();
            
			if (Main.cardCount == 1) {
				Main.battleCards [0] = this;
			} else if (Main.cardCount == 2) {
				Main.battleCards [1] = this;
                
				Invoke ("CardBattleHandler", 1f);
			}

		}
	}
    
    //Invoked with a delay to give animations time to show in Gameplay
	void CardBattleHandler(){
		Main.Battle ();
	}


}
