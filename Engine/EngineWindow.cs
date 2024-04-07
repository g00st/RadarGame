using App.Engine;
using App.Engine.ImGuisStuff;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Engine;

public class EngineWindow : GameWindow
{
    ImGuiController _controller;
    protected View MainView = new View();
    private const int TargetFPS = 60; // Set your target FPS here
    private DateTime _lastFrameTime;

    public EngineWindow(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { ClientSize = ( width, height), Title = "hi", Profile = ContextProfile.Core })
    {
        ErrorChecker.InitializeGLDebugCallback(); 
        _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
        this.Resize += e => this.resize();
    }
    
    
    
    void resize()
    {
        _controller.WindowResized(ClientSize.X, ClientSize.Y);
        MainView.Resize(ClientSize.X, ClientSize.Y);
    }
    
    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        _controller.PressChar((char)e.Unicode);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        _controller.MouseScroll(e.Offset);
    }
    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _controller.Update(this, (float)args.Time);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        MainView.draw();
            
        DrawInfo.darwImguiDebugWindow();
        ImGuiController.CheckGLError("End of frame");
        _controller.Render();
        this.SwapBuffers();

    }

}
