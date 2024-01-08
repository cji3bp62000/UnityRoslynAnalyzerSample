using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfWhitespaceAnalyzerTestScript : MonoBehaviour
{
    void Test()
    {
        var i = 0;

        // 正しいフォーマット例：
        // if (true) {
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
