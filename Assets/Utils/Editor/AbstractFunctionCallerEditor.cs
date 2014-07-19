using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;



public abstract class AbstractFunctionCallerEditor : Editor
{
	private List<Dictionary<string,object>> _parameterDetailsDict;


	void OnEnable()
	{
		_parameterDetailsDict = new List<Dictionary<string, object>>();
	}


	void OnDisable()
	{
		_parameterDetailsDict = null;
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var methodIndex = 0;
		foreach( var methodInfo in target.GetType().GetMethods() )
		{
			foreach( var attr in methodInfo.GetCustomAttributes( typeof( FunctionCallerAttribute ), true ) )
			{
				drawMethodGUI( methodIndex++, attr as FunctionCallerAttribute, methodInfo );
			}
		}
	}


	private void drawMethodGUI( int methodIndex, FunctionCallerAttribute attribute, MethodInfo methodInfo )
	{
		GUILayout.Space( 10 );

		if( _parameterDetailsDict.Count <= methodIndex )
			_parameterDetailsDict.Add( new Dictionary<string,object>() );
		var paramDict = _parameterDetailsDict[methodIndex];


		// draw the params
		foreach( var param in methodInfo.GetParameters() )
		{
			var paramKey = param.Name;

			if( !paramDict.ContainsKey( paramKey ) )
				paramDict.Add( paramKey, param.ParameterType.IsValueType ? System.Activator.CreateInstance( param.ParameterType ) : null );

			// add any supported types that you want here
			if( param.ParameterType == typeof( int ) )
				paramDict[paramKey] = EditorGUILayout.IntField( paramKey, (int)paramDict[paramKey] );
			else if( param.ParameterType == typeof( string ) )
				paramDict[paramKey] = EditorGUILayout.TextField( paramKey, (string)paramDict[paramKey] );
		}

		if( GUILayout.Button( attribute.buttonTitle ) )
		{
			var values = new object[paramDict.Count];
			paramDict.Values.CopyTo( values, 0 );
			methodInfo.Invoke( target, values );
		}
	}
}
