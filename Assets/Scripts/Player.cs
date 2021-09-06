using System;

public class Player
{
    // プロパティ（外部から読み取り可能、書き込みはクラス内のみ）
    public float Speed { get; private set; }
    public float Stamina { get; private set; }
    public float FormQuality { get; private set; } // 100が完璧、0が完全に反則状態
    public int RedCardCount { get; private set; }

    // 定数
    private const float MaxStamina = 100f;
    private const float MaxFormQuality = 100f;
    private const int DisqualificationCardCount = 3; // 3枚で失格

    public Player()
    {
        // 初期化
        Speed = 0f;
        Stamina = MaxStamina;
        FormQuality = MaxFormQuality;
        RedCardCount = 0;
    }

    // プレイヤーが「歩く」入力をした時に呼ばれるメソッド
    // isJustTiming: リズムに合わせて完璧なタイミングで入力できたかどうか
    public void Step(bool isJustTiming)
    {
        if (Stamina <= 0) return; // スタミナ切れなら動けない

        if (isJustTiming)
        {
            // 完璧なタイミング：速度が上がり、フォームも維持
            Speed += 2.0f;
            FormQuality = Math.Min(FormQuality + 10f, MaxFormQuality); // フォームが少し改善
            Stamina -= 1.0f; // スタミナ消費は少なめ
        }
        else
        {
            // タイミングがずれた、または連打した時：速度は出るがフォームが崩壊
            Speed += 3.0f; // 無理やり進むので速度は一時的に上がる
            FormQuality -= 30f; // フォーム（姿勢）が大きく崩れる
            Stamina -= 3.0f; // スタミナを激しく消費
        }
    }

    // 毎フレーム（常に）呼ばれる状態更新メソッド
    public void UpdateState(float deltaTime)
    {
        // 時間経過とともにスピードは自然に落ちる（摩擦や空気抵抗のイメージ）
        Speed = Math.Max(0, Speed - (deltaTime * 5f));

        // フォームも時間経過で少しずつ元の正しい姿勢に戻ろうとする
        FormQuality = Math.Min(MaxFormQuality, FormQuality + (deltaTime * 10f));
    }

    // 審判からレッドカードを受ける
    public void ReceiveRedCard()
    {
        RedCardCount++;
    }

    // 失格かどうかを判定
    public bool IsDisqualified()
    {
        return RedCardCount >= DisqualificationCardCount;
    }
}