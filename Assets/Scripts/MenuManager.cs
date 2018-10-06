using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject starPrefab;
    private Button mapOneButton;
    private Button mapTwoButton;
    private Button mapThreeButton;
    private Button mapOneDescriptionButton;
    private Button mapTwoDescriptionButton;
    private Button mapThreeDescriptionButton;
    private Button exitDescriptionButton;
    private GameObject descriptionCanvas;
    private Text description;

	// Use this for initialization
	void Start () {
        mapOneButton = GameObject.Find("MapOne").GetComponent<Button>();
        mapTwoButton = GameObject.Find("MapTwo").GetComponent<Button>();
        mapThreeButton = GameObject.Find("MapThree").GetComponent<Button>();
        mapOneDescriptionButton = GameObject.Find("MapOneDescription").GetComponent<Button>();
        mapTwoDescriptionButton = GameObject.Find("MapTwoDescription").GetComponent<Button>();
        mapThreeDescriptionButton = GameObject.Find("MapThreeDescription").GetComponent<Button>();
        exitDescriptionButton = GameObject.Find("ExitDescriptionButton").GetComponent<Button>();
        description = GameObject.Find("DescriptionText").GetComponent<Text>();
        descriptionCanvas = GameObject.Find("DescriptionCanvas");
        descriptionCanvas.SetActive(false);

        mapOneButton.onClick.AddListener(delegate { SelectMapTask("MapOne"); });
        mapTwoButton.onClick.AddListener(delegate { SelectMapTask("MapTwo"); });
        mapThreeButton.onClick.AddListener(delegate { SelectMapTask("MapThree"); });
        mapOneDescriptionButton.onClick.AddListener(delegate { DisplayDescriptionTask("Defend your Space Station from Enemy Attackers from all directions."); });
        mapTwoDescriptionButton.onClick.AddListener(delegate { DisplayDescriptionTask("Standard but with lots and lots of suicidal Mines that explode on contact!"); });
        mapThreeDescriptionButton.onClick.AddListener(delegate { DisplayDescriptionTask("There's no station to defend, just stay alive!"); });
        exitDescriptionButton.onClick.AddListener(delegate { ExitDescription(); });
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 temp = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.4f, Camera.main.transform.position.z);
        Camera.main.transform.position = temp;


        Vector3 starPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 1f), 1, 1));
        Instantiate(starPrefab, starPosition, Quaternion.identity);
    }

    void CreateStar(Vector3 position)
    {
        Instantiate(starPrefab, position, Quaternion.identity);
    }

    void SelectMapTask(string mapName)
    {
        Debug.Log(mapName);
        SceneManager.LoadScene(mapName, LoadSceneMode.Single);
    }

    void DisplayDescriptionTask(string text)
    {
        description.text = text;
        descriptionCanvas.SetActive(true);
    }

    void ExitDescription()
    {
        descriptionCanvas.SetActive(false);
    }
}