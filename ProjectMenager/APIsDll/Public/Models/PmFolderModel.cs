using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIsDll.Public.Models
{
    public class PmFolderModel
    {
        public PmFolderModel():
            this(string.Empty, string.Empty, new List<string>())
        {

        }
        public PmFolderModel(string title, string description, List<string> tags)
        {
            this.Title = title;
            this.Description = description;
            this.Tags = tags;

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolderPath = System.IO.Path.Combine(appDataPath, "Project Menager");
            string defaultReposPath = System.IO.Path.Combine(appFolderPath, "Repos");

            if (!Directory.Exists(defaultReposPath))
                Directory.CreateDirectory(defaultReposPath);

            this.Path = System.IO.Path.Combine(defaultReposPath, Title == string.Empty ? Id : Title);
        }


        private static string GenereateID { get => Guid.NewGuid().ToString().Substring(0, 10).Replace(':', '_'); }


        public string Id { get; set; } = GenereateID;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null) return false;
            if (obj is PmFolderModel model)
            {
                if (model.Id == this.Id) return true;
                return false;

                //    if (Title != model.Title) return false;
                //    if (Description != model.Description) return false;
                //    if (Path != model.Path) return false;
                //    if (Tags.Count != model.Tags.Count) return false;
                //    for (int i = 0; i < Tags.Count; i++)
                //    {
                //        if (Tags[i] != model.Tags[i]) return false;
                //    }

                //    return true;
                //}
                //else return false;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Title: {Title}, ");
            sb.Append($"Path {Path}");

            return sb.ToString();
        }

        public static bool operator ==(PmFolderModel left, PmFolderModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PmFolderModel left, PmFolderModel right)
        {
            return !(left == right);
        }
    }
}
