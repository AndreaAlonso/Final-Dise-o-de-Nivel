using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Dictionary<string, Collider> colliders;
    public List<object> heroInformation;

	// Use this for initialization
	void Start ()
    {
        heroInformation = new List<object>();
        Collider[] myColliders = GetComponents<Collider>();
        colliders = new Dictionary<string, Collider>();
        colliders["Spawn1"] = myColliders[0];
        colliders["Spawn2"] = myColliders[1];
        colliders["Final"] = myColliders[2];
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetRespawn(string r, Hero hero)
    {
        heroInformation.Clear();
        if(r == "Spawn1")
        {
            colliders["Spawn1"].enabled = false;
            heroInformation.Add(hero.transform.position);
            heroInformation.Add(hero.life);
            FindObjectOfType<Boos1>().canAttack = true;
            hero.checkPoint = true;
        }
        else
        {
            colliders["Spawn2"].enabled = false;
            heroInformation.Add(hero.transform.position);
            heroInformation.Add(hero.life);
            FindObjectOfType<FinalBoss>().canAttack = true;
        }
    }

    public void GetHeroInformation(Hero hero)
    {
        hero.transform.position = (Vector3)heroInformation[0];
        hero.life = (float)heroInformation[1];
    }

    public void Final()
    {
        colliders["Final"].enabled = false;
    }

}
