using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public List<GameObject> particles;

    private static ParticlesController _instance;
    public static ParticlesController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ParticlesController>();
                if (_instance == null)
                {
                    Debug.Log("No ParticlesController!");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayParticle(SimpleOre ore,float time)
    {
        StartCoroutine(PlayParticles(ore, time));
    }

    IEnumerator PlayParticles(SimpleOre ore, float time)
    {
        GameObject tmp = Instantiate(particles[(int)ore.oreType+1],ore.transform.position,new Quaternion(0,0,0,0));
        tmp.GetComponent<ParticleSystem>().Play();
        
        yield return new WaitForSeconds(time);
        Destroy(tmp);
        yield break;
    }

}
