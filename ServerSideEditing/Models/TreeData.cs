using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideEditing.Models
{
    public class TreeData
    {
        [Key]
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Progress { get; set; }
        public String Priority { get; set; }
        public int Duration { get; set; }
        public int? ParentID { get; set; }
        public bool? isParent { get; set; }

        public static List<TreeData> Tree = new List<TreeData>();

        public static List<TreeData> GetTree()
        {
            if (Tree.Count == 0)
            {
                const int PARENT = 1;
                const int CHILD = 10;
                const int GRANDCHILD = 3;

                int root = -1;
                for (var t = 1; t <= PARENT; t++)
                {
                    Random ran = new Random();
                    string priority = (ran.Next() % 3) == 0 ? "High" : (ran.Next() % 2) == 0 ? "Release Breaker" : "Critical";
                    string progress = (ran.Next() % 3) == 0 ? "Started" : (ran.Next() % 2) == 0 ? "Open" : "In Progress";
                    root++;
                    int rootItem = Tree.Count + root + 1;
                    Tree.Add(new TreeData() { TaskID = rootItem, TaskName = "Parent Task " + rootItem.ToString(), StartDate = new DateTime(1992, 06, 07), EndDate = new DateTime(1994, 08, 25), isParent = true, ParentID = null, Progress = progress, Priority = priority, Duration = ran.Next(1, 50) });
                    int parent = Tree.Count;
                    for (var c = 0; c < CHILD; c++)
                    {
                        root++;
                        string val = ((parent + c + 1) % 3 == 0) ? "Low" : "Critical";
                        progress = (ran.Next() % 3) == 0 ? "In Progress" : (ran.Next() % 2) == 0 ? "Open" : "Validated";
                        int iD = Tree.Count + root + 1;
                        Tree.Add(new TreeData() { TaskID = iD, TaskName = "Child Task " + iD.ToString(), StartDate = new DateTime(1992, 06, 07), EndDate = new DateTime(1994, 08, 25), isParent = (((parent + c + 1) % 3) == 0), ParentID = rootItem, Progress = progress, Priority = val, Duration = ran.Next(1, 50) });
                        if ((((parent + c + 1) % 3) == 0))
                        {
                            int immParent = Tree.Count;
                            for (var s = 0; s < GRANDCHILD; s++)
                            {
                                root++;
                                string Prior = (immParent % 2 == 0) ? "Validated" : "Normal";
                                Tree.Add(new TreeData() { TaskID = Tree.Count + root + 1, TaskName = "Sub Task " + (Tree.Count + root + 1).ToString(), StartDate = new DateTime(1992, 06, 07), EndDate = new DateTime(1994, 08, 25), isParent = false, ParentID = iD, Progress = (immParent % 2 == 0) ? "On Progress" : "Closed", Priority = Prior, Duration = ran.Next(1, 50) });
                            }
                        }
                    }
                }
            }
            return Tree;
        }
    }
}
