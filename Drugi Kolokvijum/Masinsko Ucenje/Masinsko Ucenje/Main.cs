using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace Masinsko_Ucenje
{
    public partial class Main : Form
    {

        LinearRegression regression;
        KMeans kmeans;
        DBScan dbscan;
        string[] lines;

        enum Header
        {
            Indeks, Bodovi
        }

        public Main()
        {
            InitializeComponent();

            lines = File.ReadAllLines(@"./../../data/bodovi.csv");
            lines = lines.Skip(1).ToArray(); // skip header row (Indeks, Bodovi)
        }

        private void btnLinearRegression_Click(object sender, EventArgs e)
        {
            regression = new LinearRegression();
            List<double> x = new List<double>();
            List<double> y = new List<double>();

            // TODO 1: Ucitati i isparsirati skup podataka iz lines u x i y
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                x.Add(double.Parse(parts[(int)Header.Indeks]));
                y.Add(double.Parse(parts[(int)Header.Bodovi]));
            }


            // TODO 4: Izvršiti linearnu regresiju na primeru predviđanja stope 
            // smrtnosti od raka kože na osnovu geografske širine američkih država.
            regression.fit(x.ToArray(), y.ToArray());

            // draw regresiion line on a chart
            drawRegressionResults(x, y);
        }

        private void btnKmeans_Click(object sender, EventArgs e)
        {
            this.kmeans = new KMeans();
            clusteringHistory = new List<List<Cluster>>();

            List<Point> kmeansElements = new List<Point>();
            int k = Convert.ToInt32(tbK.Value);
            double toleracijaNaGresku = Convert.ToDouble(tbErr.Text);

            // TODO 8: Klasterovati na osnovu broja bodova
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                double lat = double.Parse(parts[(int)Header.Indeks]);
                double lng = double.Parse(parts[(int)Header.Bodovi]);
                kmeansElements.Add(new Point(lat, lng));
            }

            //
            this.kmeans.elementi = kmeansElements;
            this.kmeans.podeliUGrupe(k, toleracijaNaGresku);

            // draw clustering results on a chart
            drawClusteringResults();
        }

        private void btnDBScan_Click(object sender, EventArgs e)
        {
            // Vrlo verovatno dolazi
            this.dbscan = new DBScan();
            clusteringHistory = new List<List<Cluster>>();

            List<Point> dbscanElements = new List<Point>();
            double E = Convert.ToDouble(tbE.Text);
            Console.WriteLine("epsilon:" + E);

            lines = File.ReadAllLines(@"./../../data/dbscan.tsv");
            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');
                double lat = double.Parse(parts[(int)Header.Indeks]);
                double lng = double.Parse(parts[(int)Header.Bodovi]);
                dbscanElements.Add(new Point(lat, lng));
            }

            this.dbscan.elementi = dbscanElements;
            this.dbscan.podeliUGrupe(E);

            drawClusteringResultsDB();
        }

        #region GUI_Functions
        private void drawRegressionResults(List<double> X, List<double> Y)
        {
            RegressionChart.Visible = true;
            ClusteringChart.Visible = false;
            RegressionChart.Series.Clear();

            Series diagramLimitsSeries = new Series("DiagramLimits");
            diagramLimitsSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            diagramLimitsSeries.Points.AddXY(X.Min(), Y.Min());
            diagramLimitsSeries.Points.AddXY(X.Max(), Y.Max());
            diagramLimitsSeries.Points[0].IsEmpty = true;
            diagramLimitsSeries.Points[1].IsEmpty = true;
            diagramLimitsSeries.IsVisibleInLegend = false;
            RegressionChart.Series.Add(diagramLimitsSeries);
            RegressionChart.Update();


            // Create a point series.
            Series pointSeries = new Series("Tacke");
            pointSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            RegressionChart.Series.Add(pointSeries);

            for (int i = 0; i < X.Count; i++)
            {
                pointSeries.Points.AddXY(X[i], Y[i]);
                //Thread.Sleep(5);
                RegressionChart.Update();
            }

            // Create a line series.
            string lineLabel = "";
            if (regression.n > 0)
                lineLabel = "Regresiona prava: y=" + Math.Round(regression.k,2) + "*x + " + Math.Round(regression.n,2);
            else
                lineLabel = "Regresiona prava: y=" + Math.Round(regression.k,2) + "*x - " + Math.Round(Math.Abs(regression.n),2);
            Series lineSeries = new Series(lineLabel);
            lineSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            lineSeries.Points.AddXY(X.Max(), regression.n + X.Max() * regression.k);
            lineSeries.Points.AddXY(X.Min(), regression.n + X.Min() * regression.k);
            RegressionChart.Series.Add(lineSeries);
            RegressionChart.Update();
        }

        public static List<List<Cluster>> clusteringHistory = new List<List<Cluster>>();

        private void drawClusteringResults()
        {
            ClusteringChart.Visible = true;
            RegressionChart.Visible = false;

            foreach (List<Cluster> clusterList in clusteringHistory)
            {
                ClusteringChart.Series.Clear();

                // visualize each cluster
                for (int i = 0; i < kmeans.brojGrupa; i++ )
                {
                    // Create a point series.
                    Series pointSeries = new Series("Klaster " + i+1);
                    pointSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

                    // add cluster center and mark it differently
                    pointSeries.Points.AddXY(clusterList[i].centar.x, clusterList[i].centar.y);
                    pointSeries.Points[0].MarkerSize = pointSeries.Points[0].MarkerSize * 2;
                    pointSeries.Points[0].MarkerBorderColor = Color.Black;
                    pointSeries.Points[0].MarkerBorderWidth = 3;

                    for (int j = 0; j < clusterList[i].elementi.Count; j++)
                    {
                        pointSeries.Points.AddXY(clusterList[i].elementi[j].x, clusterList[i].elementi[j].y);
                    }
                    

                    ClusteringChart.Series.Add(pointSeries);
                }

                ClusteringChart.Update();
                Thread.Sleep(500);
                
            }
        }
        private void drawClusteringResultsDB()
        {
            ClusteringChart.Visible = true;
            RegressionChart.Visible = false;

 
            ClusteringChart.Series.Clear();
            for (int i = 0; i < dbscan.grupe.Count; i++)
            {
                Series pointSeries = new Series("Klaster " + i + 1);
                    pointSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    for (int j = 0; j < this.dbscan.grupe[i].elementi.Count; j++)
                    {
                        pointSeries.Points.AddXY(this.dbscan.grupe[i].elementi[j].x, this.dbscan.grupe[i].elementi[j].y);
                    }
                    ClusteringChart.Series.Add(pointSeries);
                }
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion GUI_Functions
    }
}
