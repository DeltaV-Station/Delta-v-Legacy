using Content.Shared.SimpleStation14.Traits.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client.SimpleStation14.Overlays.Shaders;

public sealed class NearsightedOverlay : Overlay
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;
    private readonly ShaderInstance _nearsightShader;

    public float Radius = 0f;
    private float _oldRadius = 0f;
    public float Darkness = 1f;

    private float _lerpTime = 0f;
    private float _lerpDuration = 0.25f;


    public NearsightedOverlay()
    {
        IoCManager.InjectDependencies(this);
        _nearsightShader = _prototypeManager.Index<ShaderPrototype>("GradientCircleMask").InstanceUnique();
    }


    protected override void Draw(in OverlayDrawArgs args)
    {
        // Check if the player has a NearsightedComponent and is controlling it
        if (!_entityManager.TryGetComponent(_playerManager.LocalPlayer?.ControlledEntity, out NearsightedComponent? nearComp) ||
            _playerManager.LocalPlayer?.ControlledEntity != nearComp.Owner)
            return;

        // Set the radius and darkness values based on whether the player is wearing glasses or not
        if (nearComp.Glasses)
        {
            Radius = nearComp.gRadius;
            Darkness = nearComp.gAlpha;
        }
        else
        {
            Radius = nearComp.Radius;
            Darkness = nearComp.Alpha;
        }

        // Check if the player has an EyeComponent and if the overlay should be drawn for this eye
        if (!_entityManager.TryGetComponent(_playerManager.LocalPlayer?.ControlledEntity, out EyeComponent? eyeComp) ||
            args.Viewport.Eye != eyeComp.Eye)
            return;


        var viewport = args.WorldAABB;
        var handle = args.WorldHandle;
        var distance = args.ViewportBounds.Width;

        var lastFrameTime = (float) _timing.FrameTime.TotalSeconds;

        // If the current radius value is different from the previous one, lerp between them
        if (!MathHelper.CloseTo(_oldRadius, Radius, 0.001f))
        {
            _lerpTime += lastFrameTime;
            var t = MathHelper.Clamp(_lerpTime / _lerpDuration, 0f, 1f); // Calculate lerp time
            _oldRadius = MathHelper.Lerp(_oldRadius, Radius, t); // Lerp between old and new radius values
        }
        // If the current radius value is the same as the previous one, reset the lerp time and old radius value
        else
        {
            _lerpTime = 0f;
            _oldRadius = Radius;
        }

        // Calculate the outer and inner radii based on the current radius value
        var outerMaxLevel = 0.6f * distance;
        var outerMinLevel = 0.06f * distance;
        var innerMaxLevel = 0.02f * distance;
        var innerMinLevel = 0.02f * distance;

        var outerRadius = outerMaxLevel - _oldRadius * (outerMaxLevel - outerMinLevel);
        var innerRadius = innerMaxLevel - _oldRadius * (innerMaxLevel - innerMinLevel);

        // Set the shader parameters and draw the overlay
        _nearsightShader.SetParameter("time", 0.0f);
        _nearsightShader.SetParameter("color", new Vector3(1f, 1f, 1f));
        _nearsightShader.SetParameter("darknessAlphaOuter", Darkness);
        _nearsightShader.SetParameter("innerCircleRadius", innerRadius);
        _nearsightShader.SetParameter("innerCircleMaxRadius", innerRadius);
        _nearsightShader.SetParameter("outerCircleRadius", outerRadius);
        _nearsightShader.SetParameter("outerCircleMaxRadius", outerRadius + 0.2f * distance);
        handle.UseShader(_nearsightShader);
        handle.DrawRect(viewport, Color.Black);

        handle.UseShader(null);
    }
}
