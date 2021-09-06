using UnityEngine;

public class RaceGameDirector : MonoBehaviour
{
    // Unityエディタ上から、動かしたいCubeをセットするための枠
    public GameObject playerCube; 

    // 内部ロジックの管理クラス
    private GameManager gameManager;
    private InputController inputController;

    void Start()
    {
        // ゲーム開始時の初期化（例：ゴールを100m先に設定）
        gameManager = new GameManager(100f);
        
        // GameManagerの中にあるPlayerデータをInputControllerに渡して紐づける
        inputController = new InputController(gameManager.CurrentPlayer);
    }

    void Update()
    {
        // 1. プレイヤーの入力を取得（Aキーを左足、Dキーを右足とする）
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        // 2. 入力判定ロジックへ情報を渡す
        // Time.timeはゲーム開始からの経過時間（秒）
        inputController.ProcessInput(left, right, Time.time);

        // 3. ゲーム全体の進行（スピードの減衰や距離の計算など）
        // Time.deltaTimeは前回のフレームからの経過時間
        gameManager.UpdateGame(Time.deltaTime);

        // 4. 計算された「距離」をCubeの座標に反映させて前進させる
        // 今回はZ軸（奥の方向）に向かって進むようにします
        Vector3 newPos = playerCube.transform.position;
        newPos.z = gameManager.CurrentDistance; 
        playerCube.transform.position = newPos;

        // 【テスト用】現在の状態をConsoleウィンドウに出力して確認
        // A・Dキーを交互に押すと、数値がどう変化するか確認できます
        Debug.Log($"距離: {gameManager.CurrentDistance:F1}m | " +
                  $"速度: {gameManager.CurrentPlayer.Speed:F1} | " +
                  $"フォーム: {gameManager.CurrentPlayer.FormQuality:F0}/100 | " +
                  $"レッドカード: {gameManager.CurrentPlayer.RedCardCount}");
    }
}