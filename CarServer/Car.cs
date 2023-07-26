class Car
{
    public int Id { get; set; }
    public string? Marka { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }

    public Car() { }

    public Car(int id, string? marka, string? model, int year)
    {
        Id = id;
        Marka = marka;
        Model = model;
        Year = year;
    }

    public override string ToString() => $"{Id}.{Marka} {Model} {Year}";
}
