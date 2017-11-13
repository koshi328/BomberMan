using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : ControllerBase
{
	public override void Initialize ()
    {
	}

    /// <summary>
    /// 操作に応じて移動処理を呼び出す関数
    /// </summary>
    public override void ControlMove()
    {
        // 操作対象が移動中なら処理しない
        if (_player._state == Player.STATE.MOVE) return;

        base.ControlMove();

        Vector2 direction = Vector2.zero;

        // 取得するキー情報のタグ
        var leftStickHorizontal = "L_XAxis_" + (_player._playerNumber + 1).ToString();
        var leftStickVertical = "L_YAxis_" + (_player._playerNumber + 1).ToString();
        var dpadHorizontal = "DPad_XAxis_" + (_player._playerNumber + 1).ToString();
        var dpadVertical= "DPad_YAxis_" + (_player._playerNumber + 1).ToString();

        // 入力を取得
        float xInput = Input.GetAxisRaw(leftStickHorizontal);
        float yInput = Input.GetAxisRaw(leftStickVertical);
        if (Mathf.Abs(xInput) < float.Epsilon &&
            Mathf.Abs(yInput) < float.Epsilon)
        {
            xInput = Input.GetAxisRaw(dpadHorizontal);
            yInput = Input.GetAxisRaw(dpadVertical);
        }

        // 縦方向の移動の入力がされていたら
        if (float.Epsilon < Mathf.Abs(yInput))
        {
            if (yInput < 0)
            {
                direction = Vector2.up;
            }
            else
            {
                direction = Vector2.down;
            }
        }
        else if (float.Epsilon < Mathf.Abs(xInput))
        {
            if (xInput < 0)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
        }
        else
        {
            return;
        }

        // 移動処理
        if (!_player.GetStatus(Player.ISMISOBON))
        {
            _player.Move(direction);
        }
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void ControlSetBomb()
    {
        base.ControlSetBomb();

        GamepadInput.GamePad.Index index;

        switch(_player._playerNumber)
        {
            case 0:
                index = GamepadInput.GamePad.Index.One;
                break;
            case 1:
                index = GamepadInput.GamePad.Index.Two;
                break;
            case 2:
                index = GamepadInput.GamePad.Index.Three;
                break;
            case 3:
                index = GamepadInput.GamePad.Index.Four;
                break;
            default:
                index = GamepadInput.GamePad.Index.One;
                break;
        }

        if (GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.X, index))
        {
            if (!_player.GetStatus(Player.ISMISOBON))
            {
                _player.SetBomb();
            }
        }
    }
}
