namespace OzonEdu.MerchandiseService.HttpModels.DataTransferObjects
{
    public class RequestMerchRequestDto
    {
        public string EmployeeEmail { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        public int ClothingSize { get; set; }
        public int MerchType { get; set; }
    }
}
