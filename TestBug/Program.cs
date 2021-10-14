using System;

namespace TestBug
{
    class Program
    {
        static void Main(string[] args)
        {
            var source1 = new int[] { 1, 2, 2, 2, 3, 4, 4, 5, 5, 5, 5, 5, 1 }.AsSpan();

            Console.WriteLine(source1.Test());
        }
    }

    static class Extenders
    {
        public static int Test(this Span<int> source)
        {
            return source.DetectElementSize();
        }
        
        static int DetectElementSize<T>(this Span<T> values) where T : struct
        {
            if (values.Length == 0) return 0;

            int elementSize = values[0] switch
            {
                <= 255 => 1,
                <= short.MaxValue => 2,
                <= int.MaxValue => 4,
                _ => 8
            };

            foreach (var b in values.Slice(1))
            {
                if (elementSize == 8) break;
                var currentElementSize = b switch
                {
                    <= 255 => 1,
                    <= short.MaxValue => 2,
                    <= int.MaxValue => 4,
                    _ => 8
                };

                elementSize = Math.Max(elementSize, currentElementSize);
            }

            return elementSize;
        }
    }
    
}