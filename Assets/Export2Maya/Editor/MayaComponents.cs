using UnityEngine;
using System.Collections;

// --------------------------------------------------
// MayaEdge Class
// --------------------------------------------------
// Helper class for storing Maya Edge data
public class MayaEdge {
	public int StartEdge;
	public int EndEdge;
	
	// --------------------------------------------------
	// Constructor
	// --------------------------------------------------
	public MayaEdge(int A, int B){
		StartEdge = A;
		EndEdge = B;
	}
}

// --------------------------------------------------
// MayaTerrainFace Class
// --------------------------------------------------
// Helper class for storing Unity terrain quad faces
//
// Technically terrains are just polygons, but there
// are certain optimizations we can assume on terrains
// because of their predictable structure
public class MayaTerrainFace {
	// --------------------------------------------------
	// Terrain Faces Setup
	// --------------------------------------------------
    //    |
    //    |
    //    Y+                                            
	//  ______                ______                __4___
	//  |    /|               |    /  /|            |    /     /|
	//  |   / |               | B /  / |            | B /     / |
	//  |  /  |               |  /  /  |           5|  /3   2/  |1
	//  | /   |               | /  / A |            | /     / A |
	//  |/____| X+ ---        |/  /____|            |/     /____|
	//                                                        0
	//  Terrain Layout       Face Triangles         Edge Numbers
	//
	// When building new faces:
	//		Edge 1 : Never shared
	//		Edge 2 : Never shared
	//		Edge 4 : Never shared
	//		Edge 3 : Always shared
	//		Edge 0 : Sometimes shared
	//		Edge 5 : Sometimes shared
	public int EdgeIndex0;
	public int EdgeIndex1;
	public int EdgeIndex2;
	public int EdgeIndex3;
	public int EdgeIndex4;
	public int EdgeIndex5;
	
	public int UVIndex0;
	public int UVIndex1;
	public int UVIndex2;
	public int UVIndex3;
	public int UVIndex4;
	public int UVIndex5;
	
	public int UV2Index0;
	public int UV2Index1;
	public int UV2Index2;
	public int UV2Index3;
	public int UV2Index4;
	public int UV2Index5;
	
	

}