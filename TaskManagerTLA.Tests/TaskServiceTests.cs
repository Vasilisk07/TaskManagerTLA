//using AutoMapper;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TaskManagerTLA.BLL.DTO;
//using TaskManagerTLA.BLL.Mapper;
//using TaskManagerTLA.BLL.Services;
//using TaskManagerTLA.DAL.Entities;
//using TaskManagerTLA.DAL.Interfaces;

//namespace TaskManagerTLATest
//{
//    public class TaskServiceTests
//    {
//        List<AssignedTask> TestListActualTasks;
//        List<GlobalTask> TestListTasks;
//        Mock<IUnitOfWork> mockUnitOfWork;
//        IMapper mapper;

//        [SetUp]
//        public void Setup()
//        {
//            TestListActualTasks = GetListActualTasks();
//            TestListTasks = GetListTasks();
//            mockUnitOfWork = new Mock<IUnitOfWork>();
//            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
//            mapper = mapperConfig.CreateMapper();

//        }

//        [Test]
//        public void GetActualTasks_Returned_List_5_Count()
//        {
//            //Arrange
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.GetAll()).Returns(TestListActualTasks);
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            var resultList = service.GetAssignedTasks().ToList();

//            //Assert
//            Assert.AreEqual(resultList.Count, TestListActualTasks.Count, "Inner count collection {0} does not match the input count collection {1}", resultList.Count, TestListActualTasks.Count);

//        }


//        // TODO що за inner елемент?
//        [Test]
//        public void GetActualTask_Inner_TestId_OutTestId()
//        {
//            //Arrange
//            int testId = 1;
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.Get(testId)).Returns(TestListActualTasks.First());
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            var resultList = service.GetAssignedTask(testId);

//            //Assert
//            Assert.AreEqual(resultList.Id, testId, "Inner Element id {0} not equal return element id {1}", resultList.Id, testId);

//        }


//        [Test]
//        public void MakeActualTask_Inner_ActualTask_WriteDB()
//        {
//            //Arrange
//           // AssignedTaskDTO InnerActualTaskDTO = new AssignedTaskDTO { Id = 6, TaskName = "Read book", UserID = "Jack", TaskId = 7, SpentHours = 10, Description = "Some Task Work" };
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.GetAll()).Returns(TestListActualTasks);
//            mockUnitOfWork.Setup(repo => repo.Save());
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//           // service.AddAssignedTask(InnerActualTaskDTO);

//            //Assert
//            mockUnitOfWork.Verify(r => r.AssignedTasks.Create(It.IsAny<AssignedTask>()), Times.Once(), "Object was not added database");
//            mockUnitOfWork.Verify(r => r.Save(), "Changes not save");
//        }

//        [Test]
//        public void MakeActualTask_Inner_ActualTask_Exist()
//        {
//            //Arrange
//            AssignedTaskDTO ExistActualTaskDTO = new AssignedTaskDTO
//            {
//                Id = TestListActualTasks.First().GlobalTaskId,
//              //  TaskName = TestListActualTasks.First().Name,
//                //UserID = TestListActualTasks.First().UserId,
//                GlobalTaskId = TestListActualTasks.First().GlobalTaskId,
//                SpentHours = TestListActualTasks.First().SpentHours,
//                Description = TestListActualTasks.First().Description
//            };
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.GetAll()).Returns(TestListActualTasks);
//            mockUnitOfWork.Setup(repo => repo.Save());
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            service.AddAssignedTask(ExistActualTaskDTO);

//            //Assert
//            mockUnitOfWork.Verify(r => r.AssignedTasks.Create(It.IsAny<AssignedTask>()), Times.Never(), "Rewrite exist object");
//            // TODO помилки в Verify повинні краще описати саме причину поломки тесту
//            // тобто тут можна написати (якщо треба) "`Create` should not be called` чи щось такеі

//        }


//        [Test]
//        public void DeleteActualTask_Inner_TestId()
//        {
//            //Arrange
//            int testId = 1;
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.Delete(testId));
//            mockUnitOfWork.Setup(repo => repo.Save());
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            service.DeleteAssignedTask(testId);

//            //Assert
//            mockUnitOfWork.Verify(r => r.AssignedTasks.Delete(testId), Times.Once(), "Object not deleted");
//            mockUnitOfWork.Verify(r => r.Save(), "Changes not save");
//        }

