using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    public AudioSource[] beShotSound;
    TextMeshProUGUI hpText;
    Image damageScreen;
    public int hp = 100;
    public float hpRecoveryDelay = 3;
    public int hpRecoveryVolume = 10;
    public int damage;
    float currentTime = 0;

    void Start()
    {
        hpText = GameObject.Find("HP").GetComponent<TextMeshProUGUI>();
        damageScreen = GameObject.Find("DamageScreen").GetComponent<Image>();
    }

    void Update()
    {
        hpText.text = "" + hp;
        if (hp <= 0)
        {
            hp = 0;
            GameObject enemyFac = GameObject.Find("EnemyFactory");
            GameObject[] pistolEnemy = GameObject.FindGameObjectsWithTag("Pistol");
            GameObject[] rifleEnemy = GameObject.FindGameObjectsWithTag("Rifle");
            DestroyObject(enemyFac); DestroyObject(pistolEnemy); DestroyObject(rifleEnemy);
            EnemyFactory.count = 0;
        }
            
        else if (hp >= 100)
            hp = 100;

        if (hp <= 30)
            hpText.color = Color.red;
        else
            hpText.color = Color.green;

        if (currentTime > hpRecoveryDelay)
        {
            if (hp < 100 && hp > 0)
            {
                hp += hpRecoveryVolume;
                currentTime = 0;
            }
        }
        currentTime += Time.deltaTime;
    }

    IEnumerator DamageScreen()
    {
        damageScreen.color = new Color(1, 0, 0, Random.Range(0.5f, 0.6f));
        yield return new WaitForSeconds(0.5f);
        damageScreen.color = Color.clear;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            beShotSound[(int)Random.Range(0, 3.9f)].Play();
            StartCoroutine(DamageScreen());
            hp -= (int)Random.Range(1, 2) * damage;
        }
    }

    void DestroyObject(GameObject target)
    {
        Destroy(target);
    }

    void DestroyObject(GameObject[] target)
    {
        for(int i = 0; i< target.Length; i++)
        {
            Destroy(target[i]);
        }
    }
}
