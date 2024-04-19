using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaycastingEngine
{
    public class Ray
    {
        public AVector2f p1; // Начало луча
        public AVector2f p2; // Конец луча



        // Создание луча разными способами
        public Ray() { }
        public Ray(AVector2f p1, AVector2f p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
        public Ray((AVector2f p1, AVector2f p2) points)
        {
            this.p1 = points.p1;
            this.p2 = points.p2;
        }
        public Ray(AVector2f origin, AVector2f direction, float magnitude)
        {
            (AVector2f p1, AVector2f p2) points = PointsFromRay(origin, direction, magnitude);
            this.p1 = points.p1;
            this.p2 = points.p2;
        }

        // Установка лучу точек начала и конца
        public void SetPoints((AVector2f p1, AVector2f p2) points)
        {
            p1 = points.p1;
            p2 = points.p2;
        }

        // Нахождение точек начала и конца из данных о начале луча, его направлении и длине
        public static (AVector2f p1, AVector2f p2) PointsFromRay(AVector2f origin, AVector2f direction, float magnitude) 
            => (origin, origin + direction.normalized * magnitude);



        // Нахождение пересечения отрезков через параметрические уравнения
        internal (bool exists, AVector2f point, float f) GetIntersectionPoint_ParametricEquations(AVector2f e1, AVector2f e2)
        {
            float s = ((e1.x - p1.x) * (p2.y - p1.y) - (e1.y - p1.y) * (p2.x - p1.x)) / ((e2.y - e1.y) * (p2.x - p1.x) - (e2.x - e1.x) * (p2.y - p1.y));
            float r = (s * (e2.y - e1.y) + (e1.y - p1.y)) / (p2.y - p1.y);


            if (0 > s || 1 < s || 0 > r || 1 < r) // No intersection
                return (false, default(AVector2f), -1);

            return (true, r * (p2 - p1) + p1, s);
        }

        // Нахождение пересечения отрезков через линейные функции
        internal (bool intersectionExists, AVector2f intersectionPoint) GetIntersectionPoint_LineFunctions(AVector2f e1, AVector2f e2)
        {
            // Let`s represent the ray and the edge as linear functions
            (float k, float b) ray = ToLinearFunction(p1, p2);
            (float k, float b) edge = ToLinearFunction(e1, e2);


            // If we will take both functions with special x argument, both of them will return same y values
            // Let`s take k and b as ray and k1 and b1 as edge. Also intersection point is p      (Linear function: y = kx + b)
            // So, p.y == k * p.x + b == k1 * p.x + b1
            // So, p.x = (b1 - b) / (k - k1)  !!! If k == k1, lines are parallel
            // As p belongs to ray and we know p.x, p.y is (k * p.x + b)

            if (ray.k == edge.k) // Lines are parallel
                return (intersectionExists: false, default(AVector2f));


            AVector2f intersectionPoint = new();

            // If any of lines are vertical
            if (Single.IsInfinity(ray.k))
            {
                intersectionPoint.x = p1.x;
                intersectionPoint.y = edge.k * p1.x + edge.b;
            }
            else if (Single.IsInfinity(edge.k))
            {
                intersectionPoint.x = e1.x;
                intersectionPoint.y = ray.k * e1.x + ray.b;
            }
            else
            {
                intersectionPoint.x = (edge.b - ray.b) / (ray.k - edge.k);
                intersectionPoint.y = ray.k * intersectionPoint.x + ray.b;
            }

            //if (id > 1920 / 2 - 5 && id < 1920 / 2 + 5)
            //    Core.Log($"id: {id} p1: {p1}  p2: {p2}  e1: {edgeP1}  e2: {edgeP2}" +
            //        $" intersect p: {intersectionPoint}  onLine: {/*!PointOnLineSegment(intersectionPoint, p1, p2) == false && */!PointOnLineSegment(intersectionPoint, edgeP1, edgeP2) == false}");

            if (PointOnLineSegment(intersectionPoint, p1, p2) == false || PointOnLineSegment(intersectionPoint, e1, e2) == false)
                return (intersectionExists: false, default(AVector2f)); // Intersection point is outside either ray or edge

            // Intersection point exists and belongs to ray
            return (intersectionExists: true, intersectionPoint);




            // Returns k and b values for linear function that passes through p1 and p2 point
            // Linear function: y = kx + b
            (float k, float b) ToLinearFunction(AVector2f p1, AVector2f p2)
            {
                AVector2f v = p2 - p1;
                float k = v.y / v.x;
                float b = p1.y - k * p1.x;
                return (k, b);
            }

            // Checks if point that belongs to line lays on line segment
            bool PointOnLineSegment(AVector2f point, AVector2f segmentStart, AVector2f segmentEnd)
            {
                float error = 0.001f; // without error PC can say that 0 != 0 for example
                bool xIsValid = MathF.Min(segmentStart.x, segmentEnd.x) - error <= point.x && point.x <= MathF.Max(segmentStart.x, segmentEnd.x) + error;
                bool yIsValid = MathF.Min(segmentStart.y, segmentEnd.y) - error <= point.y && point.y <= MathF.Max(segmentStart.y, segmentEnd.y) + error;

                return xIsValid && yIsValid;
            }
        }
    }
}
