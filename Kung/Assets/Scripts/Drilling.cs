using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum CurrentDirectionState
{
    None,
    Left,
    Right,
    Down
}
public class Drilling : MonoBehaviour
{

    [SerializeField] private GameObject drill;
    [SerializeField] private Tilemap drillAbleTilemap;
    public float drillDamage;
    public float drillSpeed;
    bool isGround = true;
    float cooltime = 0;
    // ÇöÀç ±¼ÂøÇÒ ¹æÇâ
    public CurrentDirectionState currentDirectionState = CurrentDirectionState.Down;
    //private Dictionary<Vector3Int,float> tileDict = new Dictionary<Vector3Int,float>();
    private float[,] tiles;
    [SerializeField] private Animator drillAnimator;
    int width;
    int height;
    int offsetX;
    int offsetY;

    private (bool valid, int x, int y) TryCellToIndex(Vector3Int cellPos)
    {
        int x = cellPos.x + offsetX;
        int y = cellPos.y + offsetY;
        if (x < 0 || y < 0 || x >= width || y >= height)
            return (false, 0, 0);
        return (true, x, y);
    }
    private void Start()
    {
        BoundsInt bounds = drillAbleTilemap.cellBounds;
        width = bounds.xMax - bounds.xMin;
        height = bounds.yMax - bounds.yMin;
        tiles = new float[width, height];
        offsetX = -bounds.xMin;
        offsetY = -bounds.yMin;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                //tileDict[pos] = 100;
                tiles[TryCellToIndex(pos).x, TryCellToIndex(pos).y] = 100; 
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
                Vector3Int currentPos = drillAbleTilemap.WorldToCell(transform.position);
                Vector3Int pos = currentPos;


                switch (currentDirectionState)
                {
                    case CurrentDirectionState.None:
                        break;
                    case CurrentDirectionState.Left:
                        pos = new Vector3Int(currentPos.x - 1, currentPos.y);
                        break;
                    case CurrentDirectionState.Right:
                        pos = new Vector3Int(currentPos.x + 1, currentPos.y);
                        break;
                    case CurrentDirectionState.Down:
                        pos = new Vector3Int(currentPos.x, currentPos.y - 1);
                        break;
                    default:
                        break;
                }
                //tileDict[pos] -= drillDamage;
                var (valid, x, y) = TryCellToIndex(pos);
                if (valid)
                {
                    tiles[x, y] -= drillDamage;
                    if (tiles[x, y] <= 0)
                    {
                        drillAbleTilemap.SetTile(pos, null);
                    }
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
