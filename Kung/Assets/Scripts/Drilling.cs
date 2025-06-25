using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drilling : MonoBehaviour
{

    [SerializeField] private GameObject drill;
    [SerializeField] private Tilemap drillAbleTilemap;
    public float drillDamage;
    public float drillSpeed;
    bool isGround = true;
    float cooltime = 0;

    private Dictionary<Vector3Int,float> tileDict = new Dictionary<Vector3Int,float>();
    [SerializeField] private Animator drillAnimator;
    private void Start()
    {
        BoundsInt bounds = drillAbleTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                tileDict[pos] = 100;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log(drillAbleTilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0)));
        }

        if (Input.GetKey(KeyCode.X) && isGround)
        {
            cooltime += Time.deltaTime;
            //±¼Âø ½ÃÀÛ
            drillAnimator.SetBool("Drilling", true);
            //drill.SetActive(true);
            if (cooltime >= drillSpeed)
            {
                Vector3Int currentPos = drillAbleTilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0));
                Vector3Int pos = new Vector3Int(currentPos.x, currentPos.y - 1);
                tileDict[pos] -= drillDamage;
                if (tileDict[pos] <= 0)
                {
                    drillAbleTilemap.SetTile(pos, null);
                }
                cooltime = 0;
            }

        }
        else
        {
            //drill.SetActive(false);
            drillAnimator.SetBool("Drilling", false);

        }
    }
}
