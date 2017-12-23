using System;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{	
	[Serializable]
	public class ResourceParameterAttribute : ParameterAttribute
	{
		private string _dataPath;
		[SerializeField]
		private string _selectedData;


		public ResourceParameterAttribute(string name, string dataPath, string defaultChoice) : base(name)
		{
			_dataPath = dataPath;
			_selectedData = defaultChoice;
		}

		public override ParameterEditor GetEditor()
		{
			List<string> options = new List<string>();

			UnityEngine.Object[] objects = Resources.LoadAll<UnityEngine.Object>(_dataPath);

			for (int objectIdx = 0; objectIdx < objects.Length; ++objectIdx)
			{
				options.Add(objects[objectIdx].name);
			}
			
			return new ListStringParameterEditor(name, options, _selectedData);
		}
	}
}
