namespace OzonEdu.MerchandiseService.HttpModels.DataTransferObjects
{
    public class RequestMerchRequestDto
    {
        public string EmployeeEmail { get; set; }
        public string ManagerEmail { get; set; }
        public int MerchType { get; set; }
    }
}
