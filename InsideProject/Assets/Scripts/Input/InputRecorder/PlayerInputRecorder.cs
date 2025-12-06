using System.Collections.Generic;
using UnityEngine;

public class PlayerInputRecorder : MonoBehaviour
{
    public MonoBehaviour inputSourceBehaviour;

    private IPlayerInput _input;

    [HideInInspector]
    public List<InputFrame> recordedFrames = new List<InputFrame>();

    private bool _isRecording;
    private float _recordStartTime;

    private void Awake()
    {
        _input = inputSourceBehaviour as IPlayerInput;
        if (_input == null)
        {
            Debug.LogError("inputSourceBehaviour must implement IPlayerInput!");
        }
    }

    private void Update()
    {
        if (!_isRecording || _input == null) return;

        float t = Time.time - _recordStartTime;

        recordedFrames.Add(new InputFrame
        {
            time = t,
            move = _input.Move,
            look = _input.Look,
            jumpPressed = _input.JumpPressed,
            runHeld = _input.RunHeld,
            dashPressed = _input.DashPressed,
        });
    }

    public void StartRecording()
    {
        recordedFrames.Clear();
        _recordStartTime = Time.time;
        _isRecording = true;
        Debug.Log("Recording started.");
    }

    public void StopRecording()
    {
        _isRecording = false;
        Debug.Log("Recording stopped. Frames: " + recordedFrames.Count);
    }
}
