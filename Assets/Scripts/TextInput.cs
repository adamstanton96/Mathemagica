using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////////////////////////
//Monobehaviour used to poll user numerical input every frame//
///////////////////////////////////////////////////////////////

public class TextInput : MonoBehaviour
{
    string inputString; //Currently stored user input.

	// Use this for initialization
	void Start ()
    {
        inputString = "";
	}
	
	// Update is called once per frame
	void Update ()
    {
        pollInputs(); //Poll inputs each frame.
	}

    //Polls Key Inputs For Numerical Entries//
    void pollInputs()
    {
        if(Input.GetKeyUp("0") || Input.GetKeyUp(KeyCode.Keypad0))
        {
            Debug.Log("0");
            inputString += "0";
        }
        else if (Input.GetKeyUp("1") || Input.GetKeyUp(KeyCode.Keypad1))
        {
            inputString += "1";
        }
        else if (Input.GetKeyUp("2") || Input.GetKeyUp(KeyCode.Keypad2))
        {
            inputString += "2";
        }
        else if (Input.GetKeyUp("3") || Input.GetKeyUp(KeyCode.Keypad3))
        {
            inputString += "3";
        }
        else if (Input.GetKeyUp("4") || Input.GetKeyUp(KeyCode.Keypad4))
        {
            inputString += "4";
        }
        else if (Input.GetKeyUp("5") || Input.GetKeyUp(KeyCode.Keypad5))
        {
            inputString += "5";
        }
        else if (Input.GetKeyUp("6") || Input.GetKeyUp(KeyCode.Keypad6))
        {
            inputString += "6";
        }
        else if (Input.GetKeyUp("7") || Input.GetKeyUp(KeyCode.Keypad7))
        {
            inputString += "7";
        }
        else if (Input.GetKeyUp("8") || Input.GetKeyUp(KeyCode.Keypad8))
        {
            inputString += "8";
        }
        else if (Input.GetKeyUp("9") || Input.GetKeyUp(KeyCode.Keypad9))
        {
            inputString += "9";
        }
    }

    //Returns the currently input string - for use in setting the player input values//
    public string getInputString()
    {
        return inputString;
    }

    //Clears the string, resetting inputs//
    public void clear()
    {
        inputString = "";
    }
}
