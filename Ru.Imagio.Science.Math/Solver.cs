using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ru.Imagio.Science.Math
{
    public class Solver
    {
        public Matrix Solve()
        {
            const int sizeX = 11;   //  число узлов разбиения сетки
            const int sizeY = 11; 

            const int lx = 1;   //  Размеры квадрата в сечении
            const int ly = 1;

            const int etta = 1; //  Сдвиговая вязкость
            const int tau = 1;  //  Время релаксации
            const double kappa = 0.12;  //  параметры модели учитывающие размеры
            const double betta = 0.1;   //  и форму клубка
            const double b = 3.0 * betta / tau; //  вспомогательный коэффициент

            const double Re = 3.8E-6;   //  число Рейнольдса

            const double A = 1.0 / Re; //  Градиент давления

            const double wMax = 0.25;   //  Максимальная скорость проскальзывания (на границе)

            const double dx = 1.0*lx/sizeX; //  Размер ячейки
            const double dy = 1.0*ly/sizeY;

            const double We = 1.0/5;    //  число Вайсенберга

            var wMatrix = new Matrix(sizeY, sizeX);   //  Скорость потока
            var wNext = new Matrix(sizeY, sizeX);

            var a11Matrix = new Matrix(sizeY, sizeX); //  тензор анизотропии
            var a12Matrix = new Matrix(sizeY, sizeX);
            var a13Matrix = new Matrix(sizeY, sizeX);
            var a22Matrix = new Matrix(sizeY, sizeX);
            var a23Matrix = new Matrix(sizeY, sizeX);
            var a33Matrix = new Matrix(sizeY, sizeX);

            const double dt = 1E-6; //  приращение времени для скорости потока

            const double endTime = 0.3E-3; //  конечное время эксперимента

            for (var time = 0.0; time < endTime; time += dt)
            {
                //  расчет скорости
                for (var x = 0; x < sizeX; x++)
                {
                    for (var y = 0; y < sizeY; y++)
                    {
                        if (x == 0 || y == 0)
                            //  на границе скорость всегда 0
                            wNext[x, y] = 0;
                        else
                        {
                            //  скорость внутри решетки
                            var da13dx = (a13Matrix[x + 1, y] - a13Matrix[x - 1, y])/dx/2;
                            var da23dy = (a23Matrix[x, y + 1] - a13Matrix[x, y - 1])/dy/2;

                            wNext[x, y] = wMatrix[x, y] + dt*(3*etta/tau*(da13dx + da23dy) + A);
                        }
                    }
                }
                wMatrix.CopyFrom(wNext);
                
                // расчет тензора
                for (var x = 0; x < sizeX; x++)
                {
                    for (var y = 0; y < sizeY; y++)
                    {
                        //  расчет частных производных скорости
                        var dwdx = 0.0;
                        var dwdy = 0.0;

                        switch (x)
                        {
                            case 0: //  левая граница
                                dwdx = (-3*wMatrix[x, y] + 4*wMatrix[x + 1, y] - wMatrix[x + 2, y])/dx/2;
                                break;
                            case sizeX - 1: //  правая граница
                                dwdx = (-3*wMatrix[x, y] + 4*wMatrix[x - 1, y] - wMatrix[x - 2, y])/dx/2;
                                break;
                            default:    //  внутри сетки
                                dwdx = (wMatrix[x + 1, y] - wMatrix[x - 1, y])/dx/2;
                                break;
                        }

                        switch (y)
                        {
                            case 0: //  верхняя граница
                                dwdy = (-3 * wMatrix[x, y] + 4 * wMatrix[x, y + 1] - wMatrix[x, y + 2]) / dy / 2;
                                break;
                            case sizeY - 1: //  нижняя граница
                                dwdx = (-3 * wMatrix[x, y] + 4 * wMatrix[x, y - 1] - wMatrix[x, y - 2]) / dy / 2;
                                break;
                            default:    //  внутри сетки
                                dwdx = (wMatrix[x, y + 1] - wMatrix[x, y - 1]) / dy / 2;
                                break;
                        }

                        var a11 = a11Matrix[x, y];
                        var a12 = a12Matrix[x, y];
                        var a13 = a13Matrix[x, y];
                        var a22 = a22Matrix[x, y];
                        var a23 = a23Matrix[x, y];
                        var a33 = a33Matrix[x, y];
                        var trace = a11 + a22 + a33;
                        var q = (1.0 + (kappa - betta)*trace)/tau;

                        a11Matrix[x, y] = (a11 - dt*b*(a11*a11 + a12*a12 + a13*a13)) / (1 + dt * q);
                        a22Matrix[x, y] = (a22 - dt*b*(a12*a12 + a22*a22 + a23*a23))/(1 + dt*q);
                        a33Matrix[x, y] = (a33 - dt*b*(a13*a13 + a23*a23 + a33*a33) + dt*We*(2*a13*dwdx + 2*a23*dwdy))/
                                          (1 + dt*q);
                        a12Matrix[x, y] = (a12 - dt*b*(a11*a12 + a12*a22 + a13*a23))/(1 + dt*q);
                        a13Matrix[x, y] = (a13 - dt*b*(a11*a13 + a12*a23 + a13*a33) + dt*We*1/3*dwdx +
                                           dt*We*(a11*dwdx + a12*dwdy))/(1 + dt*q);
                        a23Matrix[x, y] = (a23 - dt * b * (a12 * a13 + a22 * a23 + a23 * a33) + dt * We * 1 / 3 * dwdy +
                                           dt * We * (a12 * dwdx + a22 * dwdy)) / (1 + dt * q);
                    }
                }
            }

            return wMatrix;
        }
    }
}
