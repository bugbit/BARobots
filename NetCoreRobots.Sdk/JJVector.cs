/* JJVector class: see "extensions.txt" for details
 *
 * Author: Christo Fogelberg (doubtme@hotmail.com)
 * Additions: Alan Lund (alan.lund@acm.org)
 *
 * Built as an addon for JRobots, by Leonardo Boselli (boselli@uno.it)
 *
 * Planned additions: Set of static functions that have the same functionality
 * but don't return new JJVectors, instead taking one as an argument
 *
 * October, 2001. Please direct any comments or questions regarding this
 * class to Christo or Alan, comments regarding JRobots in general to
 * Leonardo :)
 */
/*
 * Convert to C# for Oscar Hernández Bañó
 */

using System;

namespace NetCoreRobots.Sdk
{
    public class JJVector
    {
        private double mX;
        private double mY;
        private double mT;

        public JJVector(double x, double y, double t)
        {
            mX = x;
            mY = y;
            mT = t;
        }

        /// <summary>
        /// Using set(JJVector) is _not_ the same as using "=". For JJVectors a and b,
        /// a = b;
        /// a.set(b);
        /// do different things - in the first case a points to the same JJVector as b.
        /// In the second, the value of the JJVector pointed to by a is the same as that
        /// pointed to by b - so they can be changed independently.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public JJVector(double x, double y) : this(x, y, 1) { }
        public JJVector(JJVector v) : this(v.mX, v.mY, v.mT) { }

        public JJVector() : this(0, 0, 1) { }

        public static JJVector Polar(double r, double a)
        {
            JJVector pR = new JJVector(r, 0);

            pR.rotateSelf(a);

            return pR;
        }

        public static JJVector Polar(double r, double a, double t)
        {
            JJVector pR = new JJVector(r, 0, t);

            pR.rotateSelf(a);

            return pR;
        }

        public void set_x(double x) => mX = x;
        public void set_y(double y) => mY = y;
        public void set_t(double t) => mT = t;

        public void set(double x, double y)
        {
            mX = x;
            mY = y;
        }

        public void set(double x, double y, double t)
        {
            mX = x;
            mY = y;
            mT = t;
        }

        public void set(JJVector v) => set(v.mX, v.mY, v.mT);

        public double x() => mX;

        public double y() => mY;

        public double t() => mT;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns the magnitude of the vector</returns>
        public double mag() => Math.Sqrt(mX * mX + mY * mY);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns the angle of the vector</returns>
        public double angle() => 180.0 * Math.Atan2(mY, mX) / Math.PI;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns the r (polar) coordinate of the vector(same as mag())</returns>
        public double r() => mag();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns the a (polar) coordinate of the vector(same as angle())</returns>
        public double a() => angle();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns>returns this . v, i.e. the dot product of this and v.</returns>
        public double dot(JJVector v) => mX * v.x() + mY * v.y();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns the speed (distance / time) represented by the vector; that is, mag() / t().</returns>
        public double speed() => mag() / mT;

        /// <summary>
        /// Computes the velocity vector, which is just the vector pointing
        /// in the same direction as "this", but with both time and distance
        /// scaled, such that result.t() == 1.  (See below for the
        /// usefulness of the second form.)
        /// </summary>
        /// <returns></returns>
        public JJVector velocity()
        {
            return velocity(new JJVector());
        }

        /// <summary>
        /// Computes the velocity vector, which is just the vector pointing
        /// in the same direction as "this", but with both time and distance
        /// scaled, such that result.t() == 1.  (See below for the
        /// usefulness of the second form.)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public JJVector velocity(JJVector result)
        {
            mult(1.0d / mT, result);
            result.set_t(1.0);

            return result;
        }

        public JJVector plus(JJVector v, JJVector result)
        {
            result.set(mX + v.x(), mY + v.y(), mT + v.t());

            return result;
        }

        public JJVector plus(JJVector v)
        {
            return plus(v, new JJVector());
        }

        public JJVector minus(JJVector v, JJVector result)
        {
            result.set(mX - v.x(), mY - v.y(), mT - v.t());
            return result;
        }

        public JJVector minus(JJVector v)
        {
            return minus(v, new JJVector());
        }

        public JJVector mult(double k, JJVector result)
        {
            result.set(mX * k, mY * k, mT * k);

            return result;
        }

        public JJVector mult(double k)
        {
            return mult(k, new JJVector());
        }

        /// <summary>
        /// Rotates this by "a" degrees and returns the result
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public JJVector rotate(double degrees, JJVector result)
        {
            result.set(this);
            result.rotateSelf(degrees);

            return result;
        }

        /// <summary>
        /// Rotates this by "a" degrees and returns the result
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public JJVector rotate(double degrees)
        {
            return rotate(degrees, new JJVector());
        }


        /// <summary>
        /// returns the distance beteween the two vectors. Will always be positive.Same as minus(v).mag().

        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        // Useful Functions
        public double dist(JJVector v)
        {
            var dx = mX - v.x();
            var dy = mY - v.y();

            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// returns the "unit-ised" vector of this.  This function works in the same way as plus, minus and mult.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public JJVector unit(JJVector result)
        {
            var invMag = 1.0d / mag();

            return mult(invMag, result);
        }

        /// <summary>
        ///  returns the "unit-ised" vector of this.  This function works in the same way as plus, minus and mult.
        /// </summary>
        /// <returns></returns>
        public JJVector unit()
        {
            return unit(new JJVector());
        }

        private void rotateSelf(double degrees)
        {
            var radians = Math.PI * degrees / 180.0;
            var newX = Math.Cos(radians) * mX - Math.Sin(radians) * mY;
            var newY = Math.Sin(radians) * mX + Math.Cos(radians) * mY;

            mX = newX;
            mY = newY;
        }
    }
}
