namespace FFBDraftAPI.Models
{
    public class FFBTeams
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public string ThirdPartyID { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }    

        public FFBTeams()
        {
            Name = string.Empty;
            Manager = string.Empty;
            ThirdPartyID = string.Empty;
            Email = string.Empty;
            Nickname = string.Empty;
        }
    }
}
