using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// この Asmdef の Ruleset で、RA0001 を無効化し、 RA0002 をエラーにしています
/// </summary>
// <Rule Id="RA0001" Action="None" />
// <Rule Id="RA0002" Action="Error" />
public class AsmdefRulesetTestScript : MonoBehaviour
{
    private Material matNG_RuleOK;

    void Start()
    {
        // RA0001 警告でしたが、Ruleset で無効化 (None) しているので、警告なし
        // (material 未破棄警告)
        matNG_RuleOK = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        // RA0002 警告でしたが、Ruleset でエラー (Error) になっています
        // コメントを外すと、Unity もコンパイルエラーになります
        // (if 文のフォーマット警告)
        /*
        if(true) {

        }
        */
    }
}
