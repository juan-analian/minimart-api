using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Services;
using Minimart.Core.Resources;
using Minimart.WebApi.Controllers;
using Minimart.WebApi.Mapping;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Minimart.Test
{
    public class ProductsControllerTest
    {
        private static IMapper _mapper;
        
        public ProductsControllerTest()
        {

            if (_mapper == null)
            {
                //define DI for the profile class
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new ModelToResourceProfile()));
                _mapper = mappingConfig.CreateMapper();
            }
        }

        [Fact]
        public async Task Getproducts_Without_Params_return_200_and_correct_resource()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var controller = new ProductsController(_mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetProducts(null);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.IsType<List<ProductResource>>(okResult.Value);
        }

        [Fact]
        public async Task GetSingleProduct_Without_Params_return_404_NotFound()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var controller = new ProductsController(_mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetProductFromStore(0, 0);

            //Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
            
        }

        [Fact]
        public async Task GetSingleProduct_With_ProductId_Without_StoreId_return_404_NotFound()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();
            _mockProductService.Setup(s => s.FindById(0)).ReturnsAsync(new Product());

            var _mockStoreService = new Mock<IStoreService>();
            var controller = new ProductsController(_mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetProductFromStore(0, 0);

            //Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);

        }

        [Fact]
        public async Task GetSingleProduct_Without_ProductId_With_StoreId_return_404_NotFound()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();           
            var _mockStoreService = new Mock<IStoreService>();
            _mockStoreService.Setup(s => s.FindById(0)).ReturnsAsync(new Store());

            var controller = new ProductsController(_mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetProductFromStore(0, 0);

            //Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);

        }


        [Fact]
        public async Task GetSingleProduct_With_all_parameters_return_200_Ok()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();
            _mockProductService.Setup(s => s.FindById(0)).ReturnsAsync(new Product());
            _mockProductService.Setup(s => s.FindByIdAndStoreId(0,0)).ReturnsAsync(new Product());

            var _mockStoreService = new Mock<IStoreService>();
            _mockStoreService.Setup(s => s.FindById(0)).ReturnsAsync(new Store());
            var controller = new ProductsController(_mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetProductFromStore(0, 0);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.IsType<ProductResource>(okResult.Value);

        }


    }
}
