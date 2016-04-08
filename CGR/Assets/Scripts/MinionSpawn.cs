using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour
{
    public Animator anim;

    private int _bonusMinion = 1;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_bonusMinion == 0 && LevelManager.Instance.GetMinionCount() > 0)
            anim.SetBool("Stocked", false);
        //else
        //    anim.Set
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player" && (_bonusMinion > 0 || LevelManager.Instance.GetMinionCount() == 0))
        {
            if(_bonusMinion > 0)           
                _bonusMinion--;

            anim.SetBool("Stocked", false);
            anim.SetBool("Spawning", true);
        }            
    }

    void newMinion()
    {
        GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
        newMinion.transform.position = transform.position;
        LevelManager.Instance.AddMinion(newMinion);
        anim.SetBool("Spawning", false);
    }
}
