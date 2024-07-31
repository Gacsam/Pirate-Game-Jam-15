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
            // DontDestroyOnLoad(this.gameObject);
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

        mainMenuObject = Camera.main.transform.Find("UI_Canvas/MainMenu").gameObject;

        GameSpeed = GameSpeed.Normal;
        mainMenuObject.SetActive(false);

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

    // cache minion production
    [HideInInspector]
    bool[] productionArray = new bool[5];
    [HideInInspector]
    int productionIndex = 0;

    void InteractedWithButton(Button interactedButton)
    {
        bool buttonSuccess = false;
        switch (interactedButton.name)
        {
            case "Melee Spawn Button":
                
                if(productionIndex >= productionArray.Length){
                    buttonSuccess = false;
                    break;
                }
                productionArray[productionIndex++] = true;
                productionSlider.value = productionIndex;
                if(!onCooldown){StartCoroutine(ProductionQueue());}
                break;
            case "StartButton":
                GameSpeed = GameSpeed.Normal;
                mainMenuObject.SetActive(false);
                break;
            case "0x":
                GameSpeed = GameSpeed.Stop;
                break;
            case "1x":
                GameSpeed = GameSpeed.Normal;
                break;
            case "2x":
                GameSpeed = GameSpeed.Fast;
                break;
            case "3x":
                GameSpeed = GameSpeed.Superfast;
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
            // GameMan.ModifyGold(UnitSide.Alchemy, -30);
            return true;
        }
        else return false;
    }

    private static GameObject mainMenuObject;
    
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

    private Slider productionSlider;

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

        productionSlider = GameObject.Find("Production Queue").GetComponent<Slider>();
        productionSlider.maxValue = productionArray.Length;
        productionSlider.value = 0;
    }

    bool onCooldown = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameSpeed == GameSpeed.Normal)
            {
                GameSpeed = GameSpeed.Stop;
                mainMenuObject.SetActive(true);
            }
            else if (GameSpeed == GameSpeed.Stop)
            {
                GameSpeed = GameSpeed.Normal;
                mainMenuObject.SetActive(false);
            }
        }

    }

    static GameSpeed currentWorldSpeed = GameSpeed.Normal;

    public static GameSpeed GameSpeed
    {
        get { return currentWorldSpeed; }
        set
        {
            switch (value)
            {
                case GameSpeed.Stop:
                    Time.timeScale = 0;

                    break;
                case GameSpeed.Normal:
                    Time.timeScale = 1;
                    break;
                case GameSpeed.Fast:
                    Time.timeScale = 2;
                    break;
                case GameSpeed.Superfast:
                    Time.timeScale = 3;
                    break;
                default:
                    break;
            }
            currentWorldSpeed = value;
        }
    }

    IEnumerator ProductionQueue(){
        onCooldown = true;

        yield return new WaitForSeconds(GameObject.Find("Player Tower").GetComponent<Tower_Spawner>().CD);

        // try spawn minion untill success
        while(!SpawnButton()){yield return new WaitForSeconds(0.5f);}

        productionIndex--;
        productionArray[productionIndex] = false;
        productionSlider.value = productionIndex;
        onCooldown = false;

        // start next production
        if(!onCooldown && productionIndex != 0 && productionArray[productionIndex-1] == true){StartCoroutine(ProductionQueue());}
    }


}
