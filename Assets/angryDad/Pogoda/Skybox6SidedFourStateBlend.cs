using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox6SidedFourStateBlend : MonoBehaviour
{
    [Header("Наш бленд-материал (шейдер: Skybox/Two6SidedBlend_Builtin)")]
    public Material blendMaterial;

    [Header("Материалы Skybox/6 Sided по порядку: 0=Ночь, 1=Утро, 2=День, 3=Вечер")]
    public Material[] sixSided = new Material[4];

    [Header("Источник времени (компонент с полем/свойством TimeOfDay = 0..24)")]
    public MonoBehaviour timeSource;
    public string timeField = "TimeOfDay";

    [Header("Окна плавных переходов (часы)")]
    public Vector2 nightToMorning = new Vector2(5f, 6f);    // ночь -> утро
    public Vector2 morningToDay = new Vector2(11f, 12f);  // утро -> день
    public Vector2 dayToEvening = new Vector2(17f, 18f);  // день -> вечер
    public Vector2 eveningToNight = new Vector2(23f, 0.5f); // вечер -> ночь (через 24->0!)

    [Header("Визуальные параметры (общие)")]
    public float exposure = 1f;
    [Range(0, 360)] public float rotationY = 0f;
    public Color tint = Color.white;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Фолбэк (если нет источника времени): сколько сек = сутки")]
    public float cycleSeconds = 600f;

    // ------ shader property IDs ------
    static readonly int _FrontTex = Shader.PropertyToID("_FrontTex");
    static readonly int _BackTex = Shader.PropertyToID("_BackTex");
    static readonly int _LeftTex = Shader.PropertyToID("_LeftTex");
    static readonly int _RightTex = Shader.PropertyToID("_RightTex");
    static readonly int _UpTex = Shader.PropertyToID("_UpTex");
    static readonly int _DownTex = Shader.PropertyToID("_DownTex");

    static readonly int _FrontA = Shader.PropertyToID("_FrontA");
    static readonly int _BackA = Shader.PropertyToID("_BackA");
    static readonly int _LeftA = Shader.PropertyToID("_LeftA");
    static readonly int _RightA = Shader.PropertyToID("_RightA");
    static readonly int _UpA = Shader.PropertyToID("_UpA");
    static readonly int _DownA = Shader.PropertyToID("_DownA");
    static readonly int _FrontB = Shader.PropertyToID("_FrontB");
    static readonly int _BackB = Shader.PropertyToID("_BackB");
    static readonly int _LeftB = Shader.PropertyToID("_LeftB");
    static readonly int _RightB = Shader.PropertyToID("_RightB");
    static readonly int _UpB = Shader.PropertyToID("_UpB");
    static readonly int _DownB = Shader.PropertyToID("_DownB");

    static readonly int _Exposure = Shader.PropertyToID("_Exposure");
    static readonly int _Rotation = Shader.PropertyToID("_Rotation");
    static readonly int _Blend = Shader.PropertyToID("_Blend");
    static readonly int _Tint = Shader.PropertyToID("_Tint");

    void OnEnable()
    {
        // проверки
        if (!blendMaterial) { Debug.LogError("Назначь blendMaterial с шейдером Two6SidedBlend_Builtin"); enabled = false; return; }
        if (sixSided.Length < 4 || sixSided[0] == null || sixSided[1] == null || sixSided[2] == null || sixSided[3] == null)
        { Debug.LogError("Заполни 4 Skybox/6 Sided материала (0=ночь,1=утро,2=день,3=вечер)."); enabled = false; return; }
        // сразу закрепим материал
        RenderSettings.skybox = blendMaterial;
    }

    IEnumerator Start()
    {
        // На всякий случай ждём 1 кадр, пока другие компоненты проинициализируются
        yield return null;

        RenderSettings.skybox = blendMaterial;
        ApplyCommon();

        // Инициализируемся корректной парой по текущему часу
        float hour = GetHour24();
        int a = StateByHour(hour), b = NextState(a);
        ApplyPair(a, b);
        blendMaterial.SetFloat(_Blend, 0f);
    }

    void Update()
    {
        float hour = GetHour24(); // 0..24 (может быть дробным)

        int a, b; float w; // текущая пара и прогресс бленда (0..1)

        if (InRange(hour, nightToMorning)) { a = 0; b = 1; w = Mathf.InverseLerp(nightToMorning.x, nightToMorning.y, hour); }
        else if (InRange(hour, morningToDay)) { a = 1; b = 2; w = Mathf.InverseLerp(morningToDay.x, morningToDay.y, hour); }
        else if (InRange(hour, dayToEvening)) { a = 2; b = 3; w = Mathf.InverseLerp(dayToEvening.x, dayToEvening.y, hour); }
        else if (InRangeWrap(hour, eveningToNight)) { a = 3; b = 0; w = ProgressAny(hour, eveningToNight); }
        else { a = StateByHour(hour); b = NextState(a); w = 0f; }

        // ЖЁСТКО подставляем правильную пару каждый кадр (чтобы ничего не "мигало")
        ApplyPair(a, b);

        // Плавный бленд
        blendMaterial.SetFloat(_Blend, curve.Evaluate(w));

        ApplyCommon();

        // Можно вызывать реже, если нужно сэкономить (например, раз в 0.5–1 сек)
        DynamicGI.UpdateEnvironment();
    }

    void LateUpdate()
    {
        // Подстраховка: если что-то в кадре сменило skybox — вернём наш
        if (RenderSettings.skybox != blendMaterial)
            RenderSettings.skybox = blendMaterial;
    }

    // ---------- helpers ----------

    void ApplyCommon()
    {
        blendMaterial.SetFloat(_Exposure, exposure);
        blendMaterial.SetFloat(_Rotation, rotationY);
        blendMaterial.SetColor(_Tint, tint);
    }

    void ApplyPair(int a, int b)
    {
        var A = sixSided[a];
        var B = sixSided[b];

        blendMaterial.SetTexture(_FrontA, A.GetTexture(_FrontTex));
        blendMaterial.SetTexture(_BackA, A.GetTexture(_BackTex));
        blendMaterial.SetTexture(_LeftA, A.GetTexture(_LeftTex));
        blendMaterial.SetTexture(_RightA, A.GetTexture(_RightTex));
        blendMaterial.SetTexture(_UpA, A.GetTexture(_UpTex));
        blendMaterial.SetTexture(_DownA, A.GetTexture(_DownTex));

        blendMaterial.SetTexture(_FrontB, B.GetTexture(_FrontTex));
        blendMaterial.SetTexture(_BackB, B.GetTexture(_BackTex));
        blendMaterial.SetTexture(_LeftB, B.GetTexture(_LeftTex));
        blendMaterial.SetTexture(_RightB, B.GetTexture(_RightTex));
        blendMaterial.SetTexture(_UpB, B.GetTexture(_UpTex));
        blendMaterial.SetTexture(_DownB, B.GetTexture(_DownTex));
    }

    // 0..24 часы (если нет источника — бежим по cycleSeconds)
    float GetHour24()
    {
        if (timeSource)
        {
            var tp = timeSource.GetType();
            object raw = null;

            var f = tp.GetField(timeField);
            if (f != null) raw = f.GetValue(timeSource);
            else
            {
                var p = tp.GetProperty(timeField);
                if (p != null) raw = p.GetValue(timeSource);
            }

            if (raw is float fv) return Mathf.Repeat(fv, 24f);
            if (raw is int iv) return Mathf.Repeat(iv, 24f);
        }
        // fallback: эмуляция суток
        float dur = Mathf.Max(1f, cycleSeconds);
        return (Time.time % dur) / dur * 24f;
    }

    // Определяем базовое состояние без бленда: 0..6 ночь, 6..12 утро, 12..18 день, 18..24 вечер
    int StateByHour(float h)
    {
        if (h < 6f) return 0;
        if (h < 12f) return 1;
        if (h < 18f) return 2;
        return 3;
    }
    int NextState(int a) => (a + 1) & 3;

    // Прямое окно (не пересекает 24->0)
    bool InRange(float h, Vector2 win) => win.y >= win.x && h >= win.x && h < win.y;

    // Wrap-окно: пересекает полуночь, например (23 .. 0.5)
    bool InRangeWrap(float h, Vector2 win)
    {
        if (win.y >= win.x) return false; // это не wrap-окно
        return (h >= win.x) || (h < win.y);
    }

    // Прогресс для окна, в том числе wrap
    float ProgressAny(float h, Vector2 win)
    {
        if (win.y >= win.x)
            return Mathf.InverseLerp(win.x, win.y, h);

        // wrap: переносим всё в развёрнутую шкалу
        float hy = (h < win.y) ? h + 24f : h;
        float y = win.y + 24f;
        return Mathf.InverseLerp(win.x, y, hy);
    }
}