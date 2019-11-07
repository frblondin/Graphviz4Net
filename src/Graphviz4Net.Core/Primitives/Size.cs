using System;
using System.Collections.Generic;
using System.Text;

namespace Graphviz4Net.Primitives
{
    /// <summary>Implements a structure that is used to describe the <see cref="Size" /> of an object. </summary>
    public struct Size
    {
        /// <summary>Compares two instances of <see cref="Size" /> for equality. </summary>
        /// <param name="size1">The first instance of <see cref="Size" /> to compare.</param>
        /// <param name="size2">The second instance of <see cref="Size" /> to compare.</param>
        /// <returns>true if the two instances of <see cref="Size" /> are equal; otherwise <see langword="false" />.</returns>
        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
        }

        /// <summary>Compares two instances of <see cref="Size" /> for inequality. </summary>
        /// <param name="size1">The first instance of <see cref="Size" /> to compare.</param>
        /// <param name="size2">The second instance of <see cref="Size" /> to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if the instances of <see cref="Size" /> are not equal; otherwise <see langword="false" />.</returns>
        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }

        /// <summary>Compares two instances of <see cref="Size" /> for equality. </summary>
        /// <param name="size1">The first instance of <see cref="Size" /> to compare.</param>
        /// <param name="size2">The second instance of <see cref="Size" /> to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if the instances of <see cref="Size" /> are equal; otherwise, <see langword="false" />.</returns>
        public static bool Equals(Size size1, Size size2)
        {
            if (size1.IsEmpty)
            {
                return size2.IsEmpty;
            }
            return size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height);
        }

        /// <summary>Compares an object to an instance of <see cref="Size" /> for equality. </summary>
        /// <param name="o">The <see cref="T:System.Object" /> to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if the sizes are equal; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object o)
        {
            if (o == null || !(o is Size))
            {
                return false;
            }
            Size size = (Size)o;
            return Size.Equals(this, size);
        }

        /// <summary>Compares a value to an instance of <see cref="Size" /> for equality. </summary>
        /// <param name="value">The size to compare to this current instance of <see cref="Size" />.</param>
        /// <returns>
        ///     <see langword="true" /> if the instances of <see cref="Size" /> are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(Size value)
        {
            return Size.Equals(this, value);
        }

        /// <summary>Gets the hash code for this instance of <see cref="Size" />. </summary>
        /// <returns>The hash code for this instance of <see cref="Size" />.</returns>
        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }

        /// <summary>Initializes a new instance of the <see cref="Size" /> structure and assigns it an initial <paramref name="width" /> and <paramref name="height" />.</summary>
        /// <param name="width">The initial width of the instance of <see cref="Size" />.</param>
        /// <param name="height">The initial height of the instance of <see cref="Size" />.</param>
        public Size(double width, double height)
        {
            if (width < 0.0 || height < 0.0)
            {
                throw new ArgumentException();
            }
            this._width = width;
            this._height = height;
        }

        /// <summary>Gets a value that represents a static empty <see cref="Size" />. </summary>
        /// <returns>An empty instance of <see cref="Size" />.</returns>
        public static Size Empty
        {
            get
            {
                return Size.s_empty;
            }
        }

        /// <summary>Gets a value that indicates whether this instance of <see cref="Size" /> is <see cref="Size.Empty" />. </summary>
        /// <returns>
        ///     <see langword="true" /> if this instance of size is <see cref="Size.Empty" />; otherwise <see langword="false" />.</returns>
        public bool IsEmpty
        {
            get
            {
                return this._width < 0.0;
            }
        }

        /// <summary>Gets or sets the <see cref="Size.Width" /> of this instance of <see cref="Size" />. </summary>
        /// <returns>The <see cref="Size.Width" /> of this instance of <see cref="Size" />. The default value is 0. The value cannot be negative.</returns>
        public double Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException();
                }
                if (value < 0.0)
                {
                    throw new ArgumentException();
                }
                this._width = value;
            }
        }

        /// <summary>Gets or sets the <see cref="Size.Height" /> of this instance of <see cref="Size" />. </summary>
        /// <returns>The <see cref="Size.Height" /> of this instance of <see cref="Size" />. The default is 0. The value cannot be negative.</returns>
        public double Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException();
                }
                if (value < 0.0)
                {
                    throw new ArgumentException();
                }
                this._height = value;
            }
        }

        /// <summary>Explicitly converts an instance of <see cref="Size" /> to an instance of <see cref="Point" />. </summary>
        /// <param name="size">The <see cref="Size" /> value to be converted.</param>
        /// <returns>A <see cref="Point" /> equal in value to this instance of <see cref="Size" />.</returns>
        public static explicit operator Point(Size size)
        {
            return new Point(size._width, size._height);
        }

        private static Size CreateEmptySize()
        {
            return new Size
            {
                _width = double.NegativeInfinity,
                _height = double.NegativeInfinity
            };
        }

        internal double _width;

        internal double _height;

        private static readonly Size s_empty = Size.CreateEmptySize();
    }
}
