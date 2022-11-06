
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

class Program
{
    static void Main(string[] args)
    {

        Console.Write("Напишите название файла. Н-р: англо-русский, \nрусско-английский и т.д.: ");
        string input = Console.ReadLine();
        Dict dict = new Dict(input);//сразу даем название словарю и файлу где он будет находиться
        dict.Menu();
        while(true)
        {
            string str = Console.ReadLine();
            int int_input = int.Parse(str);
            switch(int_input)
            {
                case 1:
                    dict.GetTranslation();
                    break;
                case 2:
                    dict.AddWord();
                    break;
                case 3:
                    dict.RemoveWord();//удалить перевод какого-либо слова
                    break;
                case 4:
                    dict.GetTranslationCount();//посчитать сколько переводов у данного слова
                    break;
                case 5:
                    dict.Save();
                    break;
                default:
                    break;
            }
            dict.Menu();
            
        }
    }


}


interface Dict1
{
    public void AddWord(string word1, string word2);
    public void RemoveWord(string word);
    public string GetTranslation(string word);
    public int GetTranslationCount(string word);
}

[Serializable]  
public class Dict
{
    protected Dictionary<string, HashSet<string>> dict = new Dictionary<string, HashSet<string>>();
    [XmlAttribute]
    public string name { get; set; }

    BinaryFormatter bf = new BinaryFormatter();
    public Dict() { }

    public Dict(string _name) { this.name = _name; }
    public Dict(Dictionary<string, HashSet<string>> dict, string name)
    {
        this.dict = dict;
        this.name = name;
    }

    public void AddWord()
    {
        Console.WriteLine("\nВведите слово");
        string word1 = Console.ReadLine();

        Console.WriteLine("Введите перевод");
        string word2 = Console.ReadLine();
        
        if(!dict.ContainsKey(word1))
        {
            dict.Add(word1, new HashSet<string>());
        }
        dict[word1].Add(word2);
        Console.WriteLine($"Добавлен перевод {word2} к слову {word1}");
       
        
        Console.WriteLine();
    }

    
    public void RemoveWord()
    {
        Console.Write("\nПеревод какого слова удалить? - ");
        string word = Console.ReadLine();
        Console.Write("Какой перевод удалить? - ");
        string translation = Console.ReadLine();
        
        if(dict.ContainsKey(word))
        {
            foreach (string w in dict.Keys)
            {
                if (w == word && dict[w].Count > 1)
                {
                    dict[w].Remove(translation);
                    break;
                }
            }
            Console.WriteLine($"Перевод {translation} слова {word} был удален со словаря");
        }
        else
        {
            Console.WriteLine("Такого слова в словаре нет!");
        }
        
    }
    public void GetTranslation()
    {
        Console.Write("\nИщем перевод слова. Введите слово: ");
        string word = Console.ReadLine();
        if(dict.ContainsKey(word))
        {
            Console.Write($"Перевод слова {word} = ");
            foreach(string w in dict[word])
            {
                Console.Write($"{w}, ");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("\nТакого слова нет, простите");
        }
       
    }
    public void GetTranslationCount()
    {
        Console.Write("\nСчитаем сколько переводов у слова. Введите слово - ");
        string word = Console.ReadLine();
        if(dict.ContainsKey(word))
        {
            foreach (string w in dict.Keys)
            {
                if (w == word)
                {
                    Console.WriteLine($"У слова {word} {dict[w].Count} переводов");
                }
                break;
            }
        }
        else
        {
            Console.WriteLine("Такого слова в переводе нет!");
        }   
    }

    public void Menu()
    {
        Console.WriteLine($"\nСловарь {name}");
        Console.WriteLine("1. Найти перевод");
        Console.WriteLine("2. Добавить слово/перевод");
        Console.WriteLine("3. Удалить перевод");
        Console.WriteLine("4. Узнать количество переводов слова");
        Console.WriteLine("5. Записать словарь в журнал");
        Console.Write("Выберите: ");
    }

    public void Save()//запись слов в файл
    {
        /*try
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {                
                
                XmlSerializer XML = new XmlSerializer(typeof(Dict));
                
                XML.Serialize(stream, this);
                Console.WriteLine($"Словарь был загружен в файл {fileName}");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }*/

        XmlTextWriter writer = null;
        try
        {
            writer = new XmlTextWriter($"{name}.xml", System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Words");
            foreach(string w in dict.Keys)
            {
                writer.WriteStartElement(w);// запись 1 слова
                foreach(string translation in dict[w])
                {
                    writer.WriteStartElement($"{translation}");//запись каждого перевода в словарь
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            Console.WriteLine($"Словарь был загружен в файл {name}.xml");
        }
        catch(Exception ex)
        {
            
        }
        finally
        {
            if(writer != null)
            {
                writer.Close();
            }
        }
    }

}