//        [Test]
//        [TestCase(1, 5, "Some Description")]
//        [TestCase(1, 18, "S")]
//        [TestCase(1, 0, "")]
//        public void EditActualTask_Inner_TestId(int testId, int testTime, string testDescriptions)
//        {
//            //Arrange
//            AssignedTask returnedActualTask = TestListActualTasks[0];
//            GlobalTask returnedTask = TestListTasks[0];
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.Get(testId)).Returns(returnedActualTask);
//            mockUnitOfWork.Setup(repo => repo.GlobalTasks.Get(returnedActualTask.GlobalTaskId)).Returns(returnedTask);
//            var service = new TaskService(mockUnitOfWork.Object, mapper);
//            //act
//           // service.EditActualTask(testId, testTime, testDescriptions);
//            //Assert
//            Assert.IsTrue(returnedActualTask.Description.Contains(testDescriptions), "ActualTask Description is not correct modify");
//            Assert.AreEqual(returnedActualTask.SpentHours, testTime, "ActTaskLeigth is not correct modify");
//            Assert.AreEqual(returnedTask.TotalSpentHours, testTime, "TaskLeigth is not correct modify");
//        }
//        [Test]
//        public void EditActualTask_Inner_null()
//        {
//            //Arrange
//            int testId = 1;
//            AssignedTask returnedActualTask = TestListActualTasks[0];
//            int firstValueleighActualTask = returnedActualTask.SpentHours;
//            string firstValueDescActualTask = returnedActualTask.Description;
//            GlobalTask returnedTask = TestListTasks[0];
//            int firstValueleighTask = returnedTask.TotalSpentHours;
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.Get(testId)).Returns(returnedActualTask);
//            mockUnitOfWork.Setup(repo => repo.GlobalTasks.Get(returnedActualTask.GlobalTaskId)).Returns(returnedTask);
//            var service = new TaskService(mockUnitOfWork.Object, mapper);
//            //act
//           // service.EditActualTask(testId, null, null);
//            //Assert
//            Assert.IsTrue(returnedActualTask.Description == firstValueDescActualTask, "ActualTask Description is not correct modify for inner null");
//            Assert.AreEqual(returnedActualTask.SpentHours, firstValueleighActualTask, "ActTaskLeigth is not correct modify for inner null");
//            Assert.AreEqual(returnedTask.TotalSpentHours, firstValueleighTask, "TaskLeigth is not correct modify for inner null");
//        }

//        // TODO в окремий тест файл
//        //TaskTest//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//        [Test]
//        public void GetTasks_Returned_List_5_Count()
//        {
//            //Arrange
//            mockUnitOfWork.Setup(repo => repo.GlobalTasks.GetAll()).Returns(TestListTasks);
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            var resultList = service.GetGlobalTasks().ToList();

//            //Assert
//            Assert.AreEqual(resultList.Count, TestListTasks.Count, "Inner count collection {0} does not match the input count collection {1}", resultList.Count, TestListTasks.Count);

//        }


//        [Test]
//        public void GetTask_Inner_TestId_OutTestId()
//        {
//            //Arrange
//            int testId = 1;
//            mockUnitOfWork.Setup(repo => repo.GlobalTasks.Get(testId)).Returns(TestListTasks.First());
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            var resultList = service.GetGlobalTask(testId);

//            //Assert
//            Assert.AreEqual(resultList.Id, testId, "Inner Element id {0} not equal return element id {1}", resultList.Id, testId);
//        }

//        [Test]
//        public void MakeTask_Inner_Task_WriteDB()
//        {
//            //Arrange
//            GlobalTaskDTO InnerTaskDTO = new GlobalTaskDTO { Id = 6, Name = "Rewrite program", Begin = new DateTime(2021, 04, 26), End = new DateTime(2021, 11, 21), TotalSpentHours = 0, Description = "Rewrite some program" };
//            mockUnitOfWork.Setup(repo => repo.GlobalTasks.GetAll()).Returns(TestListTasks);
//            mockUnitOfWork.Setup(repo => repo.Save());
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            service.AddGlobalTask(InnerTaskDTO);

//            //Assert
//            mockUnitOfWork.Verify(r => r.GlobalTasks.Create(It.IsAny<GlobalTask>()), Times.Once(), "Object was not added database");
//            mockUnitOfWork.Verify(r => r.Save(), "Changes not save");
//        }

//        [Test]
//        public void DeleteTask_Inner_TestId()
//        {
//            //Arrange
//            int testId = 1;
//            mockUnitOfWork.Setup(repo => repo.GlobalTasks.Delete(testId));
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.GetAll()).Returns(TestListActualTasks);
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            service.DeleteGlobalTask(testId);

