using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 컬러 흐름을 추론
/// </summary>
public class ColorFlowReasoner
{
    public class Step
    {
        public int colorindex;
        public float density; //해당 프레임 농도
    }

    public class Inference
    {
        public int chIndex;
        public List<Step> steps = new();
    }

    public List<Inference> Infer(Dictionary<int, List<Color32>> data)
    {
        List<Inference> inferences = new();
        const int brightnessThreshold = 120;

        foreach (var channel in data)
        {
            Inference inference = new();
            inference.chIndex = channel.Key;
            var colors = channel.Value;

            var segments = FindSegments(colors, brightnessThreshold);

            foreach (var (start, end, isBright) in segments)
            {
                int colorIndex;
                int maxIndex = start;
                if (isBright)
                {
                    maxIndex = FindMaxBrightnessIndex(colors, start, end);
                    colorIndex = FindColorIndex(colors[maxIndex]);
                }
                else
                {
                    colorIndex = 0; // 임계치 미만 구간은 0번 인덱스
                }
                var steps = CreateSteps(start, end, maxIndex, colorIndex);
                inference.steps.AddRange(steps);
            }

            inferences.Add(inference);
        }
        return inferences;
    }

    /// <summary>
    /// 밝기 임계값을 기준으로 구간(시작, 끝 인덱스) 리스트 반환
    /// </summary>
    /// <param name="colors"></param>
    /// <param name="brightnessThreshold"></param>
    /// <returns></returns>
    private List<(int start, int end, bool isBright)> FindSegments(List<Color32> colors, int brightnessThreshold)
    {
        List<(int, int, bool)> segments = new();
        int n = colors.Count;
        int start = 0;
        bool? currentBright = null;

        for (int i = 0; i < n; i++)
        {
            int brightness = Definitions.GetPerceivedBrightness(colors[i]);
            bool isBright = brightness >= brightnessThreshold;

            if (currentBright == null)
            {
                currentBright = isBright;
                start = i;
            }
            else if (currentBright != isBright)
            {
                // 구간 종료
                segments.Add((start, i - 1, currentBright.Value));
                start = i;
                currentBright = isBright;
            }

            // 마지막 프레임 처리
            if (i == n - 1)
            {
                segments.Add((start, i, currentBright.Value));
            }
        }
        return segments;
    }

    /// <summary>
    /// 구간 내에서 가장 밝은 프레임의 인덱스 반환
    /// </summary>
    /// <param name="colors"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private int FindMaxBrightnessIndex(List<Color32> colors, int start, int end)
    {
        int maxBrightness = int.MinValue;
        int maxIndex = start;
        for (int j = start; j <= end; j++)
        {
            int b = colors[j].r + colors[j].g + colors[j].b;
            if (b > maxBrightness)
            {
                maxBrightness = b;
                maxIndex = j;
            }
        }
        return maxIndex;
    }

    // 삼각형 곡선 형태의 density로 Step 리스트 생성
    private List<Step> CreateSteps(int start, int end, int maxIndex, int colorIndex)
    {
        List<Step> steps = new();
        int frameCount = end - start + 1;
        if (frameCount == 1)
        {
            steps.Add(new Step { colorindex = colorIndex, density = 1f });
        }
        else
        {
            for (int j = 0; j < frameCount; j++)
            {
                float density;
                if (j <= (maxIndex - start))
                {
                    int riseCount = maxIndex - start;
                    density = (riseCount == 0) ? 1f : (float)j / riseCount;
                }
                else
                {
                    int fallCount = end - maxIndex;
                    density = (fallCount == 0) ? 1f : 1f - ((float)(j - (maxIndex - start)) / fallCount);
                }
                steps.Add(new Step { colorindex = colorIndex, density = density });
            }
        }
        return steps;
    }

    private static int FindColorIndex(Color32 color)
    {
        int closestIndex = -1;
        int minDistance = int.MaxValue;

        for (int i = 0; i < TestColorSheet.Colors.Length; i++)
        {
            Color32 c = TestColorSheet.Colors[i];
            int dr = color.r - c.r;
            int dg = color.g - c.g;
            int db = color.b - c.b;
            int da = color.a - c.a;
            int distance = dr * dr + dg * dg + db * db + da * da;

            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
