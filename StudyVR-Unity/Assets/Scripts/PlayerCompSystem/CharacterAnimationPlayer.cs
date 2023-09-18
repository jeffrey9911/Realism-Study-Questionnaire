using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationPlayer : MonoBehaviour
{
    public Vector3 PositionOffset = Vector3.zero;
    public Vector3 RotationOffset = Vector3.zero;
    public Vector3 ScaleOffset = Vector3.one;

    public bool isOverrideFPS = false;
    [SerializeField][HideInInspector] public float PlayerFramePerSecond = 60;

    private Animator animator;
    private AnimationClip animationClip;

    public bool isPlaying = false;

    public bool isPlayingAudio = false;
    [SerializeField][HideInInspector] public AudioClip PlayerAudio;
    private AudioSource PlayerAudioSource;

    private int CurrentFrame = 0;
    private int FrameCount = 0;
    private float FrameTimer = 0;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Please setup animation first!");
            isPlaying = false;
            return;
        }

        if (isPlayingAudio)
        {
            if (PlayerAudio != null)
            {
                PlayerAudioSource = this.gameObject.AddComponent<AudioSource>();
                PlayerAudioSource.clip = PlayerAudio;
                PlayerAudioSource.loop = true;
                PlayerAudioSource.spatialBlend = 1f;
                PlayerAudioSource.rolloffMode = AudioRolloffMode.Linear;
                PlayerAudioSource.maxDistance = 10f;
                PlayerFramePerSecond = FrameCount / PlayerAudio.length;
            }
            else
            {
                Debug.LogError("Please setup audio first!");
                isPlayingAudio = false;
            }
        }

        if(!isPlaying)
        {
            animator.speed = 0f;
        }
        
    }

    private void Start()
    {
        if(animator != null)
        {
            this.transform.position += PositionOffset;
            this.transform.eulerAngles += RotationOffset;
            this.transform.localScale = ScaleOffset;

            animationClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            FrameCount = Mathf.CeilToInt(animationClip.length * animationClip.frameRate);
            
            if(isPlayingAudio)
            {
                animator.speed = 0.01f;
                PlayerAudioSource.Play();
            }
        }
    }

    private void Update()
    {
        if(isPlaying)
        {
            if(isPlayingAudio)
            {
                if (Mathf.FloorToInt(PlayerAudioSource.time / PlayerAudio.length * (FrameCount - 1)) != CurrentFrame)
                {
                    SwapFrame();
                }
            }
            else if(isOverrideFPS)
            {
                FrameTimer += Time.deltaTime;

                if (FrameTimer >= 1f / PlayerFramePerSecond)
                {
                    SwapFrame();
                    FrameTimer = 0;
                }
            }
        }
    }


    private void SwapFrame(bool isReversing = false)
    {
        int NextFrame = 0;
        if (isReversing)
        {
            NextFrame = (CurrentFrame - 1) < 0 ? FrameCount - 1 : CurrentFrame - 1;
        }
        else
        {
            NextFrame = (CurrentFrame + 1) >= FrameCount ? 0 : CurrentFrame + 1;
        }

        animator.Play(animationClip.name, 0, (float)NextFrame / (float)FrameCount);

        CurrentFrame = NextFrame;

        Debug.Log("Current Frame: " + (float)NextFrame / (float)FrameCount);
    }


    [ContextMenu("Apply Player Transform Offset")]
    public void ApplyOffset()
    {
        PositionOffset = this.transform.position;
        RotationOffset = this.transform.eulerAngles;
        ScaleOffset = this.transform.localScale;

        Debug.Log("Offset Applied");
    }

}
