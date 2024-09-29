namespace AnalyzerPlayground.Sample;

public class ReadOnlyExamples
{
    public void MyMethod()
    {
        // [readonly] のタグをつけていると、宣言以降の代入ができなくなる
        // [readonly]
        var i = 1;
        // [readonly] のタグをつけていければ、代入ができる
        var j = 2;

        i = 3; // error
        j = 4;
    }
}