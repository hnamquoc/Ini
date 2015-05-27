/*
 * Created by SharpDevelop.
 * User: Nam
 * Date: 27/05/2015
 * Time: 11:12 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using Ini;

namespace Ini.Test
{
	[TestFixture]
	public class TestIniFile
	{
		#region Test Data
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public float Height { get; set; }
		#endregion
		
		private readonly string path = @"E:\test.ini";
		private TestIniFile data = null;
		
		[TestFixtureSetUp]
		public void SetUpData()
		{
			this.data = new TestIniFile {
				FirstName = "Nam",
				MiddleName = "Quoc",
				LastName = "Huynh",
				Age = 24,
				Height = 1.73f
			};
		}
		
		[TestFixtureTearDown]
		public void Clear()
		{
			this.data = null;
		}
		
		[Test]
		public void TestSaveMethod()
		{
			IniFile.Save<TestIniFile>(data, path);
			Assert.True(System.IO.File.Exists(path));
		}
		
		[Test]
		public void TestLoadMethod()
		{
			var res = IniFile.Load<TestIniFile>(path);
			Assert.AreEqual(res.Age, 24);
		}
	}
}
