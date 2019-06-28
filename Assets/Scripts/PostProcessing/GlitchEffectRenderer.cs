using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public sealed class GlitchEffectRenderer : PostProcessEffectRenderer<GlitchEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        Material mat = new Material(Shader.Find("Custom/GlitchEffectShader"));

        mat.SetFloat("_ChromAberrAmountX", settings.ChromAberrAmountX);
        mat.SetFloat("_ChromAberrAmountY", settings.ChromAberrAmountY);
        mat.SetFloat("_RightStripesAmount", settings.RightStripesAmount);
        mat.SetFloat("_RightStripesFill", settings.RightStripesFill);
        mat.SetFloat("_LeftStripesAmount", settings.LeftStripesAmount);
        mat.SetFloat("_LeftStripesFill", settings.LeftStripesFill);
        mat.SetVector("_DisplacementAmount", settings.DisplacementAmount);
        mat.SetFloat("_WavyDisplFreq", settings.WavyDisplFreq);
        mat.SetFloat("_GlitchEffect", settings.Effect);

        context.command.Blit(context.source, context.destination, mat);
    }
}