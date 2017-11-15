﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Machete.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machete.Test.Integration;
using Machete.Domain;
using Machete.Service;
using AutoMapper;

namespace Machete.Test.Integration.Services
{
    [TestClass]
    public class OnlineOrdersServiceTest
    {
        [TestClass]
        public class ReportsV2ServiceTests
        {
            DTO.SearchOptions o;
            FluentRecordBase frb;
            WorkOrder wo;
            OnlineOrdersService serv;
            IMapper map;

            [ClassInitialize]
            public static void ClassInitialize(TestContext c) {}

            [TestInitialize]
            public void TestInitialize()
            {
                frb = new FluentRecordBase();
            }

            [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
            public void AutoMapper_OnlineOrder()
            {
                //
                //Arrange
                var wo = frb.CloneOnlineOrder();
                var serv = frb.ToServOnlineOrders();
                var map = frb.ToApiMapper();
                //
                //Act
                var result = map.Map<Api.ViewModel.WorkOrder,  Machete.Domain.WorkOrder>(wo);
                //
                //Assert
                Assert.IsNotNull(result, "DTO.WorkOrderList is Null");
                Assert.IsTrue(result.GetType() == typeof(Machete.Domain.WorkOrder));
            }

            [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
            public void CreateOnlineOrder_Succeeds()
            {
                //
                // Arrange
                var e = frb.ToEmployer();
                var wo = frb.CloneWorkOrder();
                var lserv = frb.ToServLookup();

                wo.zipcode = "98118";
                wo.EmployerID = e.ID;
                var ll = lserv.Get(l => l.key == "transport_bus");
                wo.transportMethodID = ll.ID;
                wo.workAssignments = new List<WorkAssignment>();
                var wa = frb.CloneWorkAssignment();
                wa.transportCost = 5;
                wo.workAssignments.Add(wa);
                var serv = frb.ToServOnlineOrders();

                // 
                // Act
                var result = serv.Create(wo, "CreateOnlineOrder_Succeeds");
                //
                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.workAssignments);
                Assert.IsTrue(result.workAssignments.Count() == 1);
            }

            [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
            [ExpectedException(typeof(MacheteInvalidInputException))]
            public void CreateOnlineOrder_WA_empty_throws_error()
            {
                //
                // Arrange
                var e = frb.ToEmployer();
                var wo = frb.CloneWorkOrder();
                var lserv = frb.ToServLookup();

                wo.zipcode = "98118";
                wo.EmployerID = e.ID;
                var ll = lserv.Get(l => l.key == "transport_bus");
                wo.transportMethodID = ll.ID;
                wo.workAssignments = new List<WorkAssignment>();
                var serv = frb.ToServOnlineOrders();

                // 
                // Act
                var result = serv.Create(wo, "CreateOnlineOrder_WA_empty_throws_error");

            }

            [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
            [ExpectedException(typeof(MacheteInvalidInputException))]
            public void CreateOnlineOrder_wrong_cost_throws_error()
            {
                //
                // Arrange
                var e = frb.ToEmployer();
                var wo = frb.CloneWorkOrder();
                var lserv = frb.ToServLookup();

                wo.zipcode = "98118";
                wo.EmployerID = e.ID;
                var ll = lserv.Get(l => l.key == "transport_bus");
                wo.transportMethodID = ll.ID;
                wo.workAssignments = new List<WorkAssignment>();
                var wa = frb.CloneWorkAssignment();
                wa.transportCost = 0;
                wo.workAssignments.Add(wa);
                var serv = frb.ToServOnlineOrders();

                // 
                // Act
                var result = serv.Create(wo, "CreateOnlineOrder_wrong_cost_throws_error");
                //
                // Assert
            }

            [TestMethod, TestCategory(TC.IT), TestCategory(TC.Service), TestCategory(TC.OnlineOrders)]
            [ExpectedException(typeof(InvalidOperationException))]
            public void CreateOnlineOrder_unknown_zipcode_throws_error()
            {
                //
                // Arrange
                var e = frb.ToEmployer();
                var wo = frb.CloneWorkOrder();
                var lserv = frb.ToServLookup();

                wo.zipcode = "12345";
                wo.EmployerID = e.ID;
                var ll = lserv.Get(l => l.key == "transport_bus");
                wo.transportMethodID = ll.ID;
                wo.workAssignments = new List<WorkAssignment>();
                var wa = frb.CloneWorkAssignment();
                wa.transportCost = 5;
                wo.workAssignments.Add(wa);
                var serv = frb.ToServOnlineOrders();

                // 
                // Act
                var result = serv.Create(wo, "CreateOnlineOrder_unknown_zipcode_throws_error");
                //
                // Assert
            }
        }
    }
}