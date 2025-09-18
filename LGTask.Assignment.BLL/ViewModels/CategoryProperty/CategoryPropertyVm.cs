namespace LGTask.Assignment.BLL.ViewModels.CategoryProperty
{
    public class CategoryPropertyVm
    {
        public int Id { get; set; }
        public int? PropertyId { get; set; }
        public int? CategoryId { get; set; }

        public string? PropertyName { get; set; }
        public string? CategoryName { get; set; }


    }
}
