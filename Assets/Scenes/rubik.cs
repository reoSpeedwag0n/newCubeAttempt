using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * -Lucas Howard
 * this script manages the turning of the cube's parts, rotating the sides in a given direction.
 */


public class rubik : MonoBehaviour {

	[Header("Static Game Objects")]
	public GameObject centerOfTheCube;		//the "Empty" to which all the pieces (and this script) are parented
	public GameObject[] centerPieces;		//White, Green, Red, Yellow, Blue, Orange	||		top, front, right, bottom, back, left
	public List<GameObject> allPieces;		//as the name says, every physical piece of the cube.

	[Header("Variable Game Objects")]
	public GameObject turnstile = null;			//this could technically be a pointer; all it does is point to a given empty that already exists--the one turning.
	public bool CubeIsRotating = false;
	public Vector3 destinationOrientation;

	[Header("User Inputs")]
	public KeyCode reverseButton = KeyCode.LeftShift;
	public KeyCode	turnTop = KeyCode.UpArrow, turnBottom = KeyCode.DownArrow,
					turnLeft = KeyCode.LeftArrow, turnRight = KeyCode.RightArrow,
					turnFront = KeyCode.LeftAlt, turnBack = KeyCode.RightAlt;
	public KeyCode turnCubeX = KeyCode.X, turnCubeY = KeyCode.Y, turnCubeZ = KeyCode.Z;
	public float turnSpeed = 25f;			//this is set to negative if the user holds shift (UBL --> U'B'L')
	public bool shiftIsBeingHeld = false;


	public void Update(){

		//is something moving?
		if (CubeIsRotating) {
			//rotate stuff smoothly
			turnstile.GetComponent<Transform>().RotateAround(turnstile.transform.position, turnSpeed * Time.deltaTime);

			//is it done rotating?
			if (Mathf.Round (turnstile.transform.eulerAngles.x / 3) % 30f == 0f &&
			    Mathf.Round (turnstile.transform.eulerAngles.y / 3) % 30f == 0f &&
			    Mathf.Round (turnstile.transform.eulerAngles.z / 3) % 30f == 0f) {		//0 || 90 || 180 || 270

				//Vector3 newRotation = new Vector3 (Mathf.Round (turnstile.transform.eulerAngles.x),
				//	                      Mathf.Round (turnstile.transform.eulerAngles.y),
				//	                      Mathf.Round (turnstile.transform.eulerAngles.z));
				//turnstile.GetComponent<Transform> ().eulerAngles = newRotation;
				turnstile.GetComponent<Transform>().eulerAngles = destinationOrientation;
				turnstile = null;
				CubeIsRotating = false;
			}

		} else {
			//is user inputting stuff?

			if (Input.GetKeyDown (reverseButton)) {
				turnSpeed *= -1f;
				shiftIsBeingHeld = true;
			} else if (Input.GetKeyUp (reverseButton)) {
				turnSpeed *= -1f;
				shiftIsBeingHeld = false;
			}


			if (Input.GetKeyDown (turnTop)) {
				turnstile = GetCenterPiece ("top");
				destinationOrientation = turnstile.transform.eulerAngles + Vector3.up * 90f;	//times negative one, if shift
				if (shiftIsBeingHeld)
					destinationOrientation *= -1f;
			}
			if (Input.GetKeyDown (turnFront)) {
				turnstile = GetCenterPiece ("front");
				destinationOrientation = turnstile.transform.eulerAngles + Vector3.back * 90f;
				if (shiftIsBeingHeld)
					destinationOrientation *= -1f;
			}
			if (Input.GetKeyDown (turnRight)) {
				turnstile = GetCenterPiece ("right");
				destinationOrientation = turnstile.transform.eulerAngles + Vector3.right * 90f;
				if (shiftIsBeingHeld)
					destinationOrientation *= -1f;
			}
			if (Input.GetKeyDown (turnBottom))
				turnstile = GetCenterPiece ("bottom");
			if (Input.GetKeyDown (turnBack))
				turnstile = GetCenterPiece ("back");
			if (Input.GetKeyDown (turnLeft))
				turnstile = GetCenterPiece ("left");
			if (Input.GetKeyDown (turnCubeX))
				turnstile = GetCenterPiece ("x");


			if (turnstile != null)
				//CubeIsRotating = true;
				AssignPiecesToTurnstile();
		}

	}




	public void AssignPiecesToTurnstile(){		//this will set every piece on turnstile's side to be a child of turnstile

		foreach (GameObject piece in allPieces) {
			if (turnstile.transform.position.x > 0f) {	//right
				if (piece.transform.position.x > 0f)
					piece.GetComponent<Transform> ().SetParent (turnstile.transform);
			}
			if (turnstile.transform.position.x < 0f) {	//left
				if (piece.transform.position.x < 0f)
					piece.GetComponent<Transform> ().SetParent (turnstile.transform);
			}
			if (turnstile.transform.position.y > 0f) {	//up
				if (piece.transform.position.y > 0f)
					piece.GetComponent<Transform> ().SetParent (turnstile.transform);
			}
			if (turnstile.transform.position.y < 0f) {	//down
				if (piece.transform.position.y < 0f)
					piece.GetComponent<Transform> ().SetParent (turnstile.transform);
			}
			if (turnstile.transform.position.z < 0f) {	//front
				if (piece.transform.position.z < 0f)
					piece.GetComponent<Transform> ().SetParent (turnstile.transform);
			}
			if (turnstile.transform.position.z > 0f) {	//back
				if (piece.transform.position.z > 0f)
					piece.GetComponent<Transform> ().SetParent (turnstile.transform);
			}
		}
		CubeIsRotating = true;
	}




	public GameObject GetCenterPiece(string sideToGet){
		foreach (GameObject centerPiece in centerPieces) {
			if (sideToGet == "top" && centerPiece.transform.position.y > 0.8f)
				return centerPiece;
			else if (sideToGet == "bottom" && centerPiece.transform.position.y < 0.8f)
				return centerPiece;
			else if (sideToGet == "left" && centerPiece.transform.position.x < -0.8f)
				return centerPiece;
			else if (sideToGet == "right" && centerPiece.transform.position.x > 0.8f)
				return centerPiece;
			else if (sideToGet == "back" && centerPiece.transform.position.z > 0.8f)
				return centerPiece;
			else if (sideToGet == "front" && centerPiece.transform.position.y < -0.8f)
				return centerPiece;

			//else if (sideToGet == "x")
			//			think about this

			else {
				return null;
				print ("ERROR--rubik.GetCenterPiece(), Error 1");
			}
		}
		return null;
		print ("ERROR--rubik.GetCenterPiece(), Error 2");
	}

}