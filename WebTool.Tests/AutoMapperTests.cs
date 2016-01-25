using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AutoMapper;
using iAgentDataTool.Models;
using iAgentDataTool.Models.Common;

namespace WebTool.Tests
{
    [TestFixture]
    public class AutoMapperTests
    {
        [Test]
        public void MapSomeStuffTest()
        {
            Mapper.CreateMap<ClientMaster, ClientLocations>();

            ClientMaster client = ClientMaster.CreateClientMaster(
                "Thing with the stuff",
                new Guid("2d4d1e74-b795-e011-ad7e-001e4f27a50b"),
                "SomethingSomthing"
            );

            ClientLocations clientLocation = Mapper.Map<ClientMaster, ClientLocations>(client);
            Console.WriteLine(clientLocation.ClientKey);
        }
    }
}
