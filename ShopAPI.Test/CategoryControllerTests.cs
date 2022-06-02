using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShopAPI.Controllers;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAPI.Test
{
   
    [TestFixture]
    public class CategoryControllerTests
    {
        /// <summary>
            /// Test the Action metjod returning Specific Index View
            /// </summary>
        /// 
        Mock<ApplicationDbContext> mockAppDB = new Mock<ApplicationDbContext>();
        Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
        Mock<ILogger<CategoryController>> mockILogger = new Mock<ILogger<CategoryController>>();        
        [SetUp]
        public void Setup()
        {
           
             mockAppDB = new Mock<ApplicationDbContext>();
             mockUnitOfWork = new Mock<IUnitOfWork>();
             mockILogger = new Mock<ILogger<CategoryController>>();
           
        }


        [Test]
        public void TestDepartmentIndex()
        {
            // Arrange
           // Category cat = new Category();
           // cat.Name = null;
           // cat.Description = null;
           // cat.CreatedDateTime = DateTime.Now;
            
           // mockUnitOfWork.Setup(repo => repo.Category.GetFirstOrDefault(1)).Throws<Exception>();

           //mockUnitOfWork.Object.Category.Add(cat);
           // Assert.Equals(1,cat.Id);

            //var actResult = obj.Index() as ViewResult;

            //Assert.That(actResult.ViewName, Is.EqualTo("Index"));
        }
        [Test]
        public void Get_GetAllCategory_NotPassingAnyArguments()
        {
            // Arrange
            Category cat = new Category();
            cat.Name = "string";
            cat.Description = "string";
            cat.CreatedDateTime = DateTime.Now;

            mockUnitOfWork.Setup(repo => repo.Category.GetAll(null, null))
                .Returns(new List<Category>
                {
                    new Category
                    {
                        Name="string", Description="string",CreatedDateTime=DateTime.Now,Id=1
                    }
                }.AsEnumerable()
            );

            //var controller = new CategoryController(mockAppDB.Object,mockUnitOfWork.Object,mockILogger.Object);

            var result= mockUnitOfWork.Object.Category.GetAll();

            Assert.AreEqual(1, result.Count());
            //Assert.That(actResult, Is.TypeOf();
        }
    }
}
