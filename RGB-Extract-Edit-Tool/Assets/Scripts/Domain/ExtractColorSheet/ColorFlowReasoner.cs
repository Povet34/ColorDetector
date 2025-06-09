using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 컬러 흐름을 추론
/// </summary>
public class ColorFlowReasoner
{
    //다른 색상의 라이트를 보이려면, 색상이 한번 꺼져야하는 경우
    //꺼지지 않고 색상이 바로 변화는 경우 (멀티컬러는 구현하지 않음.)

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
        const int brightnessThreshold = 50; // 밝기 임계값(적절히 조정)

        foreach (var channel in data)
        {
            Inference inference = new();
            inference.chIndex = channel.Key;

            var colors = channel.Value;
            int n = colors.Count;
            int start = -1;
            int end = -1;

            for (int i = 0; i < n; i++)
            {
                int brightness = (colors[i].r + colors[i].g + colors[i].b) / 3;

                if (start == -1 && brightness >= brightnessThreshold)
                {
                    // 밝기가 임계값 이상이 되는 구간의 시작
                    start = i;
                }
                else if (start != -1 && (brightness < brightnessThreshold || i == n - 1))
                {
                    // 밝기가 임계값 이하로 떨어지거나 마지막 프레임
                    end = (brightness < brightnessThreshold) ? i - 1 : i;

                    if (end >= start)
                    {
                        // 구간 내에서 가장 밝은 프레임 찾기
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

                        // 가장 밝은 프레임의 색상 인덱스 추론
                        int colorIndex = FindColorIndex(colors[maxIndex]);

                        // 구간 내 각 프레임에 대해 Step 생성 (삼각형 곡선 형태의 density)
                        int frameCount = end - start + 1;
                        if (frameCount == 1)
                        {
                            // 구간이 1프레임이면 density=1
                            Step step = new()
                            {
                                colorindex = colorIndex,
                                density = 1f
                            };
                            inference.steps.Add(step);
                        }
                        else
                        {
                            for (int j = 0; j < frameCount; j++)
                            {
                                float density;
                                if (j <= (maxIndex - start))
                                {
                                    // 앞쪽: 0 ~ 1로 선형 증가
                                    int riseCount = maxIndex - start;
                                    density = (riseCount == 0) ? 1f : (float)j / riseCount;
                                }
                                else
                                {
                                    // 뒤쪽: 1 ~ 0으로 선형 감소
                                    int fallCount = end - maxIndex;
                                    density = (fallCount == 0) ? 1f : 1f - ((float)(j - (maxIndex - start)) / fallCount);
                                }
                                Step step = new()
                                {
                                    colorindex = colorIndex,
                                    density = density
                                };
                                inference.steps.Add(step);
                            }
                        }
                    }

                    // 다음 구간 탐색을 위해 초기화
                    start = -1;
                    end = -1;
                }
            }
            inferences.Add(inference);
        }
        return inferences;
    }

    /// <summary>
    /// 유클리드 거리를 이용한 유사 색상 찾기 https://en.wikipedia.org/wiki/Color_difference
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static int FindColorIndex(Color32 color)
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

        return closestIndex + TestColorSheet.FIRST_COLOR_INDEX;
    }
}
