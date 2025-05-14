using System.Collections.ObjectModel;

public class Day
{
    public ObservableCollection<TimeReservation> Info { get; set; }
    public Day(ObservableCollection<TimeReservation> info)
    {
        Info = info;
    }

    public static Day SampleDay()
    {
        var info = new ObservableCollection<TimeReservation>();
        for (int i = 8; i < 16; i++)
        {
            info.Add(new TimeReservation(i, true));
        }
        return new Day(info);
    }
}
public class TimeReservation
{
    public int Time { get; set; }
    public bool IsReserved { get; set; }

    public TimeReservation(int time, bool isReserved)
    {
        this.Time = time;
        this.IsReserved = isReserved;
    }

}