using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public PlayerHP playerHP;
    public Transform playerCamera;
    public Transform setUIPos;
    public GameObject rightRay;
    public GameObject leftRay;

    private float timer = 0.0f;
    private TextMeshProUGUI text;

    private void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        text = transform.Find("Panel").Find("PlayTimeText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHP.hp <= 0)
        {
            if(gameObject.GetComponent<Canvas>().enabled == false)
            gameObject.transform.position = setUIPos.position;
            gameObject.transform.LookAt(playerCamera);
            gameObject.GetComponent<Canvas>().enabled = true;
            rightRay.SetActive(true);
            leftRay.SetActive(true);
            text.text = string.Format("PlayTime : {0:N1}s", timer);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void LoadBoatScene()
    {
        SceneManager.LoadScene(gameObject.scene.name);
    }
}
