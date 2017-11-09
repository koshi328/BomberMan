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
    // Use this for initialization
    void Start()
    {
        _mapController = GameObject.Find("MapController").GetComponent<MapController>();
        width = _mapController.GetWidth();
        height = _mapController.GetHeight();
        bombInfluenceMap = new float[width, height];
        setbombInfluenceMap = new float[width, height];
        field = new float[width, height];
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
                if (CheckWall(i, j)) field[i, j] -= 0.5f;
            }
        }
        CalcInfluence();
        CulcSetbombInfluence();
        CulcItemScore();

        if(Input.GetKeyDown(KeyCode.E))
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
                if (CanSafetyBombPut(i, j, i, j))
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
                int range = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                if (range != 0)
                {
                    range = 1 - (range / (width + height));
                    if (setbombInfluenceMap[i, j] < range) continue;
                    setbombInfluenceMap[i, j] = range;
                }
                else
                {
                    setbombInfluenceMap[i, j] = 1.0f;
                }
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
                bombInfluenceMap[x, y] = 1.0f - (j / 13.0f);
            }
        }
    }

    // dir = 0 なし
    // dir = 1 右
    // dir = 2 左
    // dir = 3 上
    // dir = 4 下
    bool CanSafetyBombPut(int bx,int by, int x, int y,int dir = 0)
    {
        if (_mapController.GetChipState(x, y) != MapController.STATE.NONE)
        {
            return false;
        }
        if (bx != x && by != y)
        {
            return true;
        }
        if (dir != 1 && CanSafetyBombPut(bx, by, x + 1, y, 2)) { field[x, y] -= 0.2f; return true; }
        if (dir != 2 && CanSafetyBombPut(bx, by, x - 1, y, 1)) { field[x, y] -= 0.2f; return true; }
        if (dir != 3 && CanSafetyBombPut(bx, by, x, y + 1, 4)) { field[x, y] -= 0.2f; return true; }
        if (dir != 4 && CanSafetyBombPut(bx, by, x, y - 1, 3)) { field[x, y] -= 0.2f; return true; }

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
            }
            Debug.Log(s);
        }
    }

    public float GetScore(int x,int y)
    {
        float score = 0.0f;
        score += setbombInfluenceMap[x, y] * 0.3f;
        score += bombInfluenceMap[x, y] * 0.5f;
        score += field[x, y] * 0.2f;
        return score;
    }
    public float GetCanSafetyBombPut(int x,int y)
    {
        return setbombInfluenceMap[x, y];
    }
    public float GetBombInfluence(int x, int y)
    {
        return bombInfluenceMap[x, y];
    }
    public bool CheckWall(int x,int y)
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
}
