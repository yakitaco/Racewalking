using System.Collections.Generic;

public class GameManager
{
    public Player CurrentPlayer { get; private set; }
    public float CurrentDistance { get; private set; }
    public float GoalDistance { get; private set; }
    
    public bool IsGameOver { get; private set; }
    public bool IsGameClear { get; private set; }

    private List<Judge> judges; // コースに立つ審判のリスト

    public GameManager(float goalDistance)
    {
        CurrentPlayer = new Player();
        CurrentDistance = 0f;
        GoalDistance = goalDistance;
        IsGameOver = false;
        IsGameClear = false;

        // 審判をコース上に配置（例：100m、200m、300m地点...）
        judges = new List<Judge>
        {
            new Judge(100f),
            new Judge(200f),
            new Judge(300f)
        };
    }

    // ゲームのメインループ（Unityの場合はUpdateメソッド内で呼ぶイメージ）
    public void UpdateGame(float deltaTime)
    {
        if (IsGameOver || IsGameClear) return;

        // 1. プレイヤーの状態を更新（スピードの減衰など）
        CurrentPlayer.UpdateState(deltaTime);

        // 2. スピードに応じて距離を進める
        CurrentDistance += CurrentPlayer.Speed * deltaTime;

        // 3. 審判によるチェック
        foreach (var judge in judges)
        {
            judge.ObservePlayer(CurrentPlayer, CurrentDistance);
        }

        // 4. 勝敗判定（失格チェック）
        if (CurrentPlayer.IsDisqualified())
        {
            IsGameOver = true;
            // ※ ここでゲームオーバー画面を表示する処理などを呼ぶ
            return;
        }

        // 5. ゴール判定
        if (CurrentDistance >= GoalDistance)
        {
            IsGameClear = true;
            // ※ ここでクリアタイムのリザルト画面を表示する処理などを呼ぶ
        }
    }
}