using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField]private string nextScene;
    private LevelTransitioner levelTransitioner;
    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
        activated = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player" && !activated)
        {
            Debug.Log("yeah you win bub");
            if(levelTransitioner == null)
                levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
            if(levelTransitioner != null)
            {
                activated = true;
                levelTransitioner.changeLevel(nextScene);
            }
            else
                Debug.LogError("Couldn't find level transitioner");
        }
    }
}
