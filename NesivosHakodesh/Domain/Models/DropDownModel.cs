namespace NesivosHakodesh.Domain.Models
{
    public class DropDownModel
    {
        public DropDownModel(ushort id, string name)
        {
            Id = id;
            Name = name;
        }

        public ushort Id { get; set; }

        public string Name { get; set; }
    }
}
