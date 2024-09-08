# UnityRoslynAnalyzerSample

Unity での Roslyn の導入・使い方サンプルレポジトリです。

AnalyzerPlayground：C# Rolsyn プロジェクト<br/>
AnalyzerPlaygroundUnity：Unity プロジェクト<br/><br/>


**アナライザー：**

1. RA0001: `MaterialLeakSemanticAnalyzer`　（→ [コード](https://github.com/cji3bp62000/UnityRoslynAnalyzerSample/blob/main/AnalyzerPlayground/AnalyzerPlayground/AnalyzerPlayground/MaterialLeakSemanticAnalyzer.cs)）
   * マテリアル未破棄アナライザー
   * new() されて、Destroy していないマテリアルに対して、警告を出す
```C#
using UnityEngine;

public class MaterialDestroyAnalyzerTestScript : MonoBehaviour
{
    private Material matOK;
    private Material matNG;

    void Start()
    {
        matOK = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        // matNG は Destroy していないため、RA0001 警告あり
        matNG = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
    }

    private void OnDestroy()
    {
        Destroy(matOK);
    }
}
```

2. RA0002: `IfWhiteSpaceSyntaxAnalyzer`　（→ [コード](https://github.com/cji3bp62000/UnityRoslynAnalyzerSample/blob/main/AnalyzerPlayground/AnalyzerPlayground/AnalyzerPlayground/IfWhiteSpaceSyntaxAnalyzer.cs)）
   * `if` 文のコードフォーマットのアナライザー
   * `if` 文が正しいフォーマットでなければ、警告を出す (スペースが一個ずつあり、改行なし)
 
```C#
    // 正しいフォーマット例：(スペースが一個ずつあり、改行なし)
    // if (true) {
    //     ...
    // }

    // 正しいフォーマット、警告なし
    if (true) {
        i++;
    }
    if (true) i++;

    // 間違ったフォーマット、RA0002 警告あり
    if(true) {
        i++;
    }
    if (true)
    {
        i++;
    }

    if (true)
        i++;
```
