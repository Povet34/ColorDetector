using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷� �帧�� �߷�
/// </summary>
public class ColorFlowReasoner
{
    //�ٸ� ������ ����Ʈ�� ���̷���, ������ �ѹ� �������ϴ� ���
    //������ �ʰ� ������ �ٷ� ��ȭ�� ��� (��Ƽ�÷��� �������� ����.)

    public class Step
    {
        public int colorindex;
        public float density; //�ش� ������ ��
    }

    public class Inference 
    {
        public int chIndex;
        public List<Step> steps = new();
    }

    public List<Inference> Infer(Dictionary<int, List<Color32>> data)
    {
        List<Inference> inferences = new();
        const int brightnessThreshold = 50; // ��� �Ӱ谪(������ ����)

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
                    // ��Ⱑ �Ӱ谪 �̻��� �Ǵ� ������ ����
                    start = i;
                }
                else if (start != -1 && (brightness < brightnessThreshold || i == n - 1))
                {
                    // ��Ⱑ �Ӱ谪 ���Ϸ� �������ų� ������ ������
                    end = (brightness < brightnessThreshold) ? i - 1 : i;

                    if (end >= start)
                    {
                        // ���� ������ ���� ���� ������ ã��
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

                        // ���� ���� �������� ���� �ε��� �߷�
                        int colorIndex = FindColorIndex(colors[maxIndex]);

                        // ���� �� �� �����ӿ� ���� Step ���� (�ﰢ�� � ������ density)
                        int frameCount = end - start + 1;
                        if (frameCount == 1)
                        {
                            // ������ 1�������̸� density=1
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
                                    // ����: 0 ~ 1�� ���� ����
                                    int riseCount = maxIndex - start;
                                    density = (riseCount == 0) ? 1f : (float)j / riseCount;
                                }
                                else
                                {
                                    // ����: 1 ~ 0���� ���� ����
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

                    // ���� ���� Ž���� ���� �ʱ�ȭ
                    start = -1;
                    end = -1;
                }
            }
            inferences.Add(inference);
        }
        return inferences;
    }

    /// <summary>
    /// ��Ŭ���� �Ÿ��� �̿��� ���� ���� ã�� https://en.wikipedia.org/wiki/Color_difference
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
