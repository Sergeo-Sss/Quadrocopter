// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using System.Collections;

public class AddUnit : MonoBehaviour {
	
	void Start () 
	{
		SelectObjects.unit.Add(gameObject);
	}
}
