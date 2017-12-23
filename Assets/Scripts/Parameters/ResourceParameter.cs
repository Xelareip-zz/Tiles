using System;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{	
	[Serializable]
	public class ResourceParameterAttribute : ParameterAttribute
	{
		private string _dataPath;

		public ResourceParameterAttribute(string name, string dataPath) : base(name)
		{
			_dataPath = dataPath;
		}

		public override ParameterEditor GetEditor(object val)
		{
			List<string> options = new List<string>();

			UnityEngine.Object[] objects = Resources.LoadAll<UnityEngine.Object>(_dataPath);

			for (int objectIdx = 0; objectIdx < objects.Length; ++objectIdx)
			{
				options.Add(objects[objectIdx].name);
			}
			
			return new ListStringParameterEditor(name, options, (string)val);
		}
	}
}
