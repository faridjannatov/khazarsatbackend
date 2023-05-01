namespace CansatGround.Models;

public class CansatGroundProperties
{
    public int TeamID { get; set; }
    public string CSVFileRoute { get; set; }
    public XbeeProperties XbeeProperties { get; set; }
}

public class XbeeProperties
{
    public int BaudRate { get; set; }
    public string PortName { get; set; }
}

public class CSVData
{
    public int commandId { get; set; }
    public int workingTime { get; set; }
    public int packagesCount { get; set; }

    public double voltage { get; set; }
    public double altitude { get; set; }
    public double speed { get; set; }

    public double xAxis { get; set; }
    public double zAxis { get; set; }
    
    public string leftTime { get; set; } // dd:mm:yyyy hh:mm:ss
    public int videoDuration { get; set; }
    /*
     <Komanda ID>, <Çalışma müddəti>, <Telemetriya paketlərinin sayı>, <Batareyanın gərginliyi >,
<Hündürlük>, <Sürət>, <Coğrafi en >, <Coğrafi uzunluq>, <Daşıyıcıdan ayrıldığı vaxt>,
<Videogörüntünün müddəti>
     */
}


public class ResponseData
{
    public int[] workingTime { get; set; } = new int[10];
    public int[] packagesCount { get; set; }  = new int[10];

    public double[] voltage { get; set; } = new double[10];
    public double[] altitude { get; set; } = new double[10];
    public double[] speed { get; set; } = new double[10];

    public double[] xAxis { get; set; } = new double[10];
    public double[] zAxis { get; set; } = new double[10];
    
    public string[] leftTime { get; set; } = new string[10]; // dd:mm:yyyy hh:mm:ss
    public int[] videoDuration { get; set; } = new int[10];
}
