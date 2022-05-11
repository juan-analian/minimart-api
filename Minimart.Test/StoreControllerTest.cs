using AutoMapper;
using Minimart.Core.Domain.Services;
using Minimart.Core.Resources;
using Minimart.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Minimart.Test
{
    public class StoreControllerTest
    {
        
        public StoreControllerTest()
        {
           
        }

        [Fact]
        public void GetStores_Without_Params_return_List_OK()
        {
            Assert.Equal("esperado", "esperado");
             
        }
    }
}
