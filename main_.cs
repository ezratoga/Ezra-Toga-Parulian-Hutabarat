using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FileIO12
{
  class main
  {

    static string output_file = "";
    static string type_ext = "";
    static string input_file = "";

    static void Main (string[] LinedOperands)
    {
      int the_length = LinedOperands.Length;
      Console.WriteLine("Length is {0}", LinedOperands.Length);
      switch (LinedOperands.Length)
      {
        case 0: Console.WriteLine("Error: Berikan operand -h, -t dan/atau -o"); break;
        case 1: Handle1Operand(LinedOperands[0]); break;
        case 2: Handle2Operands(LinedOperands[0], LinedOperands[1]); break;
        case 3: Handle3Operands(LinedOperands[0], LinedOperands[1], LinedOperands[2]); break;
        case 4: Handle4Operands(LinedOperands[0], LinedOperands[1], LinedOperands[2], LinedOperands[3]); break;
        case 5: Handle5Operands(LinedOperands[0], LinedOperands[1], LinedOperands[2], LinedOperands[3], LinedOperands[4]); break;
        default: Console.WriteLine("Error: Excessive operands"); break;
      }
      Console.WriteLine("Press any key to continue... ");
      Console.ReadKey();
    }

    static void ReadMe()
    {
      StreamReader reader = new StreamReader("readme.txt");
      Console.WriteLine(reader.ReadToEnd());
      reader.Close();
    }

    static void Handle1Operand(string operand1)
    {
      if (operand1 == "-h")
      {
        ReadMe();
      }
      else if (operand1 == "-t" || operand1 == "-o")
        Console.WriteLine("Tuliskan sintaks program dengan benar. Ketik mytools -h untuk bantuan");
      else
      {
        type_the_target("text", operand1);
      }
    }
    
    static void Handle2Operands(string operand1, string operand2)
    {
      List<string> oprnd = new List<string>();
      oprnd.Add(operand1);
      oprnd.Add(operand2);

      switch (operand1)
      {
        case "-h": ReadMe(); break;
        default: Console.WriteLine("Tuliskan sintaks program dengan benar. Ketik mytools -h untuk bantuan"); break;
      }
    }

    static void type_the_target (string the_type, string filepath)
    {
      switch (the_type)
      {
        case "json": column_json(filepath); break;  // so-called convert to text
        case "text": column_texts(filepath); break;
        default: Console.WriteLine("Tuliskan sintaks program dengan benar. Ketik mytools -h untuk bantuan");; break;
      }
    }

    static string StoreToPlainText(string operandPath1)
    {
      string input = System.IO.File.ReadAllText(operandPath1);
      string inputLine = "";
      StringReader reader = new StringReader(input);
      string Data1 = "";
      int i=1;
      while((inputLine = reader.ReadLine()) != null)
      {
        string[] inputArray = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Data1 +=    "ID = "+ i + "\n" +
                    "TimeStamp = " + inputArray[0] +" "+ inputArray[1] + "\n" +
                    "Level = "+inputArray[2] + "\n" +
                    "PID = "+inputArray[3] + "\n" +
                    "ErorrMassage = "+inputArray[4]+" "+inputArray[5]+" "+inputArray[6]+" "+inputArray[7] + "\n" +
                    "ErorrDetail = "+string.Join(" ", inputArray.Skip(8));
      }
             //string jsonString = Data1.ToJSON();
      return Data1;
    }  

    static String StoreToDataRecord(string operandPath1)
    {
      string input = System.IO.File.ReadAllText(operandPath1);
      string inputLine = "";
      StringReader reader = new StringReader(input);
      Module01 ModuleData1 = new Module01();
      List<Module01> Data1 = new List<Module01>();
      int i=1;
      while((inputLine = reader.ReadLine()) != null)
      {
        string[] inputArray = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Data1.Add(new Module01() {  ID = i++, 
                                    TimeStamp = inputArray[0]+" "+inputArray[1], 
                                    Level = inputArray[2],
                                    PID = inputArray[3],
                                    ErorrMassage = inputArray[4]+" "+inputArray[5]+" "+inputArray[6]+" "+inputArray[7],
                                    ErorrDetail = string.Join(" ", inputArray.Skip(8)),
                                  });
                  
                   
      }
      string jsonString = Data1.ToJSON();
      return jsonString;
    }  

    static void column_texts(string operand1)
    {
      try
      {
        System.IO.StreamWriter serialf;

        string path = operand1;
        string[] baris = File.ReadAllLines(path);
        int jumlah_baris = baris.Length;
        FileInfo metafile;
        FileInfo metafile2 = new FileInfo(path);

        if (metafile2.IsReadOnly == true)
          Console.WriteLine("File is Read-Only");
            
        else
        {
          string FileExt = Path.ChangeExtension(path, ".txt");

          metafile = new FileInfo(FileExt);
          string FileName = metafile.Name;

          serialf = new StreamWriter(FileName);
          
          if (metafile2.Name == "error.log")
          {
            string DataResult = StoreToPlainText(path);
            serialf.WriteLine(DataResult);
            serialf.Close();
          }
          else
          {
            for (int a = 0; a < jumlah_baris; a++)
            {
              serialf.WriteLine(baris[a]);
            }
            serialf.Close();
          }
        }
      }
      catch (IOException error)
      {
        if (error.Message.Contains("already exist"))
          Console.WriteLine("Nama file sudah ada. Silahkan rename file!");
        else
          Console.WriteLine(error.Message);
      }
      catch (UnauthorizedAccessException error_access) 
      {
        Console.WriteLine(error_access.Message);
      }
    }

    static void column_texts_out(string operand1, string operand2)
    {
      try
      {
        System.IO.StreamWriter serialf;

        string path = operand1;
        string[] baris = File.ReadAllLines(path);
        int jumlah_baris = baris.Length;
        FileInfo metafile;
        FileInfo metafile2 = new FileInfo(path);

        if (metafile2.IsReadOnly == true)
          Console.WriteLine("File is Read-Only");
            
        else
        {
            metafile = new FileInfo(operand1);
            string FileName = metafile.FullName;
            serialf = new StreamWriter(operand2);

            if (metafile2.Name == "error.log")
            {
              string DataResult = StoreToPlainText(path);
              serialf.WriteLine(DataResult);
              serialf.Close();
            }
            else
            {
              for (int a = 0; a < jumlah_baris; a++)
              {
                serialf.WriteLine(baris[a]);
              }
            }
            serialf.Close();
        }
      }
      catch (IOException error)
      {
        if (error.Message.Contains("already exist"))
          Console.WriteLine("Nama file sudah ada. Silahkan rename file!");
        else
          Console.WriteLine(error.Message);
      }
      catch (UnauthorizedAccessException error_access) 
      {
        Console.WriteLine(error_access.Message);
      }
    }

    static void column_json(string operand1)
    {
      try
      {
        System.IO.StreamWriter serialf;
        string path = operand1;
        string[] baris = File.ReadAllLines(path);
        int jumlah_baris = baris.Length;
        
        FileInfo metafile;
        FileInfo metafile2 = new FileInfo(path);
        
        if (metafile2.IsReadOnly == true)
          Console.WriteLine("File is Read-Only");
            
        else
        {
          string FileExt = Path.ChangeExtension(path, ".json");

          metafile = new FileInfo(FileExt);
          string FileName = metafile.Name;

          serialf = new StreamWriter(FileName);
          
          if (metafile2.Name == "error.log")
          {
            string DataResult = StoreToDataRecord(path);
            serialf.WriteLine(DataResult);
            serialf.Close();
          }
          else
          {
            for (int a = 0; a < jumlah_baris; a++)
            {
              serialf.WriteLine(baris[a]);
            }
            serialf.Close();
          }
        }
      }
      catch (IOException error)
      {
        if (error.Message.Contains("already exist"))
          Console.WriteLine("Nama file sudah ada. Silahkan rename file!");
        else
          Console.WriteLine(error.Message);
      }
      catch (UnauthorizedAccessException error_access) 
      {
        Console.WriteLine(error_access.Message);
      }
    }

    static void Handle3Operands(string operand1, string operand2, string operand3)
    {
      List<string> oprnd = new List<string>();
      oprnd.Add(operand1);
      oprnd.Add(operand2);
      oprnd.Add(operand3);

      for (int a = 0; a < oprnd.Count; a++)
      {
        if (oprnd[a] == "-o")
          output_file = output_file + oprnd[a+1];
        else if (oprnd[a] == "-t")
          type_ext = type_ext + oprnd[a+1];
        else if (oprnd[a] != "-o" && oprnd[a] != output_file && oprnd[a] != "-t" && oprnd[a] != type_ext)
          input_file = input_file + oprnd[a];
      }

      oprnd.Sort(CompareReversed);

      for (int a = 0; a < oprnd.Count; a++)
      {
        if (oprnd[a] == "-o" && oprnd[a+1] != output_file)
        {
          oprnd[a+1] = output_file;
        }
        else if (oprnd[a] == "-t" && oprnd[a+1] != type_ext)
        {
          oprnd[a+1] = type_ext;
        }

        if (oprnd[a] == type_ext || oprnd[a] == output_file)
        {
          if (a == 0)
            oprnd[a] = input_file;
          else 
          {
            if (oprnd[a-1] != "-t" && oprnd[a-1] != "-o")
              oprnd[a] = input_file;
          }
        }
        Console.Write(oprnd[a] + " ");

        if (oprnd[a] == input_file)
        {
          switch (oprnd[a])
          {
            default: break;
          }
        }
        else if (oprnd[a] == output_file)
        {
          switch (oprnd[a])
          {
            default: break;
          }
        }
        else
        {
          switch (oprnd[a])
          {
            case "-h": ReadMe(); break;
            case "-t": type_the_target (oprnd[a+1], input_file); break;
            case "text" : break;
            case "json" : break;
            case "-o": column_texts_out(input_file, output_file); break;
            default: Console.WriteLine("Tuliskan sintaks program dengan benar. Ketik mytools -h untuk bantuan"); break;
          }
        }
      }
    }

    public static int CompareReversed (string value1, string value2)
    {
      return Reverse(value1).CompareTo(Reverse(value2));
    }

    public static string Reverse (string value)
    {
      return new string(value.ToArray().Reverse().ToArray());
    }

    static void Handle4Operands(string operand1, string operand2, string operand3, string operand4)
    {
      List<string> oprnd = new List<string>();
      oprnd.Add(operand1);
      oprnd.Add(operand2);
      oprnd.Add(operand3);
      oprnd.Add(operand4);

      for (int a = 0; a < oprnd.Count; a++)
      {
        if (oprnd[a] == "-o")
          output_file = output_file + oprnd[a+1];
        else if (oprnd[a] == "-t")
          type_ext = type_ext + oprnd[a+1];
        else if (oprnd[a] != "-o" && oprnd[a] != output_file && oprnd[a] != "-t" && oprnd[a] != type_ext)
          input_file = input_file + oprnd[a];
      }

      oprnd.Sort(CompareReversed);

      for (int a = 0; a < oprnd.Count; a++)
      {
        if (oprnd[a] == "-o" && oprnd[a+1] != output_file)
        {
          oprnd[a+1] = output_file;
        }
        else if (oprnd[a] == "-t" && oprnd[a+1] != type_ext)
        {
          oprnd[a+1] = type_ext;
        }

        if (oprnd[a] == type_ext || oprnd[a] == output_file)
        {
          if (a == 0)
            oprnd[a] = input_file;
          else 
          {
            if (oprnd[a-1] != "-t" && oprnd[a-1] != "-o")
              oprnd[a] = input_file;
          }
        }
        Console.Write(oprnd[a] + " ");

        if (oprnd[a] == input_file)
        {
          switch (oprnd[a])
          {
            default: break;
          }
        }
        else if (oprnd[a] == output_file)
        {
          switch (oprnd[a])
          {
            default: break;
          }
        }
        else
        {
          switch (oprnd[a])
          {
            case "-h": ReadMe(); break;
            case "-t": type_the_target (oprnd[a+1], input_file); break;
            case "text" : break;
            case "json" : break;
            case "-o": column_texts_out(input_file, output_file); break;
            default: Console.WriteLine("Tuliskan sintaks program dengan benar. Ketik mytools -h untuk bantuan"); break;
          }
        }

      }
    }

    static void Handle5Operands(string operand1, string operand2, string operand3, string operand4, string operand5)
    {
      List<string> oprnd = new List<string>();
      oprnd.Add(operand1);
      oprnd.Add(operand2);
      oprnd.Add(operand3);
      oprnd.Add(operand4);

      for (int a = 0; a < oprnd.Count; a++)
      {
        if (oprnd[a] == "-o")
          output_file = output_file + oprnd[a+1];
        else if (oprnd[a] == "-t")
          type_ext = type_ext + oprnd[a+1];
        else if (oprnd[a] != "-o" && oprnd[a] != output_file && oprnd[a] != "-t" && oprnd[a] != type_ext)
          input_file = input_file + oprnd[a];
      }

      oprnd.Sort(CompareReversed);

      for (int a = 0; a < oprnd.Count; a++)
      {
        if (oprnd[a] == "-o" && oprnd[a+1] != output_file)
        {
          oprnd[a+1] = output_file;
        }
        else if (oprnd[a] == "-t" && oprnd[a+1] != type_ext)
        {
          oprnd[a+1] = type_ext;
        }

        if (oprnd[a] == type_ext || oprnd[a] == output_file)
        {
          if (a == 0)
            oprnd[a] = input_file;
          else 
          {
            if (oprnd[a-1] != "-t" && oprnd[a-1] != "-o")
              oprnd[a] = input_file;
          }
        }
        Console.Write(oprnd[a] + " ");

        if (oprnd[a] == input_file)
        {
          switch (oprnd[a])
          {
            default: break;
          }
        }
        else if (oprnd[a] == output_file)
        {
          switch (oprnd[a])
          {
            default: break;
          }
        }
        else
        {
          switch (oprnd[a])
          {
            case "-h": ReadMe(); break;
            case "-t": type_the_target (oprnd[a+1], input_file); break;
            case "text" : break;
            case "json" : break;
            case "-o": column_texts_out(input_file, output_file); break;
            default: Console.WriteLine("Tuliskan sintaks program dengan benar. Ketik mytools -h untuk bantuan"); break;
          }
        }

      }
    }
  }
}