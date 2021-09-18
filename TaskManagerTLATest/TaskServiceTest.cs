using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLATest
{
    public class TaskServiceTest
    {
        List<ActualTask> TestListActualTasks;
        List<TaskModel> TestListTasks;
        Mock<IUnitOfWork> mock;

        [SetUp]
        public void Setup()
        {
            TestListActualTasks = GetListActualTasks();
            TestListTasks = GetListTasks();
            mock = new Mock<IUnitOfWork>();

        }

        //ActualTaskTest///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void GetActTasks_Returned_List_5_Count()
        {
            //Arrange
            mock.Setup(repo=>repo.ActualTasks.GetAll()).Returns(TestListActualTasks);
            var service = new TaskService(mock.Object);

            //act
            var resultList = service.GetActTasks().ToList();
           
            //Assert
            Assert.AreEqual(resultList.Count, TestListActualTasks.Count, "Inner count collection {0} does not match the input count collection {1}", resultList.Count, TestListActualTasks.Count);
            
        }



        [Test]
        public void GetActualTask_Inner_TestId_OutTestId()
        {
             //Arrange
            int testId = 1;
            mock.Setup(repo => repo.ActualTasks.Get(testId)).Returns(TestListActualTasks.First());
            var service = new TaskService(mock.Object);

            //act
            var resultList = service.GetActualTask(testId);
          
            //Assert
            Assert.AreEqual(resultList.ActualTaskId, testId, "Inner Element id {0} not equal return element id {1}", resultList.ActualTaskId, testId);

        }


        [Test]
        public void MakeActualTask_Inner_ActualTask_WriteDB()
        {
            //Arrange
            ActualTaskDTO InnerActualTaskDTO = new ActualTaskDTO { ActualTaskId = 6, TaskName = "Read book", UserName = "Jack", TaskId = 7, ActTaskLeigth = 10, Description = "Some Task Work" };
            mock.Setup(repo=>repo.ActualTasks.GetAll()).Returns(TestListActualTasks);
            mock.Setup(repo => repo.Save());
            var service = new TaskService(mock.Object);

            //act
            service.MakeActualTask(InnerActualTaskDTO);

            //Assert
            mock.Verify(r => r.ActualTasks.Create(It.IsAny< ActualTask>()),Times.Once(), "Object was not added database");
            mock.Verify(r => r.Save(), "Changes not save");
        }
        [Test]
        public void MakeActualTask_Inner_ActualTask_Exist()
        {
            //Arrange
            ActualTaskDTO ExistActualTaskDTO = new ActualTaskDTO 
            {   
                ActualTaskId = TestListActualTasks.First().TaskId, 
                TaskName = TestListActualTasks.First().TaskName,
                UserName = TestListActualTasks.First().UserName, 
                TaskId = TestListActualTasks.First().TaskId, 
                ActTaskLeigth = TestListActualTasks.First().ActTaskLeigth, 
                Description = TestListActualTasks.First().Description 
            };
            mock.Setup(repo=>repo.ActualTasks.GetAll()).Returns(TestListActualTasks);
            mock.Setup(repo => repo.Save());
            var service = new TaskService(mock.Object);

            //act
            service.MakeActualTask(ExistActualTaskDTO);

            //Assert
            mock.Verify(r => r.ActualTasks.Create(It.IsAny<ActualTask>()),Times.Never(), "Rewrite exist object");

        }


        [Test]
        public void DeleteActualTask_Inner_TestId()
        {
            //Arrange
            int testId = 1;
            mock.Setup(repo => repo.ActualTasks.Delete(testId));
            mock.Setup(repo => repo.Save());
            var service = new TaskService(mock.Object);

            //act
            service.DeleteActualTask(testId);

            //Assert
            mock.Verify(r => r.ActualTasks.Delete(testId),Times.Once(), "Object not deleted");
            mock.Verify(r => r.Save(), "Changes not save");
        }

        [Test]
        [TestCase(1, 5, "Some Description")]
        [TestCase(1, 18, "S")]
        [TestCase(1, 0, "")]
        public void EditActualTask_Inner_TestId(int testId,int testTime,string testDescriptions)
        {
            //Arrange
            ActualTask returnedActualTask = TestListActualTasks[0];
            TaskModel returnedTask = TestListTasks[0];
            mock.Setup(repo => repo.ActualTasks.Get(testId)).Returns(returnedActualTask);
            mock.Setup(repo => repo.Tasks.Get(returnedActualTask.TaskId)).Returns(returnedTask);
            var service = new TaskService(mock.Object);
            //act
            service.EditActualTask(testId, testTime, testDescriptions);
            //Assert
            Assert.IsTrue(returnedActualTask.Description.Contains(testDescriptions), "ActualTask Description is not correct modify");
            Assert.AreEqual(returnedActualTask.ActTaskLeigth, testTime, "ActTaskLeigth is not correct modify");
            Assert.AreEqual(returnedTask.TaskLeigth, testTime, "TaskLeigth is not correct modify");
        }
        [Test]
        public void EditActualTask_Inner_null()
        {
            //Arrange
            int testId = 1;
            ActualTask returnedActualTask = TestListActualTasks[0];
            int firstValueleighActualTask = returnedActualTask.ActTaskLeigth;
            string firstValueDescActualTask = returnedActualTask.Description;
            TaskModel returnedTask = TestListTasks[0];
            int firstValueleighTask = returnedTask.TaskLeigth;
            mock.Setup(repo => repo.ActualTasks.Get(testId)).Returns(returnedActualTask);
            mock.Setup(repo => repo.Tasks.Get(returnedActualTask.TaskId)).Returns(returnedTask);
            var service = new TaskService(mock.Object);
            //act
            service.EditActualTask(testId, null, null);
            //Assert
            Assert.IsTrue(returnedActualTask.Description == firstValueDescActualTask, "ActualTask Description is not correct modify for inner null");
            Assert.AreEqual(returnedActualTask.ActTaskLeigth, firstValueleighActualTask, "ActTaskLeigth is not correct modify for inner null");
            Assert.AreEqual(returnedTask.TaskLeigth, firstValueleighTask, "TaskLeigth is not correct modify for inner null");
        }

        //TaskTest//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void GetTasks_Returned_List_5_Count()
        {
            //Arrange
            mock.Setup(repo => repo.Tasks.GetAll()).Returns(TestListTasks);
            var service = new TaskService(mock.Object);

            //act
            var resultList = service.GetTasks().ToList();

            //Assert
            Assert.AreEqual(resultList.Count, TestListTasks.Count, "Inner count collection {0} does not match the input count collection {1}", resultList.Count, TestListTasks.Count);

        }


        [Test]
        public void GetTask_Inner_TestId_OutTestId()
        {
            //Arrange
            int testId = 1;
            mock.Setup(repo => repo.Tasks.Get(testId)).Returns(TestListTasks.First());
            var service = new TaskService(mock.Object);

            //act
            var resultList = service.GetTask(testId);

            //Assert
            Assert.AreEqual(resultList.TaskModelId, testId, "Inner Element id {0} not equal return element id {1}", resultList.TaskModelId, testId);
        }

        [Test]
        public void MakeTask_Inner_Task_WriteDB()
        {
            //Arrange
            TaskDTO InnerTaskDTO = new TaskDTO { TaskModelId = 6, TaskName = "Rewrite program", TaskBegin = new DateTime(2021, 04, 26), TaskEnd = new DateTime(2021, 11, 21), TaskLeigth = 0, TaskDescription="Rewrite some program"};
            mock.Setup(repo => repo.Tasks.GetAll()).Returns(TestListTasks);
            mock.Setup(repo => repo.Save());
            var service = new TaskService(mock.Object);

            //act
            service.MakeTask(InnerTaskDTO);

            //Assert
            mock.Verify(r => r.Tasks.Create(It.IsAny<TaskModel>()), Times.Once(), "Object was not added database");
            mock.Verify(r => r.Save(), "Changes not save");
        }

        [Test]
        public void DeleteTask_Inner_TestId()
        {
            //Arrange
            int testId = 1;
            mock.Setup(repo => repo.Tasks.Delete(testId));
            mock.Setup(repo => repo.ActualTasks.GetAll()).Returns(TestListActualTasks);
            var service = new TaskService(mock.Object);

            //act
            service.DeleteTask(testId);

            //Assert
            mock.Verify(r => r.Tasks.Delete(testId), Times.Once(), "Object not deleted");
            mock.Verify(r => r.ActualTasks.GetAll(), Times.Once(), "Supporting object not found");
            mock.Verify(r=>r.ActualTasks.Delete(testId),Times.Once(), "Supporting object not deleted");
            mock.Verify(r => r.Save(), "Changes not save");
        }



        //Helper methods////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private List<TaskModel> GetListTasks()
        {
            List<TaskModel> retList = new List<TaskModel>();
            retList.Add(new TaskModel { TaskModelId = 1, TaskName = "Make program", TaskBegin = new DateTime(2021, 08, 05), TaskEnd = new DateTime(2021, 09, 01), TaskLeigth = 0, TaskDescription = "Very funny task" });
            retList.Add(new TaskModel { TaskModelId = 2, TaskName = "Make testing", TaskBegin = new DateTime(2021, 08, 05), TaskEnd = new DateTime(2021, 09, 01), TaskLeigth = 56, TaskDescription = "Very funny task" });
            retList.Add(new TaskModel { TaskModelId = 3, TaskName = "Make module", TaskBegin = new DateTime(2021, 08, 05), TaskEnd = new DateTime(2021, 09, 01), TaskLeigth = 56, TaskDescription = "Very funny task" });
            retList.Add(new TaskModel { TaskModelId = 4, TaskName = "Make controller", TaskBegin = new DateTime(2021, 08, 05), TaskEnd = new DateTime(2021, 09, 01), TaskLeigth = 56, TaskDescription = "Very funny task" });
            retList.Add(new TaskModel { TaskModelId = 5, TaskName = "Make interface", TaskBegin = new DateTime(2021, 08, 05), TaskEnd = new DateTime(2021, 09, 01), TaskLeigth = 56, TaskDescription = "Very funny task" });
            return retList;
        }

        private  List<ActualTask> GetListActualTasks()
        {
            List<ActualTask> retList = new List<ActualTask>();
            retList.Add(new ActualTask { ActualTaskId = 1, TaskName = "Make interface", UserName = "Bob", TaskId = 1, ActTaskLeigth = 0, Description = "Work some task" });
            retList.Add(new ActualTask { ActualTaskId = 2, TaskName = "Make module", UserName = "Bob", TaskId = 3, ActTaskLeigth = 10, Description = "Work some task" });
            retList.Add(new ActualTask { ActualTaskId = 3, TaskName = "Make program", UserName = "Bob", TaskId = 5, ActTaskLeigth = 10, Description = "Work some task" });
            retList.Add(new ActualTask { ActualTaskId = 4, TaskName = "Make controller", UserName = "Bob", TaskId = 4, ActTaskLeigth = 10, Description = "Work some task" });
            retList.Add(new ActualTask { ActualTaskId = 5, TaskName = "Make testing", UserName = "Bob", TaskId = 2, ActTaskLeigth = 10, Description = "Work some task" });
            retList.Add(new ActualTask { ActualTaskId = 6, TaskName = "Make testing", UserName = "Bob", TaskId = 2, ActTaskLeigth = 10, Description = "Work some task" });
            return retList;
        }
    }
}