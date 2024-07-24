using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static float globalMeleeRange = 0.5f;
    // GameMananager singleton to track all the variables that we need accessible
    private static UI_Manager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        foreach (var button in Camera.main.GetComponentsInChildren<Button>())
        {
            // LAMBDA FUNCTIONS OP
            button.onClick.AddListener(() => InteractedWithButton(button));
        }
    }
    // Having a public static "Instance" allows us to call GameMan.X rather than GameMan.instance.X
    public static UI_Manager Instance
    {
        get
        {
            // Find self and set as instance, if the "spot" is already taken throw a log
            if (instance == null)
            {
                instance = FindObjectOfType<UI_Manager>();
            }
            else
            {
                // idk go boom
            }
            return instance;
        }
    }

    public static Transform GetMainCanvasTransform()
    {
        return Camera.main.GetComponentInChildren<Canvas>().transform;
    }

    void InteractedWithButton(Button interactedButton)
    {
        bool buttonSuccess = false;
        switch (interactedButton.name)
        {
            case "Melee Spawn Button":
                buttonSuccess = SpawnButton();
                break;
            default:
                Debug.Log("No interaction found with \"" + name + "\"");
                break;
        }
        if (!buttonSuccess)
            StartCoroutine(ButtonFlashColour(interactedButton, Color.red));
    }

    bool SpawnButton()
    {
        if (GameMan.SpawnUnit(UnitSide.Alchemy))
        {
            // Do stuff like take coins away
            GameMan.ModifyGold(UnitSide.Alchemy, -30);
            return true;
        }
        else return false;
    }
    
    IEnumerator ButtonFlashColour(Button buttonToFlash, Color colourToSet)
    {
        // Check if it's not red already
        if (buttonToFlash.image.color != colourToSet)
        {
            var defaultColour = buttonToFlash.image.color;
            buttonToFlash.image.color = colourToSet;
            yield return new WaitForSeconds(0.1f);
            buttonToFlash.image.color = defaultColour;
        }
        yield return null;
    }

    // rearrange the buttons
    // NOT USED CUZ WE ONLY HAVE 1 MINION TYPE WE CAN SPAWN WITH BUTTON
    private GameObject[] buttonArray;

    void Start() {
        // Get the number of children
        int buttonCount = GetMainCanvasTransform().Find("Shards").childCount;

        // Initialize the array to hold the children
        buttonArray = new GameObject[buttonCount];

        // Loop through the children and add them to the array
        for (int i = 0; i < buttonCount; i++){
            buttonArray[i] = GetMainCanvasTransform().Find("Shards").GetChild(i).gameObject;
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
