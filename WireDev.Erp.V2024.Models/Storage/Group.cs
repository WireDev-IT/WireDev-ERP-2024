using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Group
    {
        public Group()
        {

        }

        public Group(int groupNumber)
        {
            Uuid = groupNumber;
        }

        [Key]
        public int Uuid { get; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Group needs a name!")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public bool Used { get; private set; } = false;
        public bool Use()
        {
            return Used = true;
        }

        public Task ModifyProperties(Group group)
        {
            string[] propertyNames = { "Uuid" };
            PropertyInfo[] properties = typeof(Group).GetProperties();
            foreach (PropertyInfo sPI in properties)
            {
                if (!propertyNames.Contains(sPI.Name))
                {
                    PropertyInfo? tPI = GetType().GetProperty(sPI.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (tPI != null && tPI.CanWrite && tPI.PropertyType.IsAssignableFrom(sPI.PropertyType))
                    {
                        tPI.SetValue(this, sPI.GetValue(group, null), null);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}