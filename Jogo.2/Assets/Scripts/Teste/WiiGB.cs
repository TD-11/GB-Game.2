using System.Runtime.InteropServices;
using UnityEngine;

public class WiiGB : MonoBehaviour
{
	private static int max = 16;//CANNOT EXCEED 16!!! (this is a hard limit within the plugin)
	private static bool isAwake;
	private static State[] currentStates;
	private const string pluginName = "WiiBuddy64";

	
	[DllImport(pluginName)]
	private static extern bool checkPlugin();
	
	[DllImport(pluginName)]
	private static extern void initPlugin();
	
	[DllImport(pluginName)]
	[return : MarshalAs( UnmanagedType.Struct )]
	private static extern State getStates(int i);
	
	[DllImport(pluginName)]
	private static extern float getVirtualIR(int i, int r);
	
	[DllImport(pluginName)]
	private static extern float getIRSensitivity(int i);
	
	[DllImport(pluginName)]
	private static extern bool getLED(int i ,int led);
	
	[DllImport(pluginName)]
	[return : MarshalAs( UnmanagedType.Struct )]
	private static extern bool getVirtual(int i);

	private static bool[] isVirtual;
	private static bool[] wiiRemotesActive;
	private static float[] virtualIRRot;
	private static int highestNumberedPlayer = 0;
	private static float[] IRSensitivity;// (1-100);
	private static float[][] motionPlusCalib; 
	private static bool[][] theLEDs;
	private static State[] previousStates;
	private static bool[] rumbling;
	private static Vector2[] pointsDifference;
	private static int[]switchedIR;
	private static Vector3[] virtualIR;










	


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static Vector4 GetBalanceBoard(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.LogWarning("invalid remote number");
			return Vector4.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType != 3)
		{
    		return Vector4.zero;
    	}	
    	return new Vector4(currentStates[remote].expFloat5,
    		currentStates[remote].expFloat6,
    		currentStates[remote].expFloat7,
    		currentStates[remote].expFloat8);	
    }
    
    public static Vector4 GetRawBalanceBoard(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.LogWarning("invalid remote number");
			return Vector4.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType != 3)
    		return Vector4.zero;
    		
    	return new Vector4(currentStates[remote].expFloat1,
    		currentStates[remote].expFloat2,
    		currentStates[remote].expFloat3,
    		currentStates[remote].expFloat4);	
    }
    
    public static Vector2 GetCenterOfBalance(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.LogWarning("invalid remote number");
			return Vector2.zero;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType !=3)
    		return Vector2.zero;
    	
    	float leftSide   = currentStates[remote].expFloat6+currentStates[remote].expFloat8; 
    	float rightSide  = currentStates[remote].expFloat5+currentStates[remote].expFloat7;
    	float frontSide  = currentStates[remote].expFloat6+currentStates[remote].expFloat5; 
    	float backSide   = currentStates[remote].expFloat8+currentStates[remote].expFloat7;
    	float theX=0.0f;
    	float theY=0.0f;
    	
    	if(leftSide+rightSide != 0)
    		theX = (rightSide-leftSide)/(rightSide+leftSide);
    	if(frontSide+backSide != 0)
    		theY = (frontSide-backSide)/(frontSide+backSide);
    	 
    	 return new Vector2(theX,theY);
    }
    
    public static float GetTotalWeight(int remote)
    {
		if(remote<0 || remote>(max-1))
		{
			Debug.LogWarning("invalid remote number");
			return 0.0f;
    	}
		if(!isAwake)
	    	WakeUp();

    	if(currentStates[remote].expType !=3)
    		return 0.0f;
    	
    	return currentStates[remote].expFloat5+
    		currentStates[remote].expFloat6+
    		currentStates[remote].expFloat7+
    		currentStates[remote].expFloat8;
    }
    
    public static void WakeUp()
    {
	    currentStates    = new State[max];
	    previousStates   = new State[max];
	    wiiRemotesActive = new  bool[max];
	    isVirtual        = new bool[max];
	    motionPlusCalib  = new float[max][];
	    theLEDs          = new bool[max][];
	    rumbling         = new bool[max];
	    pointsDifference = new Vector2[max];
	    switchedIR       = new int[max];
	    IRSensitivity    = new float[max];
	    virtualIR        = new Vector3[max];
	    virtualIRRot     = new float[max];
    	
	    if(!checkPlugin())
	    {
		    initPlugin();
	    }
    	
	    int newHighest = 0;
	    for(int x=0;x<max;x++)
	    {
		    currentStates[x] = getStates(x);
		    isVirtual[x]     = getVirtual(x); 
		    if(currentStates[x].active || isVirtual[x])
		    {
			    wiiRemotesActive[x] = true;
			    if(x>=newHighest)
				    newHighest=(x+1);	
    			
			    virtualIR[x]    = new Vector3(getVirtualIR(x,0),getVirtualIR(x,1),getVirtualIR(x,2));
			    virtualIRRot[x] = getVirtualIR(x,3);
		    }
		    highestNumberedPlayer = newHighest;
		    IRSensitivity[x]   = getIRSensitivity(x);
		    motionPlusCalib[x] = new float[4];
		    theLEDs[x]         = new bool[4];
		    theLEDs[x][0] = getLED(x,0);
		    theLEDs[x][1] = getLED(x,1);
		    theLEDs[x][2] = getLED(x,2);
		    theLEDs[x][3] = getLED(x,3);
    		
	    }
	    isAwake = true;
    }
    
    [StructLayout(LayoutKind.Explicit)]
    public struct State
    {
    	[FieldOffset(0)]public bool active;
		[FieldOffset(1)]public bool a;
		[FieldOffset(2)]public bool b;
		[FieldOffset(3)]public bool up;
		[FieldOffset(4)]public bool down;
		[FieldOffset(5)]public bool left;
		[FieldOffset(6)]public bool right;
		[FieldOffset(7)]public bool one;
		[FieldOffset(8)]public bool two;
		[FieldOffset(9)]public bool plus;
		[FieldOffset(10)]public bool minus;
		[FieldOffset(11)]public bool home;
    	[FieldOffset(12)]public bool expButton1;
    	[FieldOffset(13)]public bool expButton2;
    	[FieldOffset(14)]public bool expButton3;
    	[FieldOffset(15)]public bool expButton4;
    	[FieldOffset(16)]public bool expButton5;
    	[FieldOffset(17)]public bool expButton6;
    	[FieldOffset(18)]public bool expButton7;
    	[FieldOffset(19)]public bool expButton8;
    	[FieldOffset(20)]public bool expButton9;
    	[FieldOffset(21)]public bool expButton10;
    	[FieldOffset(22)]public bool expButton11;
    	[FieldOffset(23)]public bool expButton12;
    	[FieldOffset(24)]public bool expButton13;
		[FieldOffset(25)]public bool expButton14;
		[FieldOffset(26)]public bool expButton15;
    	[FieldOffset(27)]public bool yawFast;
    	[FieldOffset(28)]public bool rollFast;
    	[FieldOffset(29)]public bool pitchFast;

    	[FieldOffset(30)]public bool motionPlusAvailable;
    	[FieldOffset(31)]public bool motionPlusCalibrated;
		
		[FieldOffset(32)]public char expType;
		[FieldOffset(36)]public float battery;
		
		[FieldOffset(40)]public float wiiAccelX;
		[FieldOffset(44)]public float wiiAccelY;
		[FieldOffset(48)]public float wiiAccelZ;
		
    	[FieldOffset(52)]public float expFloat1;
    	[FieldOffset(56)]public float expFloat2;
    	[FieldOffset(60)]public float expFloat3;
    	[FieldOffset(64)]public float expFloat4;
    	[FieldOffset(68)]public float expFloat5;
    	[FieldOffset(72)]public float expFloat6;
    	[FieldOffset(76)]public float expFloat7;
    	[FieldOffset(80)]public float expFloat8;
    	
    	[FieldOffset(84)]public float yaw;
    	[FieldOffset(88)]public float roll;
    	[FieldOffset(92)]public float pitch;
    	
    	[MarshalAs(UnmanagedType.Struct)]
    	[FieldOffset(96)]public IRData rawIR1;
		[MarshalAs(UnmanagedType.Struct)]
		[FieldOffset(102)]public IRData rawIR2;
		[MarshalAs(UnmanagedType.Struct)]
		[FieldOffset(108)]public IRData rawIR3;
		[MarshalAs(UnmanagedType.Struct)]
		[FieldOffset(114)]public IRData rawIR4;
		#if  UNITY_64 && (UNITY_STANDALONE_WIN || ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)&& UNITY_2017_1_OR_NEWER))
		[FieldOffset(120)]public float empty;
		
		public State(bool expButton1)
		{
			this.expButton1 = expButton1;
			active = false;
			a = false;
			b = false;
			up = false;
			down = false;
			left = false;
			right = false;
			one = false;
			two = false;
			plus = false;
			minus = false;
			home = false;
			expButton2 = false;
			expButton3 = false;
			expButton4 = false;
			expButton5 = false;
			expButton6 = false;
			expButton7 = false;
			expButton8 = false;
			expButton9 = false;
			expButton10 = false;
			expButton11 = false;
			expButton12 = false;
			expButton13 = false;
			expButton14 = false;
			expButton15 = false;
			yawFast = false;
			rollFast = false;
			pitchFast = false;
			motionPlusAvailable = false;
			motionPlusCalibrated = false;
			expType = '\0';
			battery = 0;
			wiiAccelX = 0;
			wiiAccelY = 0;
			wiiAccelZ = 0;
			expFloat1 = 0;
			expFloat2 = 0;
			expFloat3 = 0;
			expFloat4 = 0;
			expFloat5 = 0;
			expFloat6 = 0;
			expFloat7 = 0;
			expFloat8 = 0;
			yaw = 0;
			roll = 0;
			pitch = 0;
			rawIR1 = default;
			rawIR2 = default;
			rawIR3 = default;
			rawIR4 = default;
			empty = 0;
		}
#endif
    }
    
    [StructLayout(LayoutKind.Explicit)]
    public struct IRData
    {
	    [FieldOffset(0)]public short x;
	    [FieldOffset(2)]public short y;
	    [FieldOffset(4)]public short s;
        
	    public IRData(short X, short Y, short S)
	    {
		    x = X;
		    y = Y;
		    s = S;
	    }
    }
}
