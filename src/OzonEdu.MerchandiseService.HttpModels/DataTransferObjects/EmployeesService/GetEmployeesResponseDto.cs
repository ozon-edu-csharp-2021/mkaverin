using System;

namespace OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.EmployeesService
{
    //public class GetEmployeesResponseDto
    //{
    //    public ItemGetEmployeesResponseDto[] Items { get; set; }
    //    public int TotalCount { get; set; }
    //}

    //public class ItemGetEmployeesResponseDto
    //{
    //    public int Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string MiddleName { get; set; }
    //    public DateTime BirthDay { get; set; }
    //    public DateTime HiringDate { get; set; }
    //    public string Email { get; set; }
    //    public int ClothingSize { get; set; }
    //    public ConferenceGetEmployeesResponseDto[] Conferences { get; set; }
    //}

    //public class ConferenceGetEmployeesResponseDto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public DateTime Date { get; set; }
    //    public string Description { get; set; }
    //}

    public class GetEmployeesResponseDto
    {
        public ItemGetEmployeesResponseDto[] items { get; set; }
        public int totalCount { get; set; }
    }

    public class ItemGetEmployeesResponseDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public DateTime birthDay { get; set; }
        public DateTime hiringDate { get; set; }
        public string email { get; set; }
        public int clothingSize { get; set; }
        public ConferenceGetEmployeesResponseDto[] conferences { get; set; }
        public int id { get; set; }
    }
    public class ConferenceGetEmployeesResponseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
    }
}
