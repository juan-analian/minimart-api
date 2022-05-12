using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Services;
using Minimart.Core.Resources;
using Minimart.WebApi.Controllers;
using Minimart.WebApi.Mapping;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Minimart.Test
{
    public class StoreControllerTest
    {
        private static IMapper _mapper;
        private Mock<IStoreService> _mockStoreService;

        public StoreControllerTest()
        {

            if (_mapper == null)
            {
                //define DI for the profile class
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new ModelToResourceProfile()));
                _mapper = mappingConfig.CreateMapper(); 

            }

            _mockStoreService = new Mock<IStoreService>();       
        }   
    

        [Fact]
        public async Task GetStores_Without_Params_return_200_and_correct_resource()
        {
            //Arrange
            StoresController controller = new StoresController(_mockStoreService.Object, _mapper);

            //Act
            var actionResult = await controller.GetStores(null, null);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.IsType<List<StoreResource>>(okResult.Value);
        }
    }
}
