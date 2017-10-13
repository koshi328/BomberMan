using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    enum STATE
    {
        IMMUTABLE,
        BLOCK,
        BOMB
    }

    readonly int WIDTH = 15;
    readonly int HEIGHT = 13;
    readonly float CHIP_SIZE = 1.28f;

    [SerializeField]
    GameObject _blockPref;

    STATE[,] _table;

	// Use this for initialization
	void Start () {
        CreateImmutableObjects();

    }

    void CreateImmutableObjects()
    {
        _table = new STATE[WIDTH, HEIGHT];
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                if (j > 0 && j < WIDTH - 1 && i > 0 && i < HEIGHT - 1)
                {
                    if (i % 2 == 1 || j % 2 == 1) continue;
                }
                float x = -WIDTH * CHIP_SIZE / 2 + j * CHIP_SIZE;
                float y = -HEIGHT * CHIP_SIZE / 2 + i * CHIP_SIZE;
                Instantiate(_blockPref, new Vector3(x, y, 0), Quaternion.identity, this.transform);
                _table[j, i] = STATE.IMMUTABLE;
            }
        }
    }
}
