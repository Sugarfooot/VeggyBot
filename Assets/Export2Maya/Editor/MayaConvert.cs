using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------
// MayaConvert Class
// --------------------------------------------------
// Contains methods to convert translations, rotations
// and matrices from Unity to Maya.
public static class MayaConvert {
	// --------------------------------------------------
	// Converts Unity -> Maya Translation
	// --------------------------------------------------
	// Converts a Vector3 translation to Maya translation
	public static Vector3 MayaTranslation(Vector3 t){
		return new Vector3(-t.x, t.y, t.z);
	}
	
	// --------------------------------------------------
	// Converts Unity -> Maya Euler Rotation
	// --------------------------------------------------
	// Converts a Vector3 euler rotation to Maya rotation
	public static Vector3 MayaRotation(Vector3 r){
		return new Vector3(r.x, -r.y, -r.z);
	}
	
	// --------------------------------------------------
	// Converts Matrix4x4 -> Float Array[]
	// --------------------------------------------------
	public static float[] Matrix4x4ToFloatArray(Matrix4x4 m){
		float[] f = new float[16];
		Vector4 row0 = m.GetRow(0);
		Vector4 row1 = m.GetRow(1);
		Vector4 row2 = m.GetRow(2);
		Vector4 row3 = m.GetRow(3);
		
		f[0] = row0.x;		f[1] = row0.y;		f[2] = row0.z;		f[3] = row0.w;
		f[4] = row1.x;		f[5] = row1.y;		f[6] = row1.z;		f[7] = row1.w;
		f[8] = row2.x;		f[9] = row2.y;		f[10] = row2.z;		f[11] = row2.w;
		f[12] = row3.x;		f[13] = row3.y;		f[14] = row3.z;		f[15] = row3.w;
		
		return f;
	}
	
	// --------------------------------------------------
	// Converts Unity Matrix -> Maya Matrix
	// --------------------------------------------------
	public static float[] UnityMatrixToMayaMatrix(float[] m){
		float[] f = new float[16];
		
		f[0] = m[0];		f[1] = m[4] * -1;	f[2] = m[8] * -1;	f[3] = m[12];
		f[4] = m[1] * -1;	f[5] = m[5];		f[6] = m[9];		f[7] = m[13];
		f[8] = m[2] * -1;	f[9] = m[6];		f[10] = m[10];		f[11] = m[14];
		f[12] = m[3] * -1;	f[13] = m[7];		f[14] = m[11];		f[15] = m[15];
		
		return f;
	}
	
	// --------------------------------------------------
	// Extracts Translation values from Maya Matrix
	// --------------------------------------------------
	public static Vector3 MayaMatrixGetTranslation(float[] m){
		Vector3 t;
		t.x = m[12];
		t.y = m[13];
		t.z = m[14];
		return t;
	}

	// --------------------------------------------------
	// Extracts Rotation values from Maya Matrix
	// --------------------------------------------------
	public static Vector3 MayaMatrixGetRotation(float[] m){
		Vector3 forward;
		forward.x = m[8];	
		forward.y = m[9];
		forward.z = m[10];
	 
		Vector3 upwards;
		upwards.x = m[4];
		upwards.y = m[5];
		upwards.z = m[6];
	 
		Quaternion q = Quaternion.LookRotation(forward, upwards);
		Vector3 r = q.eulerAngles;
		
		return new Vector3(r.x, r.y, r.z);
	}

	// --------------------------------------------------
	// Extract Scale values from Maya Matrix
	// --------------------------------------------------
	public static Vector3 MayaMatrixGetScale(float[] m){
		Vector3 scale;
		scale.x = new Vector4(m[0], m[1], m[2], m[3]).magnitude;
		scale.y = new Vector4(m[4], m[5], m[6], m[7]).magnitude;
		scale.z = new Vector4(m[8], m[9], m[10], m[11]).magnitude;
		return scale;
	}
}
