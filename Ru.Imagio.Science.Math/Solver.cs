using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ru.Imagio.Science.Math
{
    public class Solver
    {
        public void Solve()
        {
            const int sizeX = 11;   //  число узлов разбиения сетки
            const int sizeY = 11; 

            const int lx = 1;   //  Размеры квадрата в сечении
            const int ly = 1;

            const int etta = 1; //  Сдвиговая вязкость
            const int tau = 1;  //  Время релаксации

            const double A = 1; //  Градиент давления

            const double wMax = 0.25;   //  Максимальное значение скорости

            var dx = 1.0*lx/sizeX;  //  Размер ячейки
            var dy = 1.0*ly/sizeY;

            var We = 1.0/5;

            var w = new Matrix(sizeY, sizeX);   //  Скорость потока
            var wNext = new Matrix(sizeY, sizeX);

            var a11 = new Matrix(sizeY, sizeX); //  тензор анизотропии
            var a12 = new Matrix(sizeY, sizeX);
            var a13 = new Matrix(sizeY, sizeX);
            var a22 = new Matrix(sizeY, sizeX);
            var a23 = new Matrix(sizeY, sizeX);
            var a33 = new Matrix(sizeY, sizeX);

            var a11Next = new Matrix(sizeY, sizeX); //  тензор
            var a12Next = new Matrix(sizeY, sizeX);
            var a13Next = new Matrix(sizeY, sizeX);
            var a22Next = new Matrix(sizeY, sizeX);
            var a23Next = new Matrix(sizeY, sizeX);
            var a33Next = new Matrix(sizeY, sizeX);

            var dt_w = 1E-6;    //  приращение времени для скорости потока
            var dt_a = 1E-2;    //  приращение времени для тензора

            var endTime = 0.3E-3;   //  конечное время эксперимента

            for (var time = 0.0; time < endTime; time += dt_w)
            {
                //  скорость внутри решетки
                for (var x = 1; x < sizeX - 1; x++)
                {
                    for (var y = 1; y < sizeY - 1; y++)
                    {
                        var da13dx = (a13[x + 1, y] - a13[x - 1, y])/dx/2;
                        var da23dy = (a23[x, y + 1] - a13[x, y - 1])/dy/2;

                        wNext[x, y] = w[x, y] + dt_w * (3 * etta / tau * (da13dx + da23dy) + A);
                    }
                }


            }
        }
    }
}
