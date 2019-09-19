using System;
using System.Collections.Generic;
using System.Text;

namespace Graphviz4Net.Primitives
{
    /// <summary>Represents an x- and y-coordinate pair in two-dimensional space.</summary>
    public struct Point
    {
        /// <summary>Compares two <see cref="Point" /> structures for equality. </summary>
        /// <param name="point1">The first <see cref="Point" /> structure to compare.</param>
        /// <param name="point2">The second <see cref="Point" /> structure to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if both the <see cref="Point.X" /> and <see cref="Point.Y" /> coordinates of <paramref name="point1" /> and <paramref name="point2" /> are equal; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Point point1, Point point2)
        {
            return point1.X == point2.X && point1.Y == point2.Y;
        }

        /// <summary>Compares two <see cref="Point" /> structures for inequality. </summary>
        /// <param name="point1">The first point to compare.</param>
        /// <param name="point2">The second point to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="point1" /> and <paramref name="point2" /> have different <see cref="Point.X" /> or <see cref="Point.Y" /> coordinates; <see langword="false" /> if <paramref name="point1" /> and <paramref name="point2" /> have the same <see cref="Point.X" /> and <see cref="Point.Y" /> coordinates.</returns>
        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }

        /// <summary>Compares two <see cref="Point" /> structures for equality. </summary>
        /// <param name="point1">The first point to compare.</param>
        /// <param name="point2">The second point to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="point1" /> and <paramref name="point2" /> contain the same <see cref="Point.X" /> and <see cref="Point.Y" /> values; otherwise, <see langword="false" />.</returns>
        public static bool Equals(Point point1, Point point2)
        {
            return point1.X.Equals(point2.X) && point1.Y.Equals(point2.Y);
        }

        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is a <see cref="Point" /> and whether it contains the same coordinates as this <see cref="Point" />. </summary>
        /// <param name="o">The <see cref="T:System.Object" /> to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="o" /> is a <see cref="Point" /> and contains the same <see cref="Point.X" /> and <see cref="Point.Y" /> values as this <see cref="Point" />; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object o)
        {
            if (o == null || !(o is Point))
            {
                return false;
            }
            Point point = (Point)o;
            return Point.Equals(this, point);
        }

        /// <summary>Compares two <see cref="Point" /> structures for equality.</summary>
        /// <param name="value">The point to compare to this instance.</param>
        /// <returns>
        ///     <see langword="true" /> if both <see cref="Point" /> structures contain the same <see cref="Point.X" /> and <see cref="Point.Y" /> values; otherwise, <see langword="false" />.</returns>
        public bool Equals(Point value)
        {
            return Point.Equals(this, value);
        }

        /// <summary>Returns the hash code for this <see cref="Point" />.</summary>
        /// <returns>The hash code for this <see cref="Point" /> structure.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>Gets or sets the <see cref="Point.X" />-coordinate value of this <see cref="Point" /> structure. </summary>
        /// <returns>The <see cref="Point.X" />-coordinate value of this <see cref="Point" /> structure.  The default value is 0.</returns>
        public double X { get; private set; }

        /// <summary>Gets or sets the <see cref="Point.Y" />-coordinate value of this <see cref="Point" />. </summary>
        /// <returns>The <see cref="Point.Y" />-coordinate value of this <see cref="Point" /> structure.  The default value is 0.</returns>
        public double Y { get; private set; }

        /// <summary>Creates a new <see cref="Point" /> structure that contains the specified coordinates. </summary>
        /// <param name="x">The x-coordinate of the new <see cref="Point" /> structure. </param>
        /// <param name="y">The y-coordinate of the new <see cref="Point" /> structure. </param>
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>Offsets a point's <see cref="Point.X" /> and <see cref="Point.Y" /> coordinates by the specified amounts.</summary>
        /// <param name="offsetX">The amount to offset the point's
        ///       <see cref="Point.X" /> coordinate. </param>
        /// <param name="offsetY">The amount to offset thepoint's <see cref="Point.Y" /> coordinate.</param>
        public void Offset(double offsetX, double offsetY)
        {
            this.X += offsetX;
            this.Y += offsetY;
        }
    }
}
