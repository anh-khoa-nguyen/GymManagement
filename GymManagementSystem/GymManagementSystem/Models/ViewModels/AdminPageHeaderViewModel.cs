namespace GymManagementSystem.Models.ViewModels
{
    public class AdminPageHeaderViewModel
    {
        public string Title { get; set; }
        public string CreateActionUrl { get; set; }
        public string SearchAction { get; set; }
        public string SearchController { get; set; }
        public string CurrentFilter { get; set; }
    }
}