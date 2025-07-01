
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrillState : IState
{
    private Player _player;
    float counter = 0;
    public DrillState(Player player)
    {
        _player = player;
    }
    public void Enter()
    {
        _player.isDrilling = true;

    }

    public void Exit()
    {
        _player.isDrilling = false;
        _player._drillSide.SetBool("Start", false);
        _player._drillDown.SetBool("Start", false);
    }

    public void Update()
    {
        if (_player.currentDirection == 0)
        {
            _player._drillDown.SetBool("Start", true);
            _player._drillSide.SetBool("Start", false);
        }
        else
        {
            _player._drillDown.SetBool("Start", false);
            _player._drillSide.SetBool("Start", true);
        }

        counter += Time.deltaTime;
        if (counter >= _player.drillCoolTime)
        {
            Vector3Int currentPos = _player.brokenableTilemap.WorldToCell(_player.transform.position);
            Vector3Int pos = currentPos;
            switch (_player.currentDirection)
            {
                case -1:
                    pos = new Vector3Int(currentPos.x - 1, currentPos.y);
                    break;
                case 1:
                    pos = new Vector3Int(currentPos.x + 1, currentPos.y);

                    break;
                case 0:
                    pos = new Vector3Int(currentPos.x, currentPos.y - 1);
                    break;
            }

            (int x, int y) = _player.tileManager.TryCellToIndex(pos);
            if (!_player.brokenableTilemap.HasTile(pos))
            {
                Debug.Log("타일없음");
            }
            else
            {
                //isDrilling = true;

                _player.tiles[x, y] -= _player.drillDamage;
                if (_player.brokenableTilemap.GetTile(pos) != null)
                {
                    if (_player.tiles[x, y] <= 0)
                    {
                        _player.brokenableTilemap.SetTile(pos, null);

                    }
                    else
                    {
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();
                        int index = Mathf.Clamp((int)(_player.tiles[x, y] / _player.tileManager.spriteIndex), 0, _player.brokenTileSprites.Length - 1);
                        newTile.sprite = _player.brokenTileSprites[index];
                        _player.brokenableTilemap.SetTile(pos, newTile);
                    }
                }

            }
            counter = 0;
        }

        


    }
}
