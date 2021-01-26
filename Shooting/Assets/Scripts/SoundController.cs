using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSoundType
{
    ExpAst,
    ExpEnem,
    ExpPlayer,
    FireEnem,
    FirePlayer
}

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource mBGM, mEffect;
    [SerializeField]
    private AudioClip[] mEffectArr;
    // Start is called before the first frame update
    
    public void PlayerEffectSound(int id)
    {        
        mEffect.PlayOneShot(mEffectArr[id]);
    }
}
