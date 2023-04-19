using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCrtl : MonoBehaviour
{
    public int EnemyN = 2, PlayerN = 1;
    public int EnemySelect, PlayerSelect;
    public GameObject enemies, players;
    bool turn = true;

    private void Start()
    {
        charachter stats = players.transform.GetChild(PlayerSelect).GetComponent<charachter>();
        stats.Select(true);
    }

    private void Update()
    {
        if (EnemyN >= 0)
        {
            enemies.transform.GetChild(EnemySelect).GetComponent<charachter>().Select(false);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                EnemySelect--;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                EnemySelect++;
            EnemySelect = Mathf.Clamp(EnemySelect, 0, EnemyN);
            enemies.transform.GetChild(EnemySelect).GetComponent<charachter>().Select(true);
        }
    }

    public void Atack()
    {
        if (turn && PlayerN >= 0)
        {
            charachter ch = players.transform.GetChild (PlayerSelect).GetComponent<charachter>();
            ch.Atack();

            if (PlayerSelect == PlayerN)
            {
                PlayerSelect = 0;
                turn = false;
                StartCoroutine(AtackE());
            }
            else PlayerSelect++;

            ch.Select(false);
            ch = players.transform.GetChild(PlayerSelect).GetComponent<charachter>();
            ch.Select(true);
        }
    }

    IEnumerator AtackE()
    {
        if (EnemyN >= 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            for (int i = 0; i <= EnemyN; i++)
            {
                enemies.transform.GetChild(i).GetComponent<charachter>().Atack();   
                yield return new WaitForSecondsRealtime(1f);
            }
            turn = true;
        }
    }
}
