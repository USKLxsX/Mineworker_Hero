using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour
{

    private static PlayerBattleController instance;
    public static PlayerBattleController Instance
    {
        get
        {
            if(instance == null)
            {
                instance=FindObjectOfType<PlayerBattleController>();
                if (instance == null)
                {
                    Debug.Log("No PlayerBattleController!");
                }
            }
            return instance;
        }
    }

    public int isjump;

    // Start is called before the first frame update
    void Start()
    {
        isjump = 0;
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isjump==0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isjump++;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 300f));
                Invoke("JumpBack", 1.5f);
            }
        }
        if(isjump==2) 
        {
            if (Mathf.Pow(GetComponent<Rigidbody2D>().velocity.y, 2) <= 0.0001f && transform.position.y <= -41.47f)
            {
                isjump = 0;
            }
        }
    }

    void JumpBack()
    {
        CancelInvoke("JumpBack");
        isjump = 2;
    }
}
