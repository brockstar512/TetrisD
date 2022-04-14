using UnityEngine;
using System.Collections;

/*************************************************************************************
/* ANIMATED TEXTURE
/* Internal class, used for exposing texture related variables in the viewer.
/* This is only public because it needs to be for the viewer, not because it needs to be used elsewhere.
/*************************************************************************************/
[System.Serializable]
public class AnimatedTexture
{
	public Texture	textureWithFrames;
	public int		numberOfFramesHorizontal	= 1;
	public int		numberOfFramesVertical		= 1;
	public bool 	needsFlip 					= true;
	
	private float mOneFrameUstep;
	private float mOneFrameVstep;
	
	// Precalculate the stepsize to save us from having to recalculate it each frame.
	public void Init()
	{
		mOneFrameUstep = 1.0f / (float)numberOfFramesHorizontal;
		mOneFrameVstep = 1.0f / (float)numberOfFramesVertical;
	}
	
	// Get Frame UV's method.
	// Returns an array containing 2 Vector2's.
	// The first is the bottom left texture coordinate of the requested frame.
	// The second is the distance to the top right coordinate.
	// Both return values are in 0 to 1, texture coordinates.
	public Vector2[] getUVsForFrame( int aFrameNumber )
	{	
		float lUToReturn = 0;
		float lVToReturn = 0;
		
		int atRow = aFrameNumber / (numberOfFramesHorizontal);
		
		if( needsFlip == true && aFrameNumber % numberOfFramesHorizontal == 0 )
		{
			// When flipped, we are reading each frame right to left.
			// So for the frame at the end of each row, 
			// even though the horizontal coordinate is just past the end of the row
			// we will be reading the pixels to the left of that coordinate.
			// Meaning we jump back 1 row, but only for the last frame on the row.
			atRow -= 1;
		}
		
		int atCollom = aFrameNumber - ( atRow * numberOfFramesHorizontal);
		
		lUToReturn = mOneFrameUstep * atCollom;
		lVToReturn = mOneFrameVstep * atRow;
		
		Vector2[] toReturn = new Vector2[2];
		
		if( needsFlip == true )
		{
			toReturn[0] = new Vector2( lUToReturn, 1.0f - lVToReturn );
			toReturn[1] = new Vector2( -mOneFrameUstep, -mOneFrameVstep);
		}
		else
		{
			toReturn[0] = new Vector2( lUToReturn, lVToReturn );
			toReturn[1] = new Vector2( mOneFrameUstep, mOneFrameVstep);
		}
		
		return toReturn;
	}
}

/*************************************************************************************
/* TEXTURE ANIMATION
/* Internal class, used for exposing animation variables in the viewer.
/* This is only public because it needs to be for the viewer, not because it needs to be used elsewhere.
/*************************************************************************************/
[System.Serializable]
public class TextureAnimation
{
	[SerializeField]
	public AnimatedTexture	animatedTex;
	
	public int				startAtFrame			= 0;
	public int				endAtFrame				= 0;
	public float			animationDuration		= 1;
	public bool 			loop					= true;
	public bool 			pingpong				= false;
	public int 				goToAnimationOnEnd		= -1;
	
	private float			mTimePlayed				= 0;
	private bool			mPlaying				= false;
	private int				mDeltaFrames			= 0;
	private float 			mTimePerFrame			= 0;
	
	private Vector2[] 		mUVDataThisFrame; 
	
	private float			mTimeToUseForFrameCalc;
	private bool			mSendEndedNotification	= false;
	
	// precalculate what we can
	public void InitTexture()
	{
		animatedTex.Init();
		mDeltaFrames = endAtFrame - startAtFrame;
		mTimePerFrame = animationDuration / (mDeltaFrames + 1);
		// +1 frame because we want the final frame to stay on screen for equal time
		
		// make sure the texture is ready at the first frame
		mUVDataThisFrame = animatedTex.getUVsForFrame( startAtFrame );
	}
	
	public void Play()
	{
		mPlaying = true;
	}
	
	public void Pause()
	{
		mPlaying = false;
	}
	
	public void Stop()
	{
		Pause();
		mTimePlayed = 0;
	}
	
	// The animation update method.
	// This increments the internal time and sets new UV coordinates for the material passed.
	// It returns true when the animation is ready to swap to the next animation.
	public bool Update( float aDeltaTime, Material aMaterial, bool aXFlipped )
	{
		UpdateTime( aDeltaTime );
			
		if( mPlaying == true )
		{
			// only update the UVs if there is an animation to get new UVs from
			if( mDeltaFrames != 0 )
			{
				int numberOfFramesElapsed;
				if( mDeltaFrames > 0 )
				{
					numberOfFramesElapsed = Mathf.FloorToInt(mTimeToUseForFrameCalc / mTimePerFrame);
				}
				else
				{
					// unfortunately floor and ceil do exactly the opposite to negative numbers
					numberOfFramesElapsed = Mathf.CeilToInt(mTimeToUseForFrameCalc / mTimePerFrame);
				}
			
				int atFrame = startAtFrame + numberOfFramesElapsed;
				if( mTimePlayed >= animationDuration && pingpong == false )
					mUVDataThisFrame = animatedTex.getUVsForFrame( endAtFrame );
				else
					mUVDataThisFrame = animatedTex.getUVsForFrame( atFrame );
			}
			
			if( aXFlipped == true )
			{
				// go forward 1 frame
				mUVDataThisFrame[0].x += mUVDataThisFrame[1].x;
				// reverse the direction of X
				mUVDataThisFrame[1].x = -mUVDataThisFrame[1].x;
			}
			
			updateUVsForMaterial( aMaterial );
		}
		
		return mSendEndedNotification;
	}
	
