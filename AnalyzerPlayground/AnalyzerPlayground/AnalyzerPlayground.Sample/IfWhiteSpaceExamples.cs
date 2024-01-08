namespace AnalyzerPlayground.Sample
{
    public class IfWhiteSpaceExamples
    {
        public int i;

        private void Method()
        {
            // ok:
            if (true) {
                i++;
            }
            if (true) i++;

            // ng
            if(true) {
                i++;
            }
            if (true)  {
                i++;
            }
            if (true)
            {
                i++;
            }
            if(true) i++;
            if (true)i++;

            if (true)
                i++;
        }
    }
}
