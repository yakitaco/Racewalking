using System;

public class InputController
{
    private Player targetPlayer;

    // 足の状態管理用
    public enum Foot { None, Left, Right }
    private Foot lastFoot = Foot.None;

    // タイミング判定用の設定値（秒単位）
    private float lastStepTime = 0f;
    private const float TargetInterval = 0.5f;   // 理想の歩行テンポ（0.5秒間隔）
    private const float JustWindow = 0.1f;       // ジャスト判定の許容誤差（±0.1秒）
    private const float PenaltyInterval = 0.2f;  // これより早いと「連打」とみなす極端なミス

    public InputController(Player player)
    {
        targetPlayer = player;
    }

    // 毎フレーム呼ばれ、入力の有無と現在のゲーム時間を処理する
    // （Unityなら Updateメソッド内で Input.GetKeyDown 等の結果を渡す）
    public void ProcessInput(bool isLeftPressed, bool isRightPressed, float currentTime)
    {
        // どちらの入力もなければ何もしない
        if (!isLeftPressed && !isRightPressed) return;

        // 同時押しはミス（あるいは無視）として扱う
        if (isLeftPressed && isRightPressed) return;

        Foot currentFoot = isLeftPressed ? Foot.Left : Foot.Right;

        // 1. 「交互に足を出しているか」のチェック
        if (currentFoot == lastFoot)
        {
            // 同じ足を連続で出そうとした（つまずくイメージ）
            // ※ペナルティとしてStep(false)を呼ぶか、無視するかはゲームバランス次第
            targetPlayer.Step(false); 
            lastStepTime = currentTime;
            return;
        }

        // 2. 「タイミング（リズム）」のチェック
        float interval = currentTime - lastStepTime;
        bool isJustTiming = false;

        // 最初の1歩目は基準がないので常にジャストタイミング扱いにする
        if (lastFoot == Foot.None || interval >= 2.0f) 
        {
            isJustTiming = true;
        }
        else
        {
            // 理想のテンポ（TargetInterval）との差を計算
            float diff = Math.Abs(interval - TargetInterval);

            if (diff <= JustWindow)
            {
                // 誤差範囲内なら完璧なフォーム！
                isJustTiming = true;
            }
            else if (interval < PenaltyInterval)
            {
                // 異常に早い（焦ってキーを連打している状態）＝ フォーム大崩壊
                isJustTiming = false; 
            }
            else
            {
                // ちょっと早い、または遅い場合（フォームが乱れる）
                isJustTiming = false;
            }
        }

        // 3. プレイヤーに結果を伝達して状態を更新
        targetPlayer.Step(isJustTiming);

        // 4. 今回の入力情報を「過去」として記録
        lastFoot = currentFoot;
        lastStepTime = currentTime;
    }
}