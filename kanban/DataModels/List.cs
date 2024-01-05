namespace kanban.DataModels
{
    public partial class List
    {
        public List()
        {
            KanbanTasks = new HashSet<KanbanTask>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<KanbanTask> KanbanTasks { get; set; }
    }
}