//            //Assert
//            mockUnitOfWork.Verify(r => r.GlobalTasks.Delete(testId), Times.Once(), "Object not deleted");
//            mockUnitOfWork.Verify(r => r.AssignedTasks.GetAll(), Times.Once(), "Supporting object not found");
//            mockUnitOfWork.Verify(r => r.AssignedTasks.Delete(testId), Times.Once(), "Supporting object not deleted");
//            mockUnitOfWork.Verify(r => r.Save(), "Changes not save");
//        }

//        [Test]
//        public void DeleteActualTaskByUser_InnerUserName()
//        {
//            //Arrange
//            string userName = "Frank";
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.GetAll()).Returns(TestListActualTasks);
//            var service = new TaskService(mockUnitOfWork.Object, mapper);

//            //act
//            service.DeleteAssignedTaskByUser(userName);

//            //Assert
//            mockUnitOfWork.Verify(r => r.AssignedTasks.GetAll(), Times.Once(), "Supporting object not found");
//            mockUnitOfWork.Verify(r => r.AssignedTasks.Delete(It.IsAny<int>()), Times.Exactly(2), "Object not deleted");
//            mockUnitOfWork.Verify(r => r.Save(), "Changes not save");
//        }

//        [Test]
//        public void GetDetailsTask_InnerTaskId()
//        {
//            //Arrange
//            int taskId = 3;
//            int countResultList = 1;
//            mockUnitOfWork.Setup(repo => repo.AssignedTasks.GetAll()).Returns(TestListActualTasks);
//            var service = new TaskService(mockUnitOfWork.Object, mapper);
//            //act
//            var resultList = service.GetAssignedTasksByGlobalTaskId(taskId);
//            //Assert
//            Assert.IsTrue(resultList.ToList().Count == countResultList, "Count List is {0} expected value is {1} ", resultList.ToList().Count, countResultList);
//            Assert.IsTrue(resultList.First().GlobalTaskId == taskId, "Item id {0} is not equal expected {1}", resultList.First().GlobalTaskId, taskId);
//        }

//        // TODO в окремий хелпер клас
//        //Helper methods////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//        private List<GlobalTask> GetListTasks()
//        {
//            List<GlobalTask> retList = new List<GlobalTask>();
//            retList.Add(new GlobalTask { Id = 1, Name = "Make program", Begin = new DateTime(2021, 08, 05), End = new DateTime(2021, 09, 01), TotalSpentHours = 0, Description = "Very funny task" });
//            retList.Add(new GlobalTask { Id = 2, Name = "Make testing", Begin = new DateTime(2021, 08, 05), End = new DateTime(2021, 09, 01), TotalSpentHours = 56, Description = "Very funny task" });
//            retList.Add(new GlobalTask { Id = 3, Name = "Make module", Begin = new DateTime(2021, 08, 05), End = new DateTime(2021, 09, 01), TotalSpentHours = 56, Description = "Very funny task" });
//            retList.Add(new GlobalTask { Id = 4, Name = "Make controller", Begin = new DateTime(2021, 08, 05), End = new DateTime(2021, 09, 01), TotalSpentHours = 56, Description = "Very funny task" });
//            retList.Add(new GlobalTask { Id = 5, Name = "Make interface", Begin = new DateTime(2021, 08, 05), End = new DateTime(2021, 09, 01), TotalSpentHours = 56, Description = "Very funny task" });
//            return retList;
//        }

//        private List<AssignedTask> GetListActualTasks()
//        {
//            List<AssignedTask> retList = new List<AssignedTask>();
//            //retList.Add(new AssignedTask { Id = 1, Name = "Make interface", UserId = "Bob", TaskId = 1, SpentHours = 0, Description = "Work some task" });
//            //retList.Add(new AssignedTask { Id = 2, Name = "Make module", UserId = "Bob", TaskId = 3, SpentHours = 10, Description = "Work some task" });
//            //retList.Add(new AssignedTask { Id = 3, Name = "Make program", UserId = "Bob", TaskId = 5, SpentHours = 10, Description = "Work some task" });
//            //retList.Add(new AssignedTask { Id = 4, Name = "Make controller", UserId = "Frank", TaskId = 4, SpentHours = 10, Description = "Work some task" });
//            //retList.Add(new AssignedTask { Id = 5, Name = "Make testing", UserId = "Bob", TaskId = 2, SpentHours = 10, Description = "Work some task" });
//            //retList.Add(new AssignedTask { Id = 6, Name = "Make testing", UserId = "Frank", TaskId = 2, SpentHours = 10, Description = "Work some task" });
//            return retList;
//        }
//    }
//}