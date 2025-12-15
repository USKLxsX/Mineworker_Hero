using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BreakingParticle : MonoBehaviour
{
    public GameObject ore;
    
    public ParticleSystem ps;

    void Start() {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startSize = 1f;
            emitParams.startLifetime = 1f;

            if (ore.GetComponent<SimpleOre>().oreType == SimpleOre.OreType.Ruby)
            {
                emitParams.startColor = Color.red;
            }
            else if (ore.GetComponent<SimpleOre>().oreType == SimpleOre.OreType.Blue)
            {
                emitParams.startColor = Color.blue;
            }
            else if (ore.GetComponent<SimpleOre>().oreType == SimpleOre.OreType.Purple)
            {
                emitParams.startColor = Color.magenta;
            }
            else if (ore.GetComponent<SimpleOre>().oreType == SimpleOre.OreType.Normal)
            {
                emitParams.startColor = Color.white;
            }
            else
            {
                emitParams.startColor = Color.black;
            }

            ps.Emit(emitParams, 50);
        }
    }
    
}
