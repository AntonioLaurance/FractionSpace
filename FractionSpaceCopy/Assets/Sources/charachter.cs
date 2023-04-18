using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charachter : MonoBehaviour
{
    GameObject targets;
    CombatCrtl combatCrtl;
    public GameObject barlife;
    public GameObject select;
    public SpriteRenderer sr;

    float ScaleI;
    int maxlife;
    public int life;
    public int atack;
    int target;

    public bool type;

    private void Start()
    {
        ScaleI = barlife.transform.localScale.x;
        maxlife = life;
        combatCrtl = GameObject.Find("CombatCrtl").GetComponent<CombatCrtl>();
        if (type)
        {
            targets = GameObject.Find("Enemies");
        }
        else targets = GameObject.Find("Players");
    }

    public void Atack()
    {
        StartCoroutine(AnimAtack());
        if (type) target = combatCrtl.EnemySelect;
        else target = combatCrtl.PlayerSelect;
        if (combatCrtl.EnemyN >= 0 && combatCrtl.PlayerN >= 0)
            targets.transform.GetChild(target).GetComponent<charachter>().Damage(atack);
    }

    public void Damage(int atack)
    {
        life -= atack;
        StartCoroutine(AnimDamage(atack));
        if (life <= 0)
        {
            if (type) combatCrtl.PlayerN--; else combatCrtl.EnemyN--;
            Destroy(gameObject);
        }
    }

    public void Select(bool select)
    {
        this.select.SetActive(select);
    }
    IEnumerator AnimAtack()
    {
        float mov = 0.3f;
        if (!type) mov *= -1;
        transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        yield return new WaitForSecondsRealtime(0.2f);
        transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
    }

    IEnumerator AnimDamage(float damage)
    {
        barlife.transform.localScale = new Vector3(barlife.transform.localScale.x - (damage / maxlife) * ScaleI, barlife.transform.localScale.y, barlife.transform.localScale.z);
        for (int i = 0; i < 10; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}
