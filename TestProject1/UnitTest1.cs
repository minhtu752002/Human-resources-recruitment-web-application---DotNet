using System.Net.Http;
using ResourceMNG;
using ResourceEntity;
using ResourceEntity.Models;

namespace TestProject1
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			int a =1; 
			int b = 2;
			bool result = (a + b) != 3;

			Assert.IsFalse(result, "a + b not equal 3");
		}
		[TestMethod]
		public void TestLoginSucess()
		{
			User user = new User();
			user.Username = "admin";
			user.Password = "123456";
			var context = new ResourceMngContext();
			var myuser = context.Users.Where(x => x.Username == user.Username).FirstOrDefault();
			Assert.IsTrue(myuser != null, "account login success");
		}
		[TestMethod]
		public void TestLoginFail()
		{
			User user = new User();
			user.Username = "admin";
			user.Password = "12345";
			var context = new ResourceMngContext();
			var myuser = context.Users.Where(x => x.Username == user.Username).FirstOrDefault();
			Assert.IsFalse(myuser != null, "account login fail");
		}


	}
}