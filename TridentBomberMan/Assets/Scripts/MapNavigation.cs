using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigation : MonoBehaviour
{

    MapController _mapController;
    float[,] bombInfluenceMap;
    float[,] setbombInfluenceMap;
    float[,] field;
    int width;
    int height;
    [SerializeField]
    GameObject debugPrefab;
    GameObject[,] debugs;
    // Use this for initialization
    void Start()
    {
        _mapController = GameObject.Find("MapController").GetComponent<MapController>();
        width = _mapController.GetWidth();
        height = _mapController.GetHeight();
        bombInfluenceMap = new float[width, height];
        setbombInfluenceMap = new float[width, height];
        field = new float[width, height];
        debugs = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                debugs[i, j] = Instantiate(debugPrefab);
                debugs[i, j].transform.parent = this.transform;
                debugs[i, j].transform.localPosition = new Vector3(i * 0.16f, j * 0.16f, 0);
            }
        }
    }

    private void Update()
    {
        Dump();
    }

    public void MyUpdate()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                field[i, j] = (_mapController.GetChipState(i, j) != MapController.STATE.NONE) ? 1.0f : 0.0f;
                bombInfluenceMap[i, j] = 0.0f;
                setbombInfluenceMap[i, j] = 0.0f;
                if (CheckWall(i, j)) field[i, j] -= 0.2f;
            }
        }
        CalcInfluence();
        CulcSetbombInfluence();
        CulcItemScore();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (IsCloseChip(i, j))
                {
                    bombInfluenceMap[i, j] = 5.0f;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
            Dump();
    }

    void CalcInfluence()
    {
        Bomb[] bombs = _mapController.GetBomb();
        int bombNum = bombs.Length;
        for (int i = 0; i < bombNum; i++)
        {
            Bomb bomb = bombs[i];
            if (!bomb.GetActive()) continue;
            BombInfluence(bomb);
        }
    }

    void CulcSetbombInfluence()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_mapController.GetChipState(i, j) != MapController.STATE.NONE)
                {
                    continue;
                }
                if (CheckWall(i, j) && CanSafetyBombPut(i, j, i, j))
                {
                    setbombInfluenceMap[i, j] = 1.0f;
                    BombRouteCulc(i, j);
                }
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                setbombInfluenceMap[i, j] = 1.0f - setbombInfluenceMap[i, j];
            }
        }
    }

    void BombRouteCulc(int x, int y)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_mapController.GetChipState(i, j) != MapController.STATE.NONE)
                {
                    continue;
                }
                if (x == i && y == j)
                {
                    setbombInfluenceMap[i, j] = 1.0f;
                    continue;
                }
                float range = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                range = 1 - (range / (width + height)) * 2;
                if (setbombInfluenceMap[i, j] > range) continue;
                setbombInfluenceMap[i, j] = range;
            }
        }
    }

    void BombInfluence(Bomb bomb)
    {
        Vector2[] dir =
        {
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0)
        };
        bombInfluenceMap[bomb.GetPosition().x, bomb.GetPosition().y] = 1.0f;
        for (int i = 0; i < dir.Length; i++)
        {
            for (int j = 1; j <= bomb._fireLevel; j++)
            {
                int x = bomb.GetPosition().x + ((int)dir[i].x * j);
                int y = bomb.GetPosition().y + ((int)dir[i].y * j);
                if (_mapController.GetChipState(x, y) != MapController.STATE.NONE)
                {
                    break;
                }
                bombInfluenceMap[x, y] += (1.0f - (j / 10.0f)) / (bomb._currentElapsedTime / 2 + 1.0f);
            }
        }
    }

    // dir = 0 なし
    // dir = 1 右
    // dir = 2 左
    // dir = 3 上
    // dir = 4 下
    bool CanSafetyBombPut(int bx, int by, int x, int y, int dir = 0)
    {
        if (_mapController.GetChipState(x, y) != MapController.STATE.NONE)
        {
            return false;
        }
        if (bombInfluenceMap[x, y] >= 0.3f)
        {
            return false;
        }
        if (bx != x && by != y)
        {
            return true;
        }
        if (dir != 1 && CanSafetyBombPut(bx, by, x + 1, y, 2)) { field[x, y] -= 0.5f; return true; }
        if (dir != 2 && CanSafetyBombPut(bx, by, x - 1, y, 1)) { field[x, y] -= 0.5f; return true; }
        if (dir != 3 && CanSafetyBombPut(bx, by, x, y + 1, 4)) { field[x, y] -= 0.5f; return true; }
        if (dir != 4 && CanSafetyBombPut(bx, by, x, y - 1, 3)) { field[x, y] -= 0.5f; return true; }

        return false;
    }

    void CulcItemScore()
    {
        int itemNum = _mapController.GetItems().Length;
        for (int i = 0; i < itemNum; i++)
        {
            Item item = _mapController.GetItems()[i];
            if (!item.gameObject.GetActive()) continue;
            int x = item.GetPosition().x;
            int y = item.GetPosition().y;
            field[x, y] -= 0.3f;
            //if (field[x, y] <= 0.0f) field[x, y] = 0.0f;
        }
    }

    void Dump()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            string s = "";
            for (int j = 0; j < width; j++)
            {
                s += GetScore(j, i) + ",";
                //s += setbombInfluenceMap[j,i] + ",";
                float c = setbombInfluenceMap[j, i];
                c = GetScore(j, i);
                //c = bombInfluenceMap[j, i]; ;
                if (c <= 0.0f)
                    debugs[j, i].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1.0f);
                else
                    debugs[j, i].GetComponent<SpriteRenderer>().color = new Color(c, c, c, 1.0f);
            }
            //Debug.Log(s);
        }
    }

    public float GetScore(int x, int y)
    {
        if (_mapController.GetChipState(x, y) != MapController.STATE.NONE) return 1.0f;
        float score = 0.0f;
        score += setbombInfluenceMap[x, y] * 0.3f;
        score += bombInfluenceMap[x, y] * 0.6f;
        score += field[x, y] * 0.1f;
        return score;
    }
    public float GetCanSafetyBombPut(int x, int y)
    {
        return setbombInfluenceMap[x, y];
    }
    public float GetBombInfluence(int x, int y)
    {
        return bombInfluenceMap[x, y];
    }
    public bool CheckWall(int x, int y)
    {
        Vector2[] dir =
        {
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0)
        };
        for (int i = 0; i < dir.Length; i++)
        {
            int resultX = x + (int)dir[i].x;
            int resultY = y + (int)dir[i].y;
            if (_mapController.GetChipState(resultX, resultY) == MapController.STATE.BREAKABLE_BLOCK)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsCloseChip(int x, int y)
    {
        Vector2[] dir =
{
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0)
        };
        for (int i = 0; i < dir.Length; i++)
        {
            int resultX = x + (int)dir[i].x;
            int resultY = y + (int)dir[i].y;
            if (_mapController.GetChipState(resultX, resultY) == MapController.STATE.NONE)
            {
                return false;
            }
        }
        return true;
    }

    public bool ExistBreakableBlock()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_mapController.GetChipState(i, j) == MapController.STATE.BREAKABLE_BLOCK)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
