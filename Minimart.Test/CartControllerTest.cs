using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Services;
using Minimart.Core.Domain.Services.Communication;
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
    public class CartControllerTest
    {
        private static IMapper _mapper;

        public CartControllerTest()
        {

            if (_mapper == null)
            {
                //define DI for the profile class
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new ModelToResourceProfile()));
                _mapper = mappingConfig.CreateMapper();
            }
        }

        #region Create new Cart tests
        [Fact]
        public async Task CreateCart_Without_Params_return_400_bad_request()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.AddNewCart(null, 0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }


        [Fact]
        public async Task CreateCart_With_invalid_model_state_return_400_bad_request()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);
            controller.ModelState.AddModelError("ProductId", "ProductId is required.");

            //Act
            var actionResult = await controller.AddNewCart(new CartItemSaveResource(), 0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }


        [Fact]
        public async Task CreateCart_With_storeId_0_return_400_bad_request()
        {
            //Arrange
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);
              
            //Act
            var actionResult = await controller.AddNewCart(new CartItemSaveResource() {ProductId = 1, Quantity =1 }, 0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }


        [Fact]
        public async Task CreateCart_With_correct_parameters_return_guid_200_OK()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.Create(1, 1, 1)).ReturnsAsync( () => new NewCartResponse(guid));  

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.AddNewCart(new CartItemSaveResource() { ProductId = 1, Quantity = 1 }, 1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);

            //TODO!: find out how to. Anonymous object created in separate assemblies, cannot be compared as the same type.
            //var respGuid = new { Guid = guid };
            //Assert.Equal(respGuid, okResult.Value);


        }


        [Fact]
        public async Task CreateCart_with_internal_error_return_500()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.Create(1, 1, 1)).Throws( new Exception()) ;

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.AddNewCart(new CartItemSaveResource() { ProductId = 1, Quantity = 1 }, 1);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(actionResult);

            var httpCode = ((ObjectResult)actionResult).StatusCode.Value;
            Assert.Equal((int)500, httpCode);             
        }
        

        #endregion

        #region Add Items to Cart Tests
        [Fact]
        public async Task AddItemsToCart_return_guid_200_OK()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.AddItem(guid, 1, 1)).ReturnsAsync(() => new NewCartResponse(guid));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.AddItem(new CartItemSaveResource() { ProductId = 1, Quantity = 1 }, guid);

            //Assert             
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
        }


        [Fact]
        public async Task AddItemsToCart_With_invalid_Model_return_400_BadRequest()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.AddItem(guid, 1, 1)).ReturnsAsync(() => new NewCartResponse(guid));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);
            controller.ModelState.AddModelError("Quantity", " Quantity is required");
            //Act
            var actionResult = await controller.AddItem(new CartItemSaveResource() { ProductId = 1, Quantity = 1 }, guid);

            //Assert             
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }


        [Fact]
        public async Task AddItemsToCart_With_not_existing_guid_return_400_BadRequest()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.AddItem(guid, 1, 1)).ReturnsAsync(() => new NewCartResponse("Cart not found error"));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.AddItem(new CartItemSaveResource() { ProductId = 1, Quantity = 1 }, guid);

            //Assert             
            Assert.IsType<BadRequestObjectResult>(actionResult);
        } 
        #endregion

        #region Remove items tests

        [Fact]
        public async Task RemoveItem_return_204_NoContent()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.RemoveItem(guid, 1)).ReturnsAsync(() => new NewCartResponse(guid));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.RemoveItem(guid, 1);

            //Assert             
            Assert.IsType<NoContentResult>(actionResult);

            var httpCode = ((NoContentResult)actionResult).StatusCode;
            int expected = 204;
            Assert.Equal(expected, httpCode);
        }

        [Fact]
        public async Task RemoveItem_return_400_BadRequest()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.RemoveItem(guid, 1)).ReturnsAsync(() => new NewCartResponse("cart not found"));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.RemoveItem(guid, 1);

            //Assert             
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var httpCode = ((BadRequestObjectResult)actionResult).StatusCode.Value;
            int expected = 400;
            Assert.Equal(expected, httpCode);
        }


        #endregion

        #region Apply Voucher
        [Fact]
        public async Task Apply_existing_voucher_return_204_NoContent()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.ApplyVoucher(guid, "voucher")).ReturnsAsync(() => new NewCartResponse(guid));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.ApplyVoucher(guid, "voucher");

            //Assert             
            Assert.IsType<NoContentResult>(actionResult);

            var httpCode = ((NoContentResult)actionResult).StatusCode;
            int expected = 204;
            Assert.Equal(expected, httpCode);
        }

        [Fact]
        public async Task Apply_not_existing_voucher_return_400_BadRequest()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.ApplyVoucher(guid, "voucher")).ReturnsAsync(() => new NewCartResponse("Voucher id not found"));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.ApplyVoucher(guid, "voucher");

            //Assert                                  
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var httpCode = ((BadRequestObjectResult)actionResult).StatusCode.Value;
            int expected = 400;
            Assert.Equal(expected, httpCode);
        }
        #endregion

        #region Get Cart With items
        [Fact]
        public async Task GetCart_existing_Id_return_200_Ok()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.GetCart(guid, null)).ReturnsAsync(() => new CartResponse(new Cart()));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetCart(guid);

            //Assert                                  
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.IsType<CartResource>(okResult.Value);
        }
        [Fact]
        public async Task GetCart_whit_Id_return_400_BadRequest()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var _mockProductService = new Mock<IProductService>();
            var _mockStoreService = new Mock<IStoreService>();
            var _mockCartService = new Mock<ICartService>();
            _mockCartService.Setup(s => s.GetCart(guid, null)).ReturnsAsync(() => new CartResponse("cart not found"));

            var controller = new CartsController(_mockCartService.Object, _mockProductService.Object, _mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetCart(guid);

            //Assert                                                               
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var httpCode = ((BadRequestObjectResult)actionResult).StatusCode.Value;
            int expected = 400;
            Assert.Equal(expected, httpCode);
        }

        #endregion

    }
}
