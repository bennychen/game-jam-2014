using UnityEngine;
using System;
using System.Collections;


[AttributeUsage( AttributeTargets.Method )]
public class FunctionCallerAttribute : Attribute
{
	public readonly string buttonTitle;


	public FunctionCallerAttribute( string buttonTitle )
	{
		this.buttonTitle = buttonTitle;
	}
}
