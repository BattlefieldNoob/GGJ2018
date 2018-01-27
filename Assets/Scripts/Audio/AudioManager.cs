//=====================================================================
// Global Game Jam 2018
// Written by: Giacomo Garoffolo
//=====================================================================

using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        
        /// <summary>
        /// Per gli eventi audio che possono anche stopparsi e avere cambi di parametro
        /// </summary>
        public static EventInstance PlayAudio(string eventRef)
        {
            var audioInstance = PlayAudio(eventRef, Instance.gameObject);
            
            return audioInstance;
        }

        
        
        /// <summary>
        /// Per gli eventi audio che possono anche stopparsi e avere cambi di parametro
        /// </summary>
        public static EventInstance PlayAudio(string eventRef, GameObject obj)
        {
            var audioInstance = RuntimeManager.CreateInstance(eventRef);

            if (audioInstance.isValid())
            {
                RuntimeManager.AttachInstanceToGameObject(audioInstance, obj.transform, (Rigidbody) null);
                audioInstance.start();
            }
            return audioInstance;
        }



        /// <summary>
        /// Per gli eventi audio che vengono triggerati, suonano fino alla fine e muoiono da soli
        /// </summary>
        public static void PlayOneShotAudio(string eventRef, GameObject obj)
        {
            RuntimeManager.PlayOneShotAttached(eventRef, obj);
        }

        
        
        
        
        /// <summary>
        /// Passando l'istanza si puo chiamare la stop di un suono
        /// di default c'è un fade out ma è possibile bloccare il suono immediatamente
        /// </summary>
        public static void StopAudio(EventInstance audioInstance, bool immediately = false)
        {
            if (!audioInstance.isValid()) return;
            
            audioInstance.stop(immediately ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
            audioInstance.release();
        }

        
        
        /// <summary>
        /// Per gli eventi audio che vengono triggerati, suonano fino alla fine e muoiono da soli
        /// </summary>
        public static void SetParameterToInstance(EventInstance audioInstance, string parameterName,
            float parameterValue)
        {
            audioInstance.setParameterValue(parameterName, parameterValue);
        }
        
        
        
        

        public static bool IsPlaying(EventInstance audioInstance)
        {
            if (!audioInstance.isValid()) return false;

            PLAYBACK_STATE playbackState;
            audioInstance.getPlaybackState(out playbackState);
            return (playbackState != PLAYBACK_STATE.STOPPED);
        }
    }
}