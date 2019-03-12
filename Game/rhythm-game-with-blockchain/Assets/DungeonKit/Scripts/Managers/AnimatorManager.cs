using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DungeonKIT
{
    public class AnimatorManager : MonoBehaviour
    {
        [Header("Components")]
        public PlayableDirector playableDirector;
        public Animator animator;

        [Header("Array of clips for playableDirector")]
        public TimelineAsset[] timelineAssets;

        //Play animation in animator PlayAnimation("Animation name")
        public void PlayAnimation(string animationName)
        {
            animator.Play(animationName);
        }

        //Play clip in playable director PlayPlayableDirector(timelineAssets[id], DirectorWrapMode. )
        public void PlayPlayableDirector(TimelineAsset timelineAsset, DirectorWrapMode directorWrap)
        {
            playableDirector.extrapolationMode = directorWrap; //Set director wrap mode in playableDirector
            playableDirector.playableAsset = timelineAsset; // Set clip 
            playableDirector.Play(); //Play
        }

    }

}