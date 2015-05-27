using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Ini
{
	/// <summary>
	/// Create a New INI file to store or load data
	/// </summary>
	public class IniFile
	{
		public static T Load<T>(string path) where T : class
		{
			var ini = new IniFile(path);
			var o = Activator.CreateInstance<T>();
			return ini.ReadTo(o) as T;
		}
		
		public static void Save<T>(T data, string path) where T : class
		{
			var ini = new IniFile();
			ini.SetData(data);
			ini.Write(path);
		}
		
		public IniFile(string path = null)
		{
			this.path = path ?? "";
		}
		
		public void SetData(object data)
		{
			this.data = data;
		}
		
		public object ReadTo(object result)
		{
			this.data = result;
			GetDataInfo();
			var f = System.IO.File.ReadAllLines(this.path);
			foreach (var s in f)
			{
				if (s.Count(c => c == '=') == 1)
				{
					var value = s.Split('=');
					var PropertyName = value[0].Trim();
					var Value = value[1].Trim();
					Assign(PropertyName, Value);
				}
			}
			return this.data;
		}
		
		public void Write(string path)
		{
			if (data == null)
			{
				throw new NullReferenceException();
			}
			
			var pArray = data.GetType().GetProperties();
			var content = new string[pArray.Length];
			for (var i = 0; i < pArray.Length; i++)
			{
				var name = pArray[i].Name;
				var value = pArray[i].GetValue(data, null);
				content[i] = String.Format("{0} = {1}", name, value.ToString());
			}
			System.IO.File.WriteAllLines(path, content);
		}
		
		#region Private
		private readonly string path;
		private object data;
		private Dictionary<string, PropertyInfo> properties = null;
		
		private void Assign(string PropertyName, string Value)
		{
			var value = Convert.ChangeType(Value, properties[PropertyName].PropertyType);
			properties[PropertyName].SetValue(data, value, null);
		}
		
		private void GetDataInfo()
		{
			var pArray = data.GetType().GetProperties();
			this.properties = new Dictionary<string, PropertyInfo>();
			foreach (var prop in pArray) {
				this.properties[prop.Name] = prop;
			}
		}
		#endregion
	}
}
