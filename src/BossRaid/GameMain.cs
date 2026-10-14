// -------------------------------------------------------------------------------------------------------------------------------------------------------------
// Author: 3dapi (https://github.com/3dapi)
// -------------------------------------------------------------------------------------------------------------------------------------------------------------

using System.Windows.Forms;
using Vortice.Mathematics;

class GameMain : G2AppBase
{
    public override System.Drawing.Size ScreenSize => GameGlobal.ScreenSize;
    public override string GameName => GameGlobal.GameName;

    // ---------------------------------------
    // 게임 상태
    // ---------------------------------------
    private enum GameState
    {
        Start,
        Playing,
        GameOver
    }

    private GameState _currentState = GameState.Start;

    private const float TimeLimit = 90f;      // 제한시간(초) - 테스트하며 조정
    private float _remainingTime = TimeLimit; // 남은 시간
    private bool _isVictory = false;          // true면 보스 처치 성공, false면 패배

    // ---------------------------------------
    // 폰트
    // ---------------------------------------
    private G2Font? _titleFont;
    private G2Font? _guideFont;
    private G2Font? _timerFont;
    private G2Font? _resultFont;

    protected override void Initialize()
    {
        //---------------------------------------
        // 게임 관련 객체를 생성합니다.
        //---------------------------------------
        this.ClearColor = new Color4(0.05f, 0.05f, 0.1f, 1.0f);

        // 폰트는 한 번만 생성해서 재사용
        _titleFont = new G2Font("Consolas", 48f,
            Vortice.DirectWrite.FontWeight.Bold,
            Vortice.DirectWrite.FontStyle.Normal,
            Vortice.DirectWrite.TextAlignment.Center,
            Vortice.DirectWrite.ParagraphAlignment.Center);

        _guideFont = new G2Font("Consolas", 20f,
            Vortice.DirectWrite.FontWeight.Normal,
            Vortice.DirectWrite.FontStyle.Normal,
            Vortice.DirectWrite.TextAlignment.Center,
            Vortice.DirectWrite.ParagraphAlignment.Center);

        _timerFont = new G2Font("Consolas", 20f,
            Vortice.DirectWrite.FontWeight.Bold,
            Vortice.DirectWrite.FontStyle.Normal,
            Vortice.DirectWrite.TextAlignment.Center,
            Vortice.DirectWrite.ParagraphAlignment.Near);

        _resultFont = new G2Font("Consolas", 32f,
            Vortice.DirectWrite.FontWeight.Bold,
            Vortice.DirectWrite.FontStyle.Normal,
            Vortice.DirectWrite.TextAlignment.Center,
            Vortice.DirectWrite.ParagraphAlignment.Center);
    }

    protected override void Update()
    {
        switch (_currentState)
        {
            case GameState.Start:
                UpdateStart();
                break;
            case GameState.Playing:
                UpdatePlaying();
                break;
            case GameState.GameOver:
                UpdateGameOver();
                break;
        }
    }

    private void UpdateStart()
    {
        if (G2AppBase.Instance.Input.IsKeyDown(Keys.Space))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        _remainingTime = TimeLimit;
        _isVictory = false;

        // TODO: Player, Boss, Bullet 초기화(Reset)는 여기서 호출

        _currentState = GameState.Playing;
    }

    private void UpdatePlaying()
    {
        float dt = (float)G2AppBase.Instance.DeltaTime;

        // 제한시간 카운트다운
        _remainingTime -= dt;
        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
            _isVictory = false; // 시간 초과 = 패배
            _currentState = GameState.GameOver;
            return;
        }

        //---------------------------------------
        // 게임 관련 객체를 갱신합니다.
        //---------------------------------------
        // TODO: Player / Boss / Bullet 갱신, 충돌 판정
        // TODO: 보스 HP 0 -> _isVictory = true; GameOver
        // TODO: 플레이어 HP 0 -> _isVictory = false; GameOver
    }

    private void UpdateGameOver()
    {
        if (G2AppBase.Instance.Input.IsKeyDown(Keys.Space))
        {
            StartGame();
        }
    }

    protected override void Render()
    {
        switch (_currentState)
        {
            case GameState.Start:
                RenderStart();
                break;
            case GameState.Playing:
                RenderPlaying();
                break;
            case GameState.GameOver:
                RenderGameOver();
                break;
        }
    }

    private void RenderStart()
    {
        _titleFont?.DrawText("Boss Raid",
            new Rect(0, 200, GameGlobal.ScreenSize.Width, 80),
            new Color4(1f, 1f, 1f, 1f));

        _guideFont?.DrawText("스페이스바를 눌러 시작하세요!",
            new Rect(0, 340, GameGlobal.ScreenSize.Width, 40),
            new Color4(1f, 1f, 1f, 1f));
    }

    private void RenderPlaying()
    {
        //---------------------------------------
        // 게임 관련 객체를 렌더링 합니다.
        //---------------------------------------
        // TODO: Boss, Bullet, Player 렌더링

        // 남은 시간(mm:ss, 카운트다운)
        int minutes = (int)_remainingTime / 60;
        int seconds = (int)_remainingTime % 60;

        _timerFont?.DrawText($"{minutes:00}:{seconds:00}",
            new Rect(0, 10, GameGlobal.ScreenSize.Width, 30),
            new Color4(1f, 1f, 1f, 1f));
    }

    private void RenderGameOver()
    {
        string resultText = _isVictory ? "VICTORY!" : "GAME OVER";
        Color4 resultColor = _isVictory
            ? new Color4(0.3f, 1f, 0.4f, 1f)
            : new Color4(1f, 0.3f, 0.3f, 1f);

        _resultFont?.DrawText(resultText,
            new Rect(0, 230, GameGlobal.ScreenSize.Width, 60),
            resultColor);

        _guideFont?.DrawText("스페이스바를 눌러 재시작하세요!",
            new Rect(0, 340, GameGlobal.ScreenSize.Width, 40),
            new Color4(1f, 1f, 1f, 1f));
    }

    public override void Dispose()
    {
        base.Dispose();
        //---------------------------------------
        // 게임 관련 객체를 해제합니다.
        //---------------------------------------
        _titleFont?.Dispose();
        _guideFont?.Dispose();
        _timerFont?.Dispose();
        _resultFont?.Dispose();
    }
}