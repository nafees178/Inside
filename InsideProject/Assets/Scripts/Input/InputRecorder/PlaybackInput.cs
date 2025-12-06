using System.Collections.Generic;
using UnityEngine;

public class PlaybackInput : MonoBehaviour, IPlayerInput
{
    [HideInInspector]
    public List<InputFrame> frames;

    private int _index;
    private float _playbackStartTime;

    private Vector2 _move;
    private Vector2 _look;
    private bool _jump;
    private bool _run;
    private bool _dash;

    public Vector2 Move => _move;
    public Vector2 Look => _look;
    public bool JumpPressed => _jump;
    public bool RunHeld => _run;

    public bool DashPressed => _dash;

    public bool recordKeyPressed => false;

    public bool Loop = false;

    public void BeginPlayback(List<InputFrame> sourceFrames)
    {
        frames = new List<InputFrame>(sourceFrames);
        _index = 0;
        _playbackStartTime = Time.time;
    }

    private void Update()
    {
        if (frames == null || frames.Count == 0)
        {
            _move = Vector2.zero;
            _look = Vector2.zero;
            _jump = false;
            _run = false;
            _dash = false;
            return;
        }

        float t = Time.time - _playbackStartTime;

        while (_index + 1 < frames.Count &&
               frames[_index + 1].time <= t)
        {
            _index++;
        }

        if (_index >= frames.Count)
        {
            if (Loop)
            {
                _index = 0;
                _playbackStartTime = Time.time;
            }
            else
            {
                _move = Vector2.zero;
                _look = Vector2.zero;
                _jump = false;
                _run = false;
                _dash = false;
                return;
            }
        }

        var f = frames[_index];

        _move = f.move;
        _look = f.look;
        _jump = f.jumpPressed;
        _run = f.runHeld;
        _dash = f.dashPressed;
    }
}
