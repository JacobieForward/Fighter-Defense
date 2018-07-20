using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject starPrefab;
    private Button selectMapButton;
    private Button mapOneButton;
    private Button mapTwoButton;
    private Button mapThreeButton;

    private GameObject firstCanvas;
    private GameObject selectMapCanvas;
	// Use this for initialization
	void Start () {
        selectMapButton = GameObject.Find("SelectMap").GetComponent<Button>();
        mapOneButton = GameObject.Find("MapOne").GetComponent<Button>();
        mapTwoButton = GameObject.Find("MapTwo").GetComponent<Button>();
        mapThreeButton = GameObject.Find("MapThree").GetComponent<Button>();

        firstCanvas = GameObject.Find("FirstCanvas");
        selectMapCanvas = GameObject.Find("SelectMapCanvas");

        selectMapCanvas.SetActive(false);

        selectMapButton.onClick.AddListener(FirstCanvasTask);
        mapOneButton.onClick.AddListener(delegate { SelectMapTask("MapOne"); });
        mapTwoButton.onClick.AddListener(delegate { SelectMapTask("MapTwo"); });
        mapThreeButton.onClick.AddListener(delegate { SelectMapTask("MapThree"); });
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
        GameObject newstar = Instantiate(starPrefab, position, Quaternion.identity);
    }

    void FirstCanvasTask()
    {
        firstCanvas.SetActive(false);
        selectMapCanvas.SetActive(true);
    }

    void SelectMapTask(string mapName)
    {
        Debug.Log(mapName);
        SceneManager.LoadScene("ArenaOne", LoadSceneMode.Single);
    }
}
