using Microsoft.ML;
using web_fitness.Data;
using web_fitness.Models;
using System;
using System.IO;
using System.Linq;

namespace web_fitness.MeetingsClusterer
{
    public class Clusterer
    {
        private readonly fitnessdataContext _context;
        private MLContext _mlContext;
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "MeetingsClusteringModel.zip");
        private PredictionEngine<MeetingFeatures, MeetingPrediction> _predictor;
        private readonly int NUM_OF_CLUSTER = 4;

        public Clusterer(fitnessdataContext Context)
        {
            _context = Context;
            _mlContext = new MLContext(seed: 0);
        }

        public static MeetingFeatures ConvertToMeetingFeatures(Meeting meeting)
        {
            return new MeetingFeatures
            {
                TrainingTypeID = meeting.TrainingTypeID.ToString(),
                TrainerID = meeting.TrainerID.ToString(),
                MeetDate = meeting.MeetDate.ToShortDateString(),
                Price = meeting.Price.ToString()
            };
        }

        public void CreateModel()
        {
            IDataView dataView = _mlContext.Data.LoadFromEnumerable<MeetingFeatures>(_context.Meetings.Select(m => ConvertToMeetingFeatures(m)).ToList());
            string featuresColumnName = "Features";
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("TrainingTypeID_F", "TrainingTypeID")
                .Append(_mlContext.Transforms.Text.FeaturizeText("TrainerID_F", "TrainerID"))
                .Append(_mlContext.Transforms.Text.FeaturizeText("MeetDate_F", "MeetDate"))
                .Append(_mlContext.Transforms.Text.FeaturizeText("Price_F", "Price"))
                .Append(_mlContext.Transforms.Concatenate(featuresColumnName, "TrainingTypeID_F", "TrainerID_F", "MeetDate_F", "Price_F"))
                .Append(_mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: NUM_OF_CLUSTER));
            var model = pipeline.Fit(dataView);
            using var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write);
            _mlContext.Model.Save(model, dataView.Schema, fileStream);
            _predictor = _mlContext.Model.CreatePredictionEngine<MeetingFeatures, MeetingPrediction>(model);
        }

        public MeetingPrediction Predict(Meeting meeting)
        {
            MeetingFeatures mf = ConvertToMeetingFeatures(meeting);
            return _predictor.Predict(mf);
        }
    }
}
