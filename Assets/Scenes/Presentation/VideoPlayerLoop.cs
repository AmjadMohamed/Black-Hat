using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerLoop : MonoBehaviour
{
    public VideoClip videoClip;  // Reference to the video clip to be played
    public bool isLooping = true;  // Whether the video should loop
    public bool mute = false;  // Whether the audio should be muted

    private VideoPlayer videoPlayer;  // Reference to the VideoPlayer component

    void Start()
    {
        // Get the VideoPlayer component attached to the same GameObject
        videoPlayer = GetComponent<VideoPlayer>();

        // Assign the video clip to the VideoPlayer
        videoPlayer.clip = videoClip;

        // Set the looping flag
        videoPlayer.isLooping = isLooping;

        // Set the mute flag
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, GetComponent<AudioSource>());
        GetComponent<AudioSource>().mute = mute;

        // Start playing the video
        videoPlayer.Play();
    }
}
