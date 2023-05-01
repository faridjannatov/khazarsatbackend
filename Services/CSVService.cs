using System.Globalization;
using CansatGround.Models;

namespace CansatGround.Services;

public class CSVService
{
    private readonly string csv_file_route;
    private int packagesCount = 0;
    private int linesCount { get; set; }

    public CSVService(IConfiguration configuration)
    {
        var cansatGroundProperties = configuration.To<CansatGroundProperties>("CansatGroundProperties");
        csv_file_route = cansatGroundProperties.CSVFileRoute;
        
        if(File.Exists(csv_file_route) == false) File.WriteAllText(csv_file_route, "Komanda ID;Calisma muddeti;Telemetriya paketlerinin sayı;Batareyanin gerginliyi;Hundurluk;" +
            "Suret;Cografi en;Cografi uzunluq;Dasiyicidan ayrildigi vaxt;Videogoruntunun muddeti\n");
        linesCount = File.ReadAllLines(csv_file_route).Length;
    }

    public void CSVWrite(string[] datas)
    {
        var sumText = string.Join(";", datas);
        WriteOnLine(sumText);
    }

    public int WriteOnLine(string input)
    {
        using (var sw = new StreamWriter(csv_file_route, true))
        {
            string[] values = input.Split(';');
            int indexToInsert = 2;
            Array.Resize(ref values, values.Length + 1);
            for (int i = values.Length - 1; i > indexToInsert; i--)
            {
                values[i] = values[i - 1];
            }
            values[indexToInsert] = linesCount.ToString();
            string output = string.Join(";", values);
        
            sw.WriteLine(output);
            linesCount++;
            return linesCount;
        }
    }

    public ResponseData CSVRead()
    {
        string[] baza = File.ReadAllLines(csv_file_route);
        string[] lines = baza.Reverse().Take(10).ToArray();

        if (linesCount <= 10)
        {
            Array.Resize(ref lines, linesCount - 1);
        }

        ResponseData responseData = new ResponseData();

        for (int i = lines.Length - 1; i >= 0; i--)
        {
            string[] data = lines[i].Split(";");
            int workingTime = Convert.ToInt32(data[1]);
            int packagesCount = Convert.ToInt32(data[2]);
            double voltage = double.Parse(data[3], CultureInfo.InvariantCulture);
            double altitude = double.Parse(data[4], CultureInfo.InvariantCulture);
            double speed = double.Parse(data[6], CultureInfo.InvariantCulture);
            double xAxis = double.Parse(data[7], CultureInfo.InvariantCulture);
            double zAxis = double.Parse(data[8], CultureInfo.InvariantCulture);
            string leftTime = data[9];
            int videoDuration = Convert.ToInt32(data[10]);

            responseData.workingTime[i] = workingTime;
            responseData.packagesCount[i] = packagesCount;
            responseData.voltage[i] = voltage;
            responseData.altitude[i] = altitude;
            responseData.speed[i] = speed;
            responseData.xAxis[i] = xAxis;
            responseData.zAxis[i] = zAxis;
            responseData.leftTime[i] = leftTime;
            responseData.videoDuration[i] = videoDuration;
        }

        return responseData;
    }
    
    public void testcsvwrite()
    {
        int videoDuration = 0;
        double speed, altitude, xAxis, zAxis, pressure, acceleration, voltage;
        Random random = new Random();
        string[] datasss = new string[10];
        while (true)
        {
            datasss[0] = 3311.ToString();
            datasss[1] = random.Next(5, 15).ToString();
            packagesCount++;
            datasss[2] = packagesCount.ToString();
            datasss[3] = random.Next(15, 55).ToString();
            datasss[4] = (random.Next(100, 120) / 100).ToString();
            datasss[5] = (random.Next(45, 55) / 10).ToString();
            datasss[6] = random.Next(1, 60).ToString();
            datasss[7] = DateTime.Now.ToString("dd/MM/yyyy");
            datasss[8] = videoDuration.ToString();
            datasss[9] = videoDuration.ToString();
            CSVWrite(datasss);
        }
    }
}

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public float Wage { get; set; }
}