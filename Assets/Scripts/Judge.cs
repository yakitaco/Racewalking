public class Judge
{
    public float PositionDistance { get; private set; } // 審判が立っている距離
    private bool hasJudged = false; // 既に判定済みかどうかのフラグ

    // 許容されるフォームの最低ライン（これより低いと反則）
    private const float FoulThreshold = 50f; 

    public Judge(float position)
    {
        PositionDistance = position;
    }

    // 毎フレーム呼ばれ、プレイヤーが自分の前を通過したかを監視する
    public void ObservePlayer(Player player, float currentDistance)
    {
        // まだ判定しておらず、プレイヤーが審判の位置を通過したら
        if (!hasJudged && currentDistance >= PositionDistance)
        {
            hasJudged = true; // 判定済みにする

            // 通過した瞬間のプレイヤーのフォームをチェック
            if (player.FormQuality < FoulThreshold)
            {
                // フォームが崩れていたため、ロス・オブ・コンタクト等の反則！
                player.ReceiveRedCard();
                // ※ ここでUI側に「レッドカード発生！」というイベントを飛ばす処理が入る想定です
            }
            else
            {
                // セーフ（綺麗なフォームで通過）
            }
        }
    }
}