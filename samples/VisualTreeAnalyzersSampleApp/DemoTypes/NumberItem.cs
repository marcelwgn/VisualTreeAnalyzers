namespace AnalyzersSampleApp.DemoTypes
{
    class NumberItem
    {
        public int Number { get; set; }

        public string Name => "Number " + Number;

        public NumberItem(int number)
        {
            Number = number;
        }
    }
}
