using UnityEngine.SceneManagement;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public string checkPointName;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this);
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))
        {
            Debug.Log("has key");
            if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_cp") == checkPointName)
            {
                PlayerController.instance.transform.position = transform.position;
                Debug.Log("player postion changed" + PlayerController.instance.transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", "");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player collide with check point");
        if (other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", checkPointName);
        }
    }
}