using UnityEngine;
using System.Collections;

public class MarioControlls : MonoBehaviour 
{
	public float		marioAirTime = 0.4f;
	public GameObject	planeWithMarioAnimation;
	
	private float mLastJumpStartTime = -999;
	
	void OnGUI()
	{	
		AnimatedTextureObject lAnimation = planeWithMarioAnimation.GetComponent< AnimatedTextureObject >();
		
		Rect lJump = new Rect( 275, 440, 100, 40 );
		if( GUI.Button(lJump, "Jump" ) )
		{
			mLastJumpStartTime = Time.time;
			lAnimation.StartAnimation( 1 );
		}
		
		Rect lWalkRight = new Rect( 335, 485, 100, 40 );
		if( GUI.Button( lWalkRight, "Walk Right" ) )
		{
			lAnimation.StartAnimation( 0 );
			lAnimation.setReverseHorizontalDirection( false );
		}
		
		Rect lWalkLeft = new Rect( 215, 485, 100, 40 );
		if( GUI.Button( lWalkLeft, "Walk Left" ) )
		{
			lAnimation.StartAnimation( 0 );
			lAnimation.setReverseHorizontalDirection( true );
		}
	}
	
	void Update()
	{
		AnimatedTextureObject lAnimation = planeWithMarioAnimation.GetComponent< AnimatedTextureObject >();
		
		float lDeltaJumpTime = Time.time - mLastJumpStartTime;
		if( lDeltaJumpTime < marioAirTime )
		{
			Vector3 lPosition = planeWithMarioAnimation.transform.position;
			lPosition.y += (3 / marioAirTime) * Time.deltaTime;
			planeWithMarioAnimation.transform.position = lPosition;
		}
		else
		{
			float lDistanceToGround = planeWithMarioAnimation.transform.position.y;
			float lGravityThisFrame = (3 / marioAirTime) * Time.deltaTime;
			if( lDistanceToGround > lGravityThisFrame )
			{
				Vector3 lPosition = planeWithMarioAnimation.transform.position;
				lPosition.y -= lGravityThisFrame;
				planeWithMarioAnimation.transform.position = lPosition;
			}
			else
			{
				Vector3 lPosition = planeWithMarioAnimation.transform.position;
				lPosition.y = 0;
				planeWithMarioAnimation.transform.position = lPosition;
				if( lAnimation.currentActiveAnimation != 0 )
					lAnimation.StartAnimation( 0 );
			}			
		}
	}	
}
