﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NetBpm.Workflow.Definition;
using NetBpm.Workflow.Execution;
using NetBpm.Workflow.Organisation;
using NUnit.Framework;

namespace MyTest
{
    [TestFixture]
    public class HelloWorld3Test : BaseTest
    {
        [Test]
        public void DeployTest()
        {
            FileInfo parFile = new FileInfo("ExamplePar/helloworld3.par");
            FileStream fstream = parFile.OpenRead();
            byte[] b = new byte[parFile.Length];
            fstream.Read(b, 0, (int)parFile.Length);
            processDefinitionService.DeployProcessArchive(b);

            /*
             select * from [dbo].[NBPM_PROCESSBLOCK];
             select * from [dbo].[NBPM_NODE];
             select * from [dbo].[NBPM_TRANSITION];
             select * from [dbo].[NBPM_ACTION]
             select * from [dbo].[NBPM_ATTRIBUTE];
             select * from [dbo].[NBPM_DELEGATION];
             */
        }

        [Test]
        public void StartTest()
        {
            IProcessInstance processInstance = null;
            Thread.CurrentPrincipal = new PrincipalUserAdapter("ae");

            try
            {
                IDictionary attributeValues = new Hashtable();

                IProcessDefinition booaction = processDefinitionService.GetProcessDefinition("Hello world 3");

                processInstance = executionComponent.StartProcessInstance(booaction.Id, attributeValues);
                //這時已經在First State
                Assert.IsNotNull(processInstance);
            }
            catch (ExecutionException e)
            {
                Assert.Fail("ExcecutionException while starting a new holiday request: " + e.Message);
            }
            finally
            {
                //      loginUtil.logout();
            }
        }

        [Test]
        public void TransitionFirstTest()
        {
            IProcessInstance processInstance = null;
            Thread.CurrentPrincipal = new PrincipalUserAdapter("ae");

            try
            {
                var taskLists = executionComponent.GetTaskList("ae");

                foreach (IFlow task in taskLists)
                {
                    //跑完會執行HelloWorldAction，再進入EndState
                    executionComponent.PerformActivity(task.Id);
                }
            }
            catch (ExecutionException e)
            {
                Assert.Fail("ExcecutionException while starting a new holiday request: " + e.Message);
            }
            finally
            {
                //      loginUtil.logout();
            }
        }
    }
}
