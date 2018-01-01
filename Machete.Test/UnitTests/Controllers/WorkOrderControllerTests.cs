﻿#region COPYRIGHT
// File:     WorkOrderControllerTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Test.Unit.Controller
{
    /// <summary>
    /// Summary description for WorkOrderControllerUnitTests
    /// </summary>

    [TestClass]
    public class WorkOrderTests
    {
        Mock<IWorkOrderService> serv;
        Mock<IEmployerService> empServ;
        Mock<IWorkAssignmentService> waServ;
        Mock<IWorkerService> reqServ;
        Mock<IWorkerRequestService> wrServ;
        Mock<IDefaults> def;
        Mock<IMapper> map;
        Mock<ILookupCache> lcache;
        Mock<IDatabaseFactory> dbfactory;
        FormCollection fakeform;
        List<WorkerRequest> workerRequest;
        WorkOrderController _ctrlr;
        int testid = 4242;
        //
        [TestInitialize]
        public void TestInitialize()
        {
            fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("workSiteAddress1", "blah");     //Every required field must be populated,
            fakeform.Add("city", "UnitTest");  //or result will be null.
            fakeform.Add("state", "WA");
            fakeform.Add("phone", "123-456-7890");
            fakeform.Add("zipcode", "12345-6789");
            fakeform.Add("typeOfWorkID", "1");
            fakeform.Add("dateTimeofWork", "1/1/2011");
            fakeform.Add("transportMethodID", "1");
            fakeform.Add("transportFee", "20.00");
            fakeform.Add("transportFeeExtra", "8.00");
            fakeform.Add("status", "43"); // active work order
            fakeform.Add("contactName", "test script contact name");
            //fakeform.Add("workerRequests", "30123,301234,30122,12345");
            serv = new Mock<IWorkOrderService>();
            empServ = new Mock<IEmployerService>();
            waServ = new Mock<IWorkAssignmentService>();
            reqServ = new Mock<IWorkerService>();
            wrServ = new Mock<IWorkerRequestService>();
            def = new Mock<IDefaults>();
            map = new Mock<IMapper>();
            workerRequest = new List<WorkerRequest> { };
            lcache = new Mock<ILookupCache>();
            dbfactory = new Mock<IDatabaseFactory>();
            _ctrlr = new WorkOrderController(serv.Object, waServ.Object, empServ.Object, reqServ.Object, wrServ.Object, lcache.Object, def.Object, map.Object);
            _ctrlr.SetFakeControllerContext();
            // TODO: Include Lookups in Dependency Injection, remove initialize statements
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void index_get_returns_enumerable_list()
        {
            var result = (ViewResult)_ctrlr.Index();
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void create_get_returns_workOrder()
        {
            // Arrange
            var vmwo = new Machete.Web.ViewModel.WorkOrder();
            map.Setup(x => x.Map<Domain.WorkOrder, Machete.Web.ViewModel.WorkOrder>(It.IsAny<Domain.WorkOrder>()))
                .Returns(vmwo);
            // Act
            var result = (PartialViewResult)_ctrlr.Create(0);
            // ASsert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkOrder));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void create_valid_post_returns_JSON()
        {
            //Arrange
            var workOrder = new WorkOrder();
            var _model = new WorkOrder();
            var vmwo = new Machete.Web.ViewModel.WorkOrder();
            map.Setup(x => x.Map<Domain.WorkOrder, Machete.Web.ViewModel.WorkOrder>(It.IsAny<Domain.WorkOrder>()))
                .Returns(vmwo);
            serv.Setup(p => p.Create(workOrder, "UnitTest")).Returns(() => workOrder);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = (JsonResult)_ctrlr.Create(workOrder, "UnitTest", workerRequest);
            //Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            //Assert.AreEqual(result.Data.ToString(), "{ sNewRef = /WorkOrder/Edit/4242, sNewLabel = Order #: 04242 @ blah, iNewID = 4242 }");
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void create_post_invalid_throws_exception()
        {
            //Arrange
            var workOrder = new WorkOrder();
            serv.Setup(p => p.Create(workOrder, "UnitTest")).Returns(workOrder);
            fakeform.Remove("contactName");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            _ctrlr.Create(workOrder, "UnitTest", workerRequest);
            //Assert
        }
        #endregion
        //
        //   Testing /Edit functionality
        //
        #region edittests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void edit_get_returns_workOrder()
        {

            //Arrange
            var vmwo = new Machete.Web.ViewModel.WorkOrder();
            map.Setup(x => x.Map<Domain.WorkOrder, Machete.Web.ViewModel.WorkOrder>(It.IsAny<Domain.WorkOrder>()))
                .Returns(vmwo);
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            fakeworkOrder.workerRequests = workerRequest;
            serv.Setup(p => p.Get(testid)).Returns(fakeworkOrder);
            //Act
            var result = (PartialViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(WorkOrder));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void edit_post_valid_updates_model_redirects_to_index()
        {
            //Arrange
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            fakeworkOrder.workerRequests = workerRequest;
            WorkOrder savedworkOrder = new WorkOrder();
            string user = "";
            serv.Setup(p => p.Get(testid)).Returns(fakeworkOrder);
            serv.Setup(x => x.Save(It.IsAny<WorkOrder>(),
                                          It.IsAny<string>())
                                         ).Callback((WorkOrder p, string str) =>
                                         {
                                             savedworkOrder = p;
                                             user = str;
                                         });
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            List<WorkerRequest> list = new List<WorkerRequest>();
            list.Add(new WorkerRequest { WorkerID = 12345 });
            list.Add(new WorkerRequest { WorkerID = 30002 });
            list.Add(new WorkerRequest { WorkerID = 30311 });
            list.Add(new WorkerRequest { WorkerID = 30420 });
            list.Add(new WorkerRequest { WorkerID = 30421 });
            var result = _ctrlr.Edit(testid, fakeform, "UnitTest", list) as JsonResult;
            //Assert
            //Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworkOrder, savedworkOrder);
            Assert.AreEqual(savedworkOrder.workSiteAddress1, "blah");
            Assert.AreEqual(savedworkOrder.city, "UnitTest");
            Assert.AreEqual(savedworkOrder.state, "WA");
            Assert.AreEqual(savedworkOrder.phone, "123-456-7890");
            Assert.AreEqual(savedworkOrder.zipcode, "12345-6789");
            Assert.AreEqual(savedworkOrder.typeOfWorkID, 1);
            Assert.AreEqual(savedworkOrder.dateTimeofWork, Convert.ToDateTime("1/1/2011 12:00:00 AM"));
            Assert.AreEqual(savedworkOrder.transportMethodID, 1);
            Assert.AreEqual(savedworkOrder.transportFee, Convert.ToDouble("20.00"));
            //Assert.AreEqual(savedworkOrder.workerRequests.Count(), 5);

        }
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void edit_post_workerRequests_finds_duplicates()
        {
            //Arrange
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            fakeworkOrder.workerRequests = workerRequest;
            WorkerRequest foo1 = new WorkerRequest
            {
                ID = 111,
                WorkerID = 1,
                WorkOrderID = testid,
                workerRequested = new Worker { ID = 1, dwccardnum = 12345 }

            };
            
            WorkerRequest foo2 = new WorkerRequest 
            {
                ID = 222,
                WorkerID = 2,
                WorkOrderID = testid,
                workerRequested = new Worker { ID = 2, dwccardnum = 12346 } 
            };
            workerRequest.Add(foo1);
            workerRequest.Add(foo2);
            WorkOrder savedworkOrder = new WorkOrder();
            string user = "";
            serv.Setup(p => p.Get(testid)).Returns(fakeworkOrder);
            serv.Setup(x => x.Save(It.IsAny<WorkOrder>(),
                                          It.IsAny<string>())
                                         ).Callback((WorkOrder p, string str) =>
                                         {
                                             savedworkOrder = p;
                                             user = str;
                                         });
            wrServ.Setup(x => x.GetByWorkerID(testid, 1)).Returns(foo1);
            wrServ.Setup(x => x.GetByWorkerID(testid, 2)).Returns(foo2);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            List<WorkerRequest> list = new List<WorkerRequest>();
            list.Add(new WorkerRequest { WorkerID = 12345 });
            list.Add(new WorkerRequest { WorkerID = 30002 });
            list.Add(new WorkerRequest { WorkerID = 30311 });
            list.Add(new WorkerRequest { WorkerID = 30420 });
            list.Add(new WorkerRequest { WorkerID = 30421 });
            //Act

            var result = _ctrlr.Edit(testid, fakeform, "UnitTest", list) as JsonResult;
            //Assert
            //Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(fakeworkOrder, savedworkOrder);

            Assert.AreEqual(savedworkOrder.workerRequests.Count(), 5);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 12345), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30002), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30311), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30420), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 30421), 1);
            Assert.AreEqual(savedworkOrder.workerRequests.Count(a => a.WorkerID == 12346), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void edit_post_invalid_throws_exception()
        {
            //Arrange
            var workOrder = new WorkOrder();
            workOrder.workerRequests = workerRequest;
            int testid = 4243;
            //
            // Mock service and setup SaveWorkOrder mock
            serv.Setup(p => p.Get(testid)).Returns(workOrder);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            fakeform.Remove("contactName");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            List<WorkerRequest> list = new List<WorkerRequest>();
            _ctrlr.Edit(testid, fakeform, "UnitTest", list);
            //Assert

        }
        #endregion
        #region delete tests
        /// <summary>
        /// delete GET returns workOrder
        /// </summary>
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void delete_get_returns_JSON()
        {
            //Arrange
            int testid = 4242;
            WorkOrder fakeworkOrder = new WorkOrder();
            serv.Setup(p => p.Get(testid)).Returns(fakeworkOrder);
            //Act
            JsonResult result = (JsonResult)_ctrlr.Delete(testid, "test user");
            //Assert
            IDictionary<string,object> data = new RouteValueDictionary(result.Data);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(4242, data["deletedID"]);
            
        }
        /// <summary>
        /// delete POST redirects to index
        /// </summary>
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.WorkOrders)]
        public void delete_post_returns_json()
        {
            //Arrange
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
             _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Delete(testid, "UnitTest") as JsonResult;
            //Assert
            Assert.AreEqual(result.Data.ToString(), "{ status = OK, deletedID = 4242 }");
        }
        #endregion
    }
}
