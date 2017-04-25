using UnityEngine;
using System.Collections;

public class moverPlat : MonoBehaviour {


	public int dirPlat;
	int frameCount;


	void Update ()
	{

		frameCount++;  

		if (frameCount == 470)
		{
			frameCount = 0;

			if (dirPlat == 0)
				dirPlat = 1;
			else
				dirPlat = 0;
		}



		if (dirPlat == 0)
		{
			transform.Translate(Vector3.up * 0.05f);
		}

		if (dirPlat  == 1)
		{
			transform.Translate(Vector3.down * 0.05f);
		}

	}
}