namespace NewKaratIk.Dtos
{
    public class UserDetailsModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IEnumerable<string>? SelectedRoles { get; set; }
    }
}
