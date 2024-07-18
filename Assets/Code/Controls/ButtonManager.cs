using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // rearrange the buttons
    private GameObject[] buttonArray;

    void Start() {
        // Get the number of children
        int buttonCount = transform.childCount;

        // Initialize the array to hold the children
        buttonArray = new GameObject[buttonCount];

        // Loop through the children and add them to the array
        for (int i = 0; i < buttonCount; i++){
            buttonArray[i] = transform.GetChild(i).gameObject;
        }

        int screenWidth = Screen.width;
        int spacing = screenWidth/(buttonCount+1);


        // arrange
        for (int i = 0;i < buttonCount;i++)
        {
            buttonArray[i].transform.position = new Vector3((spacing*i)+spacing,transform.position.y,0);
        }

    }
}
