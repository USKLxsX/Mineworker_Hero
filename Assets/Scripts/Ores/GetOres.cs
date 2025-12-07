using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOres : MonoBehaviour
{
    [SerializeField] Transform parenttransform;
    [SerializeField] GameObject prefab;   
    [SerializeField] Vector2 cellSize = new Vector2(1, 1); 
    [SerializeField] Vector2 start;
    [SerializeField] int row = 25;
    [SerializeField] int col = 12;
    [SerializeField] int rednum = 20;
    [SerializeField] int bluenum = 20;
    [SerializeField] int purplenum = 5;
    [SerializeField] int hardstonenum = 35;
    [SerializeField] int lavastonenum = 10;

    void Start()
    {
        int[,] matrix = RandomMatrix.GetARandomMatrix(row, col, rednum, bluenum, purplenum, hardstonenum, lavastonenum);
        for (int r = 0; r < row; r++)
            for (int c = 0; c < col; c++)
            {
                Vector2 localOffset = start + new Vector2(c * cellSize.x, -r * cellSize.y);
                Vector2 worldPos = (Vector2)parenttransform.TransformPoint(localOffset);
                GameObject one=Instantiate(prefab, worldPos, Quaternion.identity, parenttransform);
                one.GetComponent<SimpleOre>().oreType= (SimpleOre.OreType)matrix[r, c];
                one.GetComponent<SimpleOre>().oreUIImage = one.GetComponent<SpriteRenderer>();
                one.GetComponent<SimpleOre>().AutoSetupOreImage();
            }

    }
}
