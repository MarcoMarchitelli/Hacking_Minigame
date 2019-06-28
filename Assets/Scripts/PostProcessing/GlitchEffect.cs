
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(GlitchEffectRenderer), PostProcessEvent.BeforeStack, "Custom/GlitchEffect")]
public class GlitchEffect : PostProcessEffectSettings
{
    public FloatParameter ChromAberrAmountX = new FloatParameter();
    public FloatParameter ChromAberrAmountY = new FloatParameter();
    public FloatParameter RightStripesAmount = new FloatParameter();
    public FloatParameter LeftStripesAmount = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter RightStripesFill = new FloatParameter();
    [Range(0,1)]
    public FloatParameter LeftStripesFill = new FloatParameter();
    public Vector2Parameter DisplacementAmount = new Vector2Parameter();
    public FloatParameter WavyDisplFreq = new FloatParameter();
    public FloatParameter Effect = new FloatParameter();
}