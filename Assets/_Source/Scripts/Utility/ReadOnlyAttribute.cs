/**
 * ReadOnlyAttribute.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 05/06/18 (dd/mm//yy)
 * Reference from It3ration: https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
 */

using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute 
{
}