	public void updateUVsForMaterial( Material aMaterial )
	{
		aMaterial.SetTextureOffset(  "_MainTex" , mUVDataThisFrame[0] );
		aMaterial.SetTextureScale(  "_MainTex" , mUVDataThisFrame[1] );
	}
	
	public bool UpdateTime( float aDeltaTime )
	{
		mSendEndedNotification = false;
		
		if( mPlaying == true )
		{
			mTimePlayed += aDeltaTime;
			mTimeToUseForFrameCalc = mTimePlayed;
			// check if the animation has ended
			if( mTimePlayed > animationDuration )
			{
				// loop simply starts over at the beginning.
				if( loop == true )
				{
					mTimePlayed -= animationDuration;
					mTimeToUseForFrameCalc -= animationDuration;
				}
				// pingpong does not reset the time
				else if( pingpong == true )
				{
					float lClippedDuration = animationDuration - (mTimePerFrame * 2); // the last frame and first frame are not needed
					float lPingPongDuration = animationDuration + lClippedDuration;
					if( mTimePlayed <= lPingPongDuration )
					{
						// instead pingpong lets the animation continue, but any overtime becomes reverse time
						mTimeToUseForFrameCalc = mTimePerFrame + lClippedDuration - ( mTimePlayed - animationDuration);
					}
					else 
					{
						// after the overtime equals the duration, it starts over from the beginning.
						mTimePlayed -= lPingPongDuration;
						mTimeToUseForFrameCalc -= lPingPongDuration;
					}
				}
				else
				{
					// the animation is simply over, so we freeze it at the last frame
					mTimePlayed = animationDuration;
					mTimeToUseForFrameCalc = animationDuration;
					if( goToAnimationOnEnd >= 0 )
					{
						mSendEndedNotification = true;
					}
				}
			}
		}
		
		return mSendEndedNotification;
	}
	
	public Texture getTexture()
	{
		InitTexture(); // Make sure the texture is ready before passing it.
		return animatedTex.textureWithFrames;
	}
}


/*************************************************************************************
/* ANIMATED TEXTURE OBJECT
/* The actual class that will be responsible for animating the object.
/*************************************************************************************/
public class AnimatedTextureObject : MonoBehaviour
{
	[SerializeField]
	public TextureAnimation[]	animations;
	public int					currentActiveAnimation = 0;
	
	private bool				mActiveAnimation	= false;
	private bool				mXFlipped			= false;

	void Start()
	{
		StartAnimation( currentActiveAnimation );
	}
	
	public bool StartAnimation( int aAnimationIndex )
	{
		// Make sure to only play animations that we actually have data for.
		if( animations.Length > aAnimationIndex )
		{
			currentActiveAnimation = aAnimationIndex;
			// Different animations on an object can read from different textures, so we set the texture to the one we need.
			gameObject.GetComponent<Renderer>().material.SetTexture( "_MainTex", animations[aAnimationIndex].getTexture() );
			animations[aAnimationIndex].updateUVsForMaterial( gameObject.GetComponent<Renderer>().material );
			// The animation might have been playing previously, we probably want to reset which frame it was at back to the first.
			animations[aAnimationIndex].Stop();
			animations[aAnimationIndex].Play();
			mActiveAnimation = true;
		}
		else
		{
			mActiveAnimation = false;
		}
		return mActiveAnimation;
	}
	
	void Update()
	{
		if( mActiveAnimation == true )
		{
			bool lAnimationEnded = false;
			// The isVisible flag is an 'is object on screen' check, no need to recalculate UV on something we can't see.
			if (GetComponent<Renderer>().isVisible == true )
				lAnimationEnded = animations[currentActiveAnimation].Update( Time.deltaTime, gameObject.GetComponent<Renderer>().material, mXFlipped );
			else
				lAnimationEnded = animations[currentActiveAnimation].UpdateTime( Time.deltaTime );
			
			if( lAnimationEnded == true )
				StartAnimation( animations[currentActiveAnimation].goToAnimationOnEnd );
		}
	}
	
	public void Play()
	{
		if( animations.Length > currentActiveAnimation )
		{
			animations[currentActiveAnimation].Play();
		}
	}
	
	public void Pause()
	{
		if( animations.Length > currentActiveAnimation )
		{
			animations[currentActiveAnimation].Pause();
		}
	}
	
	public void Stop()
	{
		if( animations.Length > currentActiveAnimation )
		{
			animations[currentActiveAnimation].Stop();
			mActiveAnimation= false;
		}
	}
	
	public void setReverseHorizontalDirection( bool to )
	{
		mXFlipped = to;
	}
}
