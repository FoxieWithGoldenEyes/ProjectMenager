using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IODll
{
    // Provids Metods to Write and Read for all allowed file types
    public partial class AppFile<T> : IEnumerable<T>
    {
        public enum FileType
        {
            JsonData, LinedText
        }
        
        
        public AppFile(string path, FileType type)
        {
            // Set path
            if (System.IO.Path.IsPathFullyQualified(path) is false)
                throw new InvalidOperationException();
            this.Path = System.IO.Path.GetFullPath(path);

            // Create Parent directory if not exist
            if(!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName (path));

            this.Type = type;

            // Creates file
            if (!File.Exists(Path))
            {
                File.Create(Path).Close();

                // Init file if needed
                if(type is FileType.JsonData)
                    WriteAllEntries(new List<T>());
            }

        }


        public string Path { get; }
        public FileType Type { get; }


        public void WriteAllEntries(List<T> entries)
        {
            switch (Type)
            {
                case AppFile<T>.FileType.JsonData:
                    string jsonText = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(Path, jsonText);
                    break;
                case AppFile<T>.FileType.LinedText:
                    List<string> stringList = entries.ConvertAll(item => item?.ToString() ?? string.Empty);
                    File.WriteAllLines(Path, stringList);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid data format, file format is: {Type}");
            }
        }
        public List<T> ReadAllEntries()
        {
            switch (Type)
            {
                case AppFile<T>.FileType.JsonData:
                    string jsonText = File.ReadAllText(Path);
                    var entities = JsonSerializer.Deserialize<List<T>>(jsonText) ?? new List<T>();
                    return entities;
                case AppFile<T>.FileType.LinedText:
                    var textLines = File.ReadLines(Path).ToList();
                    var textLinesT = textLines.Cast<T>().ToList();
                    return textLinesT;
                default:
                    throw new InvalidOperationException($"Invalid data format, file format is: {Type}");
            }
        }
    }

    // IEnumberable
    public partial class AppFile<T>
    {
        private List<T> Data { get => ReadAllEntries(); set => WriteAllEntries(value); }
     
        
        public void Add(T item)
        {
            var fileData = Data;
            fileData.Add(item);
            Data = fileData;
        }
        public void AddRange(IEnumerable<T> items)
        {
            var fileData = Data;
            fileData.AddRange(items);
            Data = fileData;
        }

        public bool Remove(T item)
        {
            var fileData = Data;
            bool result = fileData.Remove(item);
            Data = fileData;

            return result;
        }
        public int RemoveAll(Predicate<T> match)
        {
            var fileData = Data;
            int foundEntities = fileData.RemoveAll(match);
            Data = fileData;

            return foundEntities;
        }
        

        // Implementacja IEnumerable<T> pozwala na pobieranie danych i ich przekształcanie z kolekcji
        // Metody te nie modyfikują danych, do modyfikacji należy użyć przeciążonych własnych metod
        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
