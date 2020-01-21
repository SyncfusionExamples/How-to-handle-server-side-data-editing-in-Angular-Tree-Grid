using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using ServerSideEditing.Models;

namespace ServerSideEditing.Controllers
{
    [Route("api/Tasks")]
    public class TasksController : Controller
    {
        [HttpPost]
        [Route("DataSource")]
        public object DataSource([FromBody] DataManagerRequest dm)
        {

            IEnumerable treeData = TreeData.GetTree();
            DataOperations operation = new DataOperations();
            if (dm.Where != null && dm.Where.Count > 0)
            {
                treeData = operation.PerformFiltering(treeData, dm.Where, "and");
            }

            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                treeData = operation.PerformSorting(treeData, dm.Sorted);
            }

            var count = TreeData.Tree.Count();
            if (dm.Skip != 0)
            {
                treeData = operation.PerformSkip(treeData, dm.Skip);
            }

            if (dm.Take != 0)
            {
                treeData = operation.PerformTake(treeData, dm.Take);
            }

            return dm.RequiresCounts ? Json(new { result = treeData, count = count }) : Json(treeData);

        }

        [HttpPost]
        [Route("Insert")]
        public ActionResult Insert([FromBody] CRUDModel cRUDModel)
        {
            if (cRUDModel == null) return null;
            if (cRUDModel.RelationalKey == -1) return null;

            var parentIndex = 0;
            for (; parentIndex < TreeData.Tree.Count; parentIndex++)
            {
                if (TreeData.Tree[parentIndex].TaskID == cRUDModel.RelationalKey)
                {
                    break;
                }
            }

            var childRecordsCount = FindChildRecords(cRUDModel.RelationalKey);
            var insertAt = parentIndex + childRecordsCount + 1;
            TreeData.Tree.Insert(insertAt, cRUDModel.Value);

            return Json(cRUDModel.Value);
        }

        //To find the child records
        public int FindChildRecords(int? id)
        {
            var count = 0;
            foreach (var t in TreeData.Tree)
            {
                if (t.ParentID == id)
                {
                    count++;
                    count += FindChildRecords(t.TaskID);
                }
            }
            return count;
        }

        [HttpPost]
        [Route("Update")]
        public ActionResult Update([FromBody] CRUDModel cRUDModel)
        {
            var task = TreeData.Tree.FirstOrDefault(td => td.TaskID == cRUDModel.Value.TaskID);
            if (task != null)
            {
                task.TaskName = cRUDModel.Value.TaskName;
                task.StartDate = cRUDModel.Value.StartDate;
                task.Duration = cRUDModel.Value.Duration;
                task.Priority = cRUDModel.Value.Priority;
                task.Progress = cRUDModel.Value.Progress;
            }
            return Json(cRUDModel);
        }

        [HttpPost]
        [Route("Remove")]
        public object Remove([FromBody] CRUDModel cRUDModel)
        {
            TreeData.Tree.Remove(TreeData.Tree.FirstOrDefault(task => task.TaskID.Equals(cRUDModel.Key)));
            return Json(cRUDModel);
        }

        [HttpPost]
        [Route("BatchDelete")]
        public object BatchDelete([FromBody] CRUDModel cRUDModel)
        {
            if (cRUDModel.Deleted != null)
            {
                foreach (var deletedTask in cRUDModel.Deleted)
                {
                    TreeData.Tree.Remove(TreeData.Tree.FirstOrDefault(task => task.TaskID == deletedTask.TaskID));
                }
            }
            return new { deleted = cRUDModel.Deleted };
        }

        public class CRUDModel
        {
            public TreeData Value { get; set; }
            public int Key { get; set; }
            public int RelationalKey { get; set; }
            public List<TreeData> Deleted { get; set; }
        }
    }